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
using IMRP.Enums;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP.Database.Collections
{
    public class PlayerInventory
    {
        public static IMongoCollection<PlayerInventory> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int CharacterId { get; set; }
        public List<InventoryItem> Items { get; set; }

        public void AddNew()
        {
            //PlayerInventoryId = GetNextID();
            Items = new List<InventoryItem>();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {CharacterId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<PlayerInventory>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {CharacterId} DELETED");
            }
        }

        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<PlayerInventory>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {CharacterId} UPDATED");
                }
            }
        }
        public static PlayerInventory GetByID(int characterID)
        {
            var res = collection.Find(c => c.CharacterId == characterID).FirstOrDefault();
            if (res == null) return null;
            return res;
        }

        public static int GetNextIDAsync(int characterID)
        {
            PlayerInventory inv = collection.Find(c => c.CharacterId == characterID).FirstOrDefault();

            if (inv.Items.Count == 0)
            {
                return 0;
            }
            else
            {
                
                var nextID = inv.Items.Count;
                return nextID;
            }
        }
    }
}
