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
    public class ColShapeCylinder
    {
        public static IMongoCollection<ColShapeCylinder> collection; //Initalized from player.cs

        [BsonId]
        public ObjectId Id { get; set; }
        public int ColShapeCylinderId { get; set; }
        public IMVector3 Position { get; set; }
        public float Range { get; set; }
        public float Height { get; set; }
        public uint Dimension { get; set; }

        public Enums.ColShapeType ColShapeType { get; set; }

        public ColShapeCylinder(Enums.ColShapeType colshapeType)
        {
            ColShapeType = colshapeType;
        }

        public void AddNew()
        {
            ColShapeCylinderId = GetNextID();
            collection.InsertOne(this);
            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {ColShapeCylinderId} ADDED");
        }

        public void Delete()
        {
            if (Id != null)
            {
                var filter = Builders<ColShapeCylinder>.Filter.Eq("_id", Id);
                collection.DeleteOne(filter);
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {ColShapeCylinderId} DELETED");
            }
        }
        public static List<ColShapeCylinder> GetAll()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public void Update()
        {
            if (Id != null)
            {
                var filter = Builders<ColShapeCylinder>.Filter.Eq("_id", Id);
                var result = collection.ReplaceOne(filter, this);
                if (result.IsModifiedCountAvailable)
                {
                    Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"{collection.CollectionNamespace.CollectionName} {ColShapeCylinderId} UPDATED");
                }
            }
        }
        public static ColShapeCylinder GetByID(int groundId)
        {
            return collection.Find(c => c.ColShapeCylinderId == groundId).FirstOrDefault();
        }

        private static int GetNextID()
        {
            List<ColShapeCylinder> coll = collection.FindSync(new BsonDocument()).ToList();

            if (coll.Count == 0)
            {
                return 0;
            }
            else
            {
                var maxID = (from row in coll orderby row.ColShapeCylinderId descending select row.ColShapeCylinderId).FirstOrDefault() + 1;
                return maxID;
            }
        }
    }
}
