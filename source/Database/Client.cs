using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using IMRP.Database.Collections;
using System.Reflection;
using System.Linq;

namespace IMRP.Database
{
    class player
    {
        public static MongoClient mongoClient;
        public static IMongoDatabase mongoDatabase;
        public void Initalize(string mongoHost, string mongoPort, string databaseName)
        {
            try
            {
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, "Starting IMRP...");
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, "Connecting to database...");

                mongoClient = new MongoClient($"mongodb://{mongoHost}:{mongoPort}");
                mongoDatabase = mongoClient.GetDatabase(databaseName);

                List<BsonDocument> collectionList = mongoDatabase.ListCollections().ToList();
                List<string> collNameOnly = (from row in collectionList select row["name"].ToString()).ToList();

                string nspace = "IMRP.Database.Collections";

                var collectionClasses = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.IsClass && t.Namespace == nspace
                        select t;
                collectionClasses.ToList();

                foreach(var record in collectionClasses)
                {
                    if(!record.Name.Contains("<") && !record.Name.Contains(">") && !collNameOnly.Contains(record.Name))
                    {
                        CreateCollectionOptions options = new CreateCollectionOptions();
                        mongoDatabase.CreateCollection(record.Name, options);
                    }
                }

                Account.collection = mongoDatabase.GetCollection<Account>("Account");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Account Collection Initalized.");

                Character.collection = mongoDatabase.GetCollection<Character>("Character");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Character Collection Initalized.");

                BankAccount.collection = mongoDatabase.GetCollection<BankAccount>("BankAccount");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "BankAccount Collection Initalized.");

                Vehicle.collection = mongoDatabase.GetCollection<Vehicle>("Vehicle");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Vehicle Collection Initalized.");

                PlayerInventory.collection = mongoDatabase.GetCollection<PlayerInventory>("PlayerInventory");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "PlayerInventory Collection Initalized.");

                GroundItem.collection = mongoDatabase.GetCollection<GroundItem>("GroundItem");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "GroundItem Collection Initalized.");

                ColShapeCylinder.collection = mongoDatabase.GetCollection<ColShapeCylinder>("ColShapeCylinder");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "ColShapeCylinder Collection Initalized.");

                Marker.collection = mongoDatabase.GetCollection<Marker>("Marker");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Marker Collection Initalized.");

                Property.collection = mongoDatabase.GetCollection<Property>("Property");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Property Collection Initalized.");

                JobConfiguration.collection = mongoDatabase.GetCollection<JobConfiguration>("JobConfiguration");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "JobConfiguration Collection Initalized.");

                ServerLog.collection = mongoDatabase.GetCollection<ServerLog>("ServerLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "ServerLog Collection Initalized.");

                AccountLog.collection = mongoDatabase.GetCollection<AccountLog>("AccountLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "AccountLog Collection Initalized.");

                CharacterLog.collection = mongoDatabase.GetCollection<CharacterLog>("CharacterLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "CharacterLog Collection Initalized.");

                EconomyLog.collection = mongoDatabase.GetCollection<EconomyLog>("EconomyLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "EconomyLog Collection Initalized.");

                DatabaseLog.collection = mongoDatabase.GetCollection<DatabaseLog>("DatabaseLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "DatabaseLog Collection Initalized.");

                StaffLog.collection = mongoDatabase.GetCollection<StaffLog>("StaffLog");
                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "StaffLog Collection Initalized.");

                Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, $"Connected to Database {databaseName} on {mongoHost}:{mongoPort}");

            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.DatabaseError, $"{ex.Message} {ex.StackTrace}");
            }
        }
    }
}
