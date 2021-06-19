using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IMRP.Database.Collections;
using IMRP.Models;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class CharacterCreatorEvents : Script
    {
        [RemoteEvent("finishcharacter")]
        public async Task CreateCharacter(Player player, params object[] arguments)
        {
            PlayerData.IsPlayerAuthenticated(player);
            Account account = Account.GetByID(player.GetData<int>("AccountId"));

            string firstName = (string)arguments[0];
            string lastName = (string)arguments[1];
            int age = Convert.ToInt32(arguments[2]);
            string customization = (string)arguments[3];
            try
            {
                CustomizerData customizeData = JsonConvert.DeserializeObject<CustomizerData>(customization);

                Character newCharacter = new Character();
                newCharacter.AccountId = account.AccountId;
                newCharacter.CreatedOn = DateTime.UtcNow;
                newCharacter.CustomizationData = customizeData;
                newCharacter.DateOfBirth = DateTime.UtcNow.AddYears((-1*age));
                newCharacter.FirstName = firstName;
                newCharacter.LastName = lastName;
                newCharacter.MoneyOnHand = 3000.00m;
                newCharacter.LastPosition = new IMVector3(new Vector3(56.33918, -269.1009, 48.18818));
                newCharacter.LastRotation = new IMVector3(new Vector3(0, 0, 161.4992));
                newCharacter.AddNew();

                //Create a Bank Account for the character and add 3,000 to them
                newCharacter = Character.GetByID(newCharacter.CharacterId);
                BankAccount newBankAccount = new BankAccount();
                newBankAccount.Balance = 0;
                newBankAccount.EntityId = newCharacter.CharacterId;
                newBankAccount.OwnershipType = Enums.OwnershipType.Player;
                newBankAccount.OpenedOn = DateTime.UtcNow;
                newBankAccount.AddNew();

                newBankAccount.Balance = newBankAccount.Balance+3000;
                newBankAccount.EntityId = newCharacter.CharacterId;
                newBankAccount.OwnershipType = Enums.OwnershipType.Player;
                newBankAccount.OpenedOn = DateTime.UtcNow;

                BankAccount.BankTransaction transaction = new BankAccount.BankTransaction();
                transaction.Amount = 3000;
                transaction.CreditedTo = newBankAccount.BankAccountId.ToString();
                transaction.DebitedFrom = "ACH Deposit";
                transaction.TransacitonType = BankAccount.TransactionType.Deposit;

                Util.Logging.Log(Util.Logging.LogType.Economy, $"Server {transaction.TransacitonType} ${transaction.Amount} to bank account #: {newBankAccount.BankAccountId}. Reason: New player");

                newBankAccount.Transactions.Add(transaction);
                newBankAccount.Update();

                //Set character's primary bank account
                newCharacter.PrimaryBankID = newBankAccount.BankAccountId;
                newCharacter.Update();

                //Give the player a blank inventory.
                Database.Collections.PlayerInventory pinventory = new Database.Collections.PlayerInventory();
                pinventory.CharacterId = newCharacter.CharacterId;
                pinventory.AddNew();

                Util.Logging.Log(Util.Logging.LogType.CharacterInfo, $"{account.Username} has created a new character {firstName} {lastName}");
                player.TriggerEvent("transitionToSelector");
            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError,$"IMRP.Events.CreateCharacter {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
