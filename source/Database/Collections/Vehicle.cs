using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using GTANetworkAPI;
using IMRP.Enums;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace IMRP.Database.Collections
{
    public class Vehicle
    {
        public static IMongoCollection<Vehicle> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int VehicleId { get; set; }
        public OwnershipType OwershipType { get; set; }
        public int OwnerId { get; set; }
        public VehicleHash Model {get; set;}
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public int Color1 { get; set; }
        public int Color2 { get; set; }
        public string NumberPlate { get; set; }
        public byte Alpha { get; set; }
        public bool Locked { get; set; }
        public bool Engine { get; set; }
        public uint Dimension { get; set; } = 1;
        public int Fuel { get; set; } = 100;
        public float Health { get; set; }


        
        public void AddNew()
        {
            VehicleId = GetNextID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {VehicleId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<Vehicle>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {VehicleId} DELETED");
            }
        }

        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<Vehicle>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"${collection.CollectionNamespace.CollectionName} {VehicleId} UPDATED");
                }
            }
        }
        public static List<Vehicle> GetVehiclesByAccount(int vehicleId)
        {
            return collection.Find(c => c.VehicleId == vehicleId).ToList();
        }
        public static Vehicle GetByID(int id)
        {
            return collection.Find(c => c.VehicleId == id).FirstOrDefault();
        }

        public static List<Vehicle> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        private static int GetNextID()
        {
            List<Vehicle> allChars = collection.FindSync(new BsonDocument()).ToList();

            if (allChars.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in allChars orderby row.VehicleId descending select row.VehicleId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
