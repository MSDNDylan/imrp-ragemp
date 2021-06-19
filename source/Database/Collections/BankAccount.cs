using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP.Database.Collections
{
    public class BankAccount
    {
        public static IMongoCollection<BankAccount> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public Enums.OwnershipType OwnershipType { get; set; }
        public int EntityId { get; set; } //Character or Group
        public int BankAccountId { get; set; }
        public DateTime OpenedOn { get; set; }
        public decimal Balance { get; set; }
        public List<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();

        public void AddNew()
        {
            try
            {
                BankAccountId = GetNextID();
                collection.InsertOne(this);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {BankAccountId} ADDED");
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in adding record to collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }

        public async Task Delete()
        {
            try
            {
                if (Id != null)
                {
                    var filter = Builders<BankAccount>.Filter.Eq("_id", Id);
                     collection.DeleteOneAsync(filter);
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {BankAccountId} DELETED");
                }
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in deleting record in collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }

        public void Update()
        {
            try
            {
                if (Id != null)
                {
                    BankAccount account = GetByID(BankAccountId);
                    account.Transactions = (from row in account.Transactions orderby row.TransactionDate descending select row).Take(20).ToList();

                    var filter = Builders<BankAccount>.Filter.Eq("_id", Id);
                    var result = collection.ReplaceOne(filter, this);
                    if (result.IsModifiedCountAvailable)
                    {
                        Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {BankAccountId} UPDATED");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in updating record in collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }
        public static BankAccount GetByID(int id)
        {
            return collection.Find(c => c.BankAccountId == id).FirstOrDefault();
        }

        public static List<BankAccount> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public enum TransactionType
        {
            Withdraw,
            Deposit
        }
        public class BankTransaction
        {
            public int TransactionId { get; set; }
            public TransactionType TransacitonType { get; set; } //Withdraw or Deposit
            public string DebitedFrom { get; set; } //ATM or player On Hand or Job
            public string CreditedTo { get; set; } //players Account
            public decimal Amount { get; set; }
            public DateTime TransactionDate { get; set; }
            public BankTransaction()
            {
                TransactionDate = DateTime.UtcNow;
            }
        }
        private static int GetNextID()
        {
            List<BankAccount> coll = collection.FindSync(new BsonDocument()).ToList();
            List<int> AccountNumbers = (from row in coll orderby row.BankAccountId descending select row.BankAccountId).ToList();

            if (coll.Count == 0)
            {
                return 733582931;
            }
            else
            {
                restart:
                Random generator = new Random();
                int r = Convert.ToInt32(generator.Next(700000000, 999999999).ToString("D9"));

                if(AccountNumbers.Contains(r))goto restart;

                return r;
            }
        }
    }
}
