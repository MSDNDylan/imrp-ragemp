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
    public class Marker
    {
        public static IMongoCollection<Marker> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int MarkerId { get; set; }
        public uint MarkerType { get; set; }
        public IMVector3 Position { get; set; }
        public IMVector3 Direction { get; set; }
        public IMVector3 Rotation { get; set; }
        public float Scale { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Alpha { get; set; }
        public bool BobUpAndDown { get; set; }
        public uint Dimension { get; set; }

        public void AddNew()
        {
            MarkerId = GetNextID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {MarkerId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<Marker>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {MarkerId} DELETED");
            }
        }
        public static List<Marker> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<Marker>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {MarkerId} UPDATED");
                }
            }
        }
        public static Marker GetByID(int groundId)
        {
            return collection.Find(c => c.MarkerId == groundId).FirstOrDefault();
        }

        private static int GetNextID()
        {
            List<Marker> coll = collection.FindSync(new BsonDocument()).ToList();

            if (coll.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in coll orderby row.MarkerId descending select row.MarkerId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
