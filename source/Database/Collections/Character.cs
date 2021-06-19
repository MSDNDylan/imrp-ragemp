using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using GTANetworkAPI;
using IMRP.Models;
using System.Linq;
using IMRP.Enums;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP.Database.Collections
{
    public class Character
    {
        public static IMongoCollection<Character> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public int StrangerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal MoneyOnHand { get; set; } = 0.0m;
        public DateTime CreatedOn { get; set; }
        public DateTime LastActive { get; set; }
        public CustomizerData CustomizationData { get; set; }

        public IMVector3 LastPosition { get; set; }
        public IMVector3 LastRotation { get; set; }
        public int PrimaryBankID { get; set; }

        public bool CharacterDeleted { get; set; } = false;

        public bool IsInLimbo { get; set; } = false;
        public int Health { get; set; } = 100;
        public int Armor { get; set; } = 0;
        public int Hunger { get; set; } = 100;
        public int Thirst { get; set; } = 100;

        public JobType CurrentJob { get; set; } = JobType.None;

        public Dictionary<int, string> Aliases { get; set; } = new Dictionary<int, string>();


        //Job Stuff
        public int MiningTruckStoredOres { get; set; }


        [BsonIgnore]
        public DateTime TimeOfSpawn { get; set; }

        [BsonIgnore]
        public DateTime LastHungerDecrease { get; set; }
        [BsonIgnore]
        public DateTime LastThirstDecrease { get; set; }


        [BsonIgnore]
        public DateTime NewbChatCoolDown { get; set; }

        [BsonIgnore]
        public bool CanAcceptNativeUI { get; set; } = false;

        [BsonIgnore]
        public bool Seatbelt { get; set; } = false;

        [BsonIgnore]
        public string StaffName { get; set; } = "";

        [BsonIgnore]
        public bool StaffMode { get; set; } = false;

        [BsonIgnore]
        public Modules.Staff.PermissionLevel PermissionLevel { get; set; } = Modules.Staff.PermissionLevel.None;

        public void AddNew()
        {
            CharacterId =  GetNextID();
            StrangerId = GetNextStrangerID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {CharacterId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<Character>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {CharacterId} DELETED");
            }
        }

        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<Character>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {CharacterId} UPDATED");
                }
            }
        }
        public static List<Character> GetCharactersByAccount(int accountId)
        {
            return collection.Find(c => c.AccountId == accountId && !c.CharacterDeleted).ToList();
        }
        public static Character GetByID(int id)
        {
            return collection.Find(c => c.CharacterId == id && !c.CharacterDeleted).FirstOrDefault();
        }

        public static List<Character> GetAll()
        {
            return collection.Find(c => !c.CharacterDeleted).ToList();
        }
        private static int GetNextID()
        {
            List<Character> allChars =  GetAll();

            if (allChars.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in allChars orderby row.CharacterId descending select row.CharacterId).FirstOrDefault() + 1;
                return maxID;
            }
        }
        private static int GetNextStrangerID()
        {
            List<Character> allChars = GetAll();
            List<int> allIDs = (from row in allChars orderby row.StrangerId descending select row.StrangerId).ToList();

            if (allChars.Count == 0)
            {
                return 9727348;
            }
            else
            {
                restart:
                Random generator = new Random();
                string r = generator.Next(1000000, 9999999).ToString("D7");
                int newID = Convert.ToInt32(r);
                foreach(int id in allIDs)
                {
                    if(newID == id)
                    {
                        goto restart;
                    }
                }

                return newID;
            }
        }
    }
}
