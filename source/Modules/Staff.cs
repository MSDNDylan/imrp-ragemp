using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using IMRP.Database.Collections;
using System.Threading.Tasks;

namespace IMRP.Modules
{
    public class Staff : Script
    {
        [Command("getaccountid")]
        public static async Task GetAccountId(Player player, int characterId)
        {
            try
            {
                Character character = PlayerData.players[player];
                if (!IsSufficentStaffLevel(player, PermissionLevel.Moderator)) return;

                List<Player> players = NAPI.Pools.GetAllPlayers();

                bool accountIdFound = false;
                foreach (Player c in players)
                {
                    Character targetCharacter = PlayerData.GetCharacter(c);
                    if (targetCharacter.CharacterId == characterId)
                    {
                        player.SendChatMessage($"Account ID of player: {targetCharacter.AccountId}");
                        accountIdFound = true;
                    }
                }
                if (!accountIdFound) player.SendChatMessage("Account Id not found!");
            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"Error /AccountId {ex.Message} {ex.StackTrace}");
            }
        }
        [Command("setstaffname")]
        public static void SetStaffNameAsync(Player player, string staffName, int accountId = -1)
        {
            if (!IsSufficentStaffLevel(player, PermissionLevel.Developer)) return;

            if(accountId > -1)
            {
                Account targetAcct = Account.GetByID(accountId);
                targetAcct.StaffName = staffName;
                targetAcct.Update();
                Util.ChatMessage.SendNotification(player, $"You have set {targetAcct.Username} staff name to: {staffName}");

                Dictionary<NetHandle, Character> onlineplayers = PlayerData.players;
                foreach (NetHandle handle in onlineplayers.Keys)
                {
                    Player tc = NAPI.Entity.GetEntityFromHandle<Player>(handle);
                    if (onlineplayers[tc].AccountId == accountId)
                    {
                        if (PlayerData.players.ContainsKey(tc))
                        {
                            PlayerData.players[tc].StaffName = staffName;
                            Util.ChatMessage.SendNotification(tc, $"{PlayerData.players[player].StaffName} your staff rank has been set to {Enum.GetName(typeof(PermissionLevel), targetAcct.PermissionLevel)}!");
                            break;
                        }
                    }
                }
            }
            else
            {
                Account targetAcct = Account.GetByID(PlayerData.players[player].AccountId);
                targetAcct.StaffName = staffName;
                targetAcct.Update();

                PlayerData.players[player].StaffName = staffName;
                PlayerData.players[player].Update();
                Util.ChatMessage.SendNotification(player, $"Your new staff name is: {staffName}");
            }
        }
        [Command("setstafflevel")]
        public static async Task SetStaffLevelAsync(Player player, int accountId, PermissionLevel permLevel)
        {
            if (!IsSufficentStaffLevel(player, PermissionLevel.Administrator)) return;
           /* if (!IsValidPermLevel(level))
            {
                Util.ChatMessage.SendErrorChatMessage(player, $"The permission {level} level specified is invalid.");
                player.SendChatMessage("----Available Staff Ranks----");
                foreach (int pl in Enum.GetValues(typeof(PermissionLevel)))
                {
                    player.SendChatMessage($"{Enum.GetName(typeof(PermissionLevel),pl)} = {pl}");
                }
                return;
            }*/

            Account targetAcct = Account.GetByID(accountId);
            foreach(PermissionLevel pl in Enum.GetValues(typeof(PermissionLevel)))
            {
                if(pl == permLevel)
                {
                    if ((int)pl >= (int)PlayerData.players[player].PermissionLevel)
                    {
                        Util.ChatMessage.SendErrorChatMessage(player, $"You can not set a staff rank equal or greater than your own.");
                        return;
                    }
                    targetAcct.PermissionLevel = (PermissionLevel)pl;
                    targetAcct.Update();


                    Dictionary<NetHandle, Character> onlineplayers = PlayerData.players;
                    foreach(NetHandle handle in onlineplayers.Keys)
                    {
                        Player tc = NAPI.Entity.GetEntityFromHandle<Player>(handle);

                        if(onlineplayers[tc].AccountId == accountId)
                        {
                            if (PlayerData.players.ContainsKey(tc))
                            {
                                PlayerData.players[tc].PermissionLevel = targetAcct.PermissionLevel;
                                Util.ChatMessage.SendNotification(tc, $"{PlayerData.players[player].StaffName} your staff rank has been set to {Enum.GetName(typeof(PermissionLevel), targetAcct.PermissionLevel)}!");
                                break;
                            }
                        }
                    }


                    Util.ChatMessage.SendNotification(player, $"You've set {targetAcct.Username}'s permission level to {permLevel}");
                }
            }
        }
        public static bool IsSufficentStaffLevel(Player player, PermissionLevel requiredLevel, bool staffModeRequired = false)
        {
            if (staffModeRequired)
            {
                if (!PlayerData.players[player].StaffMode)
                {
                    Util.ChatMessage.SendErrorChatMessage(player, "You are not in Staff Mode!");
                    return false;
                }
            }

            if (PlayerData.players[player].PermissionLevel >= requiredLevel)
            {
                return true;
            }
            else
            {
                Util.ChatMessage.SendErrorChatMessage(player, "Insufficent staff permissions");
                return false;
            }
        }
        public static bool IsValidPermLevel(int permLevel)
        {
            bool validPermLevel = false;
            foreach(int pl in Enum.GetValues(typeof(PermissionLevel)))
            {
                if(pl == permLevel)
                {
                    validPermLevel = true;
                }
            }
            return validPermLevel;
        }
        public enum PermissionLevel
        {
            None = 0,
            Helper = 10,
            Moderator = 50,
            Administrator = 100,
            Developer = 500
        }
    }
}
