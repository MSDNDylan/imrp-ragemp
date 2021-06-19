using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMRP.Database.Collections;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnplayerAuthentication : Script
    {
        [RemoteEvent("login")]
        public async Task playerLogin(Player player, params object[] arguments)
        {
            player.SendChatMessage("Attempting to Login... Please wait.");
            var inputUser = (string)arguments[0];
            var inputPass = (string)arguments[1];
            Account account = Account.GetByUsername(inputUser);
            if(account == null)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} attempted to login non-existant account with username {inputUser}");
                player.SendChatMessage("~r~Login Failed:~w~ Invalid username/Password!");
                player.SendNotification("~r~Login Failed:~w~ Invalid username/Password!");
                player.TriggerEvent("show_login_browser");
                return;
            }

            inputPass = BCrypt.Net.BCrypt.HashPassword(inputPass, account.Salt);
 
            if (inputPass != account.Password)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} attempted to login to {account.Username} with an invalid password.");
                player.SendChatMessage("~r~Login Failed:~w~ Invalid username/Password!");
                player.SendNotification("~r~Login Failed:~w~ Invalid username/Password!");
                player.TriggerEvent("show_login_browser");
                return;
            }
            else
            {
                account.LastOnline = DateTime.UtcNow;
                if (!account.HardwareID.Contains(player.Serial)) account.HardwareID.Add(player.Serial);
                if (!account.IPAddress.Contains(player.Address) && player.Address != "127.0.0.1") account.IPAddress.Add(player.Address);
                account.Update();

                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} has logged into {account.Username}");
                player.TriggerEvent("login_finished");
                player.Position = new Vector3(-904.0031, -363.1187, 113.0742);
                player.Rotation = new Vector3(0, 0, 209.2697);

                HeadBlend head = new HeadBlend();
                head.ShapeFirst = 0;
                head.ShapeSecond = 0;
                NAPI.Player.UpdatePlayerHeadBlend(player, 0,0,0);

                player.UpdateHeadBlend(0,0,0);
                player.SetData("Authenticated", true);
                player.SetData("AccountId", account.AccountId);
                player.TriggerEvent("initalizeCharacterSelector", account.Username);
                //player.TriggerEvent("initalizeCharacterCreator");
            }

        }
        [RemoteEvent("register")]
        public async Task playerRegister(Player player, params object[] arguments)
        {
            player.SendChatMessage("Attempting to Register Account... Please wait.");
            try
            {
                var inputUser = (string)arguments[0];
                var inputEmail = (string)arguments[1];
                var inputPass = (string)arguments[2];

                Account account =  Account.GetByUsername(inputUser);
                if (account != null)
                {
                    Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} attempted to register existing username {inputUser}");

                    player.SendChatMessage("~r~Registration Failed:~w~ Account already exists!");
                    player.SendNotification("~r~Registration Failed:~w~ Account already exists!");
                    player.TriggerEvent("show_login_browser");
                    return;
                }
                else
                {
                    int workFactor = ((DateTime.Now.Year - 2000) / 2) + 6;
                    string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
                    inputPass = BCrypt.Net.BCrypt.HashPassword(inputPass, salt);

                    Account newAccount = new Account();
                    newAccount.Username = inputUser;
                    newAccount.Password = inputPass;
                    newAccount.Email = inputEmail;
                    newAccount.Salt = salt;
                    newAccount.CharacterLimit = 3;
                    newAccount.JoinDate = DateTime.UtcNow;
                    newAccount.LastOnline = DateTime.UtcNow;
                    newAccount.HardwareID.Add(player.Serial);
                    newAccount.PermissionLevel = 0;
                    if(player.Address != "127.0.0.1")newAccount.IPAddress.Add(player.Address);
                     newAccount.AddNew();

                    Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} has created a new account {inputUser}");
                    Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} has logged into {inputUser}");
                    player.SendNotification("Registration Successful!");
                    player.TriggerEvent("login_finished");
                    player.SetData("Authenticated", true);
                    player.SetData("AccountId", newAccount.AccountId);
                    player.TriggerEvent("initalizeCharacterSelector", newAccount.Username);
                }
            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"{ex.Message} {ex.StackTrace}");
            }
        }
    }
}
