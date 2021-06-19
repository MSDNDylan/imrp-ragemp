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
    public class EconomyLog
    {
        public static IMongoCollection<EconomyLog> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }

        public Util.Logging.LogType LogType { get; set; }
        public string Message { get; set; }

        public bool WasError { get; set; } = false;
        public void AddNew()
        {
            collection.InsertOne(this);
        }
    }
}
