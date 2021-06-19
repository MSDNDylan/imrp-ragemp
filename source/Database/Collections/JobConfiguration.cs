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

namespace IMRP.Database.Collections
{
    public class JobConfiguration
    {
        public static IMongoCollection<JobConfiguration> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public Enums.JobType Job { get; set; }
        public IMVector3 JobEmployPoint { get; set; }

        public uint JobEmployPointBlip { get; set; }

        public int JobEmployPointMarkerId { get; set; } = -1;
        public int JobEmployColShape3DId { get; set; } = -1;
        public IMVector3 JobVehicleSpawnPoint { get; set; } // Vehicle Spawn Point
        public IMVector3 JobVehicleSpawnPointRot { get; set; }
        public decimal MinPayOut { get; set; }

        public decimal MaxPayOut { get; set; }

        public List<IMVector3> PickupPoints = new List<IMVector3>(); //List of MarkerIDs from Collections.Marker.cs


        public void AddNew()
        {
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {Job} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<JobConfiguration>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {Job} DELETED");
            }
        }
        public static List<JobConfiguration> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<JobConfiguration>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {Job} UPDATED");
                }
            }
        }
        public static JobConfiguration GetByID(Enums.JobType job)
        {
            return collection.Find(c => c.Job == job).FirstOrDefault();
        }
    }
}
