using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP.Database.Collections
{
    public class Account
    {
        public static IMongoCollection<Account> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public int CharacterLimit { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastOnline { get; set; }
        public Modules.Staff.PermissionLevel PermissionLevel { get; set; }
        public List<string> HardwareID = new List<string>();

        public List<string> IPAddress = new List<string>();

        public string StaffName { get; set; }

        [BsonIgnore]
        public bool StaffMode { get; set; } = false;


        public void AddNew()
        {
            try
            {
                StaffName = "ImpactStaff";
                PermissionLevel = Modules.Staff.PermissionLevel.None;

                AccountId = GetNextID();
                collection.InsertOneAsync(this);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {AccountId} ADDED");
            }
            catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in adding new record to collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }

        public void Delete()
        {
            try
            {
                if (Id != null)
                {
                    var filter = Builders<Account>.Filter.Eq("_id", Id);
                    collection.DeleteOne(filter);
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {AccountId} DELETED");
                }
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in deleting record to collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }

        public void Update()
        {
            try
            {
                if (Id != null)
                {
                    var filter = Builders<Account>.Filter.Eq("_id", Id);
                    var result = collection.ReplaceOne(filter, this);
                    if (result.IsModifiedCountAvailable)
                    {
                        Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {AccountId} UPDATED");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"DBError in Updating record to collection {collection.CollectionNamespace.CollectionName} {ex.StackTrace} {ex.Message}");
            }
        }
        public static Account GetByID(int id)
        {
            return collection.Find(c => c.AccountId == id).FirstOrDefault();
        }
        public static Account GetByUsername(string username)
        {
            return collection.Find(c => c.Username == username).FirstOrDefault();
        }
        public static List<Account> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }

        private static int GetNextID()
        {
            List<Account> coll = collection.FindSync(new BsonDocument()).ToList();

            if (coll.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in coll orderby row.AccountId descending select row.AccountId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
