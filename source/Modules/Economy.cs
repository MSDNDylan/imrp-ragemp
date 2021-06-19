using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using IMRP.Database.Collections;

namespace IMRP.Modules
{
    public class Economy
    {
        public async Task AddMoneyToPlayerPrimaryAcctAsync(Player player, decimal amount, string creditedTo, string debitedFrom, Enums.TaxType taxType)
        {
            BankAccount bankAccount = BankAccount.GetByID(PlayerData.players[player.Handle].PrimaryBankID);
            bankAccount.Balance = bankAccount.Balance + TakeTax(taxType, amount);

            BankAccount.BankTransaction transaction = new BankAccount.BankTransaction();
            transaction.TransacitonType = BankAccount.TransactionType.Deposit;
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.Amount = amount;
            transaction.CreditedTo = creditedTo;
            transaction.DebitedFrom = debitedFrom;
            bankAccount.Transactions.Add(transaction);
            bankAccount.Update();
        }

        public async Task RemoveMoneyToPlayerPrimaryAcctAsync(Player player, decimal amount, string creditedTo, string debitedFrom)
        {
            BankAccount bankAccount = BankAccount.GetByID(PlayerData.players[player.Handle].PrimaryBankID);
            bankAccount.Balance = bankAccount.Balance - amount;

            BankAccount.BankTransaction transaction = new BankAccount.BankTransaction();
            transaction.TransacitonType = BankAccount.TransactionType.Withdraw;
            transaction.TransactionDate = DateTime.UtcNow;
            transaction.Amount = amount;
            transaction.CreditedTo = creditedTo;
            transaction.DebitedFrom = debitedFrom;
            bankAccount.Transactions.Add(transaction);
            bankAccount.Update();
        }

        public decimal TakeTax(Enums.TaxType taxType, decimal amount)
        {
            decimal newAmount = amount;

            switch(taxType)
            {
                case Enums.TaxType.Food:
                    break;
                case Enums.TaxType.Gas:
                    break;
                case Enums.TaxType.GeneralSale:
                    break;
                case Enums.TaxType.Income:
                    break;
                case Enums.TaxType.Property:
                    break;
                case Enums.TaxType.Tobacco:
                    break;
                case Enums.TaxType.VehicleSale:
                    break;
                case Enums.TaxType.TaxExempt:
                    newAmount = amount;
                    break;
            }

            return newAmount;
        }
    }
}
