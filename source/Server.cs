using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using GTANetworkAPI;
using MongoDB.Bson;
using System.Collections.Generic;
using IMRP.Database.Collections;
using System.Threading;

namespace IMRP
{
    public class Server : Script
    {
        private string mongoHost = "localhost";
        private string mongoPort = "27017";
        private string databaseName = "IMRP";

        public static bool ServerInitalizing = true;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStart()
        {
            try
            {
                Database.player dbplayer = new Database.player();
                dbplayer.Initalize(mongoHost, mongoPort, databaseName);

                Util.Logging.Log(Util.Logging.LogType.ServerInfo, "IMRP Started.");
                NAPI.Server.SetGlobalServerChat(false);
                ServerInitalizing = false;


                //Spawn Markers
                List< Database.Collections.Marker> markers = Database.Collections.Marker.GetAll();
                foreach(Database.Collections.Marker marker in markers)
                {
                    NAPI.Task.Run(async () =>
                    {
                        GTANetworkAPI.Marker newm = NAPI.Marker.CreateMarker(marker.MarkerType, marker.Position.GetVector3(), marker.Direction.GetVector3(), marker.Rotation.GetVector3(), marker.Scale, new Color(marker.Red, marker.Green, marker.Blue, marker.Alpha), marker.BobUpAndDown, marker.Dimension);
                        ServerData.Markers.Add(newm.Handle, marker);
                        Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Created Marker ID: {marker.MarkerId}");
                    });
                }

                List<ColShapeCylinder> colshapes = ColShapeCylinder.GetAll();
                foreach(Database.Collections.ColShapeCylinder col in colshapes)
                {
                    GTANetworkAPI.ColShape newc = NAPI.ColShape.CreateCylinderColShape(col.Position.GetVector3(), col.Range, col.Height, col.Dimension);
                    ServerData.ColShapes.Add(newc.Handle, col);
                    Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Created ColShape ID: {col.ColShapeCylinderId}");
                }
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Initalization of ColShapes Complete.");
                List<JobConfiguration> jobs = JobConfiguration.GetAll();
                foreach(JobConfiguration job in jobs)
                {
                        Blip blip = NAPI.Blip.CreateBlip(job.JobEmployPoint.GetVector3(), 0);
                        blip.Sprite = 408;
                        blip.Name = Enum.GetName(typeof(Enums.JobType), job.Job);
                }
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Initalization of jobs Complete.");
                CancellationToken cancellation;
                Util.Timers.NewTimer(1000, cancellation);
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Initalization complete.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
        [ServerEvent(Event.ResourceStop)]
        public void ResourceStop()
        {
            List<Player> clients = NAPI.Pools.GetAllPlayers();
            foreach(Player c in clients)
            {
                Character character = PlayerData.GetCharacter(c);
                if (character == null) continue;
                character.LastPosition = new IMVector3(c.Position);
                character.LastRotation = new IMVector3(c.Rotation);
                character.Health = c.Health;
                character.Armor = c.Armor;
                character.Update();
            }

            Util.Logging.Log(Util.Logging.LogType.DatabaseInfo, "Resource stopping. Saved all active player data.");
        }
    }
}
