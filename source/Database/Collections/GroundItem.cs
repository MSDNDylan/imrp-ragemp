using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using GTANetworkAPI;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using IMRP.Models;

namespace IMRP.Database.Collections
{
    public class GroundItem
    {
        public static IMongoCollection<GroundItem> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int GroundId { get; set; }
        public List<IMRP.Models.GroundItem> Items { get; set; }

        public void AddNew()
        {
            GroundId = GetNextID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {GroundId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<GroundItem>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {GroundId} DELETED");
            }
        }

        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<GroundItem>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {GroundId} UPDATED");
                }
            }
        }
        public static GroundItem GetByID(int groundId)
        {
            return collection.Find(c => c.GroundId == groundId).FirstOrDefault();
        }

        private static int GetNextID()
        {
            List<GroundItem> coll = collection.FindSync(new BsonDocument()).ToList();

            if (coll.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in coll orderby row.GroundId descending select row.GroundId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
