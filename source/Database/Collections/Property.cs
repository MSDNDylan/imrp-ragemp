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
    public class Property
    {
        public static IMongoCollection<Property> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int PropertyId { get; set; }
        public OwnershipType OwershipType { get; set; }
        public int OwnerId { get; set; }
        public PropertyClass PropertyClass { get; set; }
        public PropertyType PropertyType { get; set; }
        public string PropertyName { get; set; }
        public string PropertyAddress { get; set; }
        public List<PropertyDoor> PropertyDoors { get; set; } = new List<PropertyDoor>();

        public void AddNew()
        {
            PropertyId = GetNextID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {PropertyId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<Property>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {PropertyId} DELETED");
            }
        }

        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<Property>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {PropertyId} UPDATED");
                }
            }
        }
        public static List<Property> GetPropertysByAccount(int PropertyId)
        {
            return collection.Find(c => c.PropertyId == PropertyId).ToList();
        }
        public static Property GetByID(int id)
        {
            return collection.Find(c => c.PropertyId == id).FirstOrDefault();
        }

        public static List<Property> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        private static int GetNextID()
        {
            List<Property> all = collection.FindSync(new BsonDocument()).ToList();

            if (all.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in all orderby row.PropertyId descending select row.PropertyId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
