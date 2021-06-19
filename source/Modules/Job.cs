using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Modules
{
    public class Job : Script
    {
        [Command("createjob")]
        public async Task CreateJobAsync(Player player, Enums.JobType job, decimal minPay, decimal maxPay)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            Database.Collections.JobConfiguration jobexist = Database.Collections.JobConfiguration.GetByID(job);
            if(jobexist != null)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "Job already exists");
                return;
            }

            Database.Collections.JobConfiguration jobConfiguration = new Database.Collections.JobConfiguration();
            jobConfiguration.Job = job;
            jobConfiguration.MaxPayOut = maxPay;
            jobConfiguration.MinPayOut = minPay;
            jobConfiguration.AddNew();

            Util.ChatMessage.SendNotification(player, $"Job: {job} created!");
        }

        [Command("setemploypoint")]
        public async Task CreateEmployPointAsync(Player player, Enums.JobType job, uint markerType = 1, float scale = 1.0f, int red = 255, int green = 255, int blue = 255, int alpha = 255, float range = 0.5f, float rangeheight = 6f, float directionX = 0.0f, float directionY = 0.0f, float directionZ = 0.0f)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            Database.Collections.JobConfiguration jobConfiguration = Database.Collections.JobConfiguration.GetByID(job);
            if (jobConfiguration == null)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "Job does not exist exists");
                return;
            }

            float groundz = player.Position.Z;

            groundz =  Util.GroundZ.GetGroundZ(player);

           // float groundz = Task.WaitAny(Util.GroundZ.GetGroundZ(player));

            jobConfiguration.JobEmployPoint = new IMVector3(new Vector3(player.Position.X, player.Position.Y, groundz));
            jobConfiguration.Update();

            Database.Collections.Marker dbmarker = Database.Collections.Marker.GetByID(jobConfiguration.JobEmployPointMarkerId);

            Database.Collections.ColShapeCylinder dbcolShape = Database.Collections.ColShapeCylinder.GetByID(jobConfiguration.JobEmployColShape3DId);

            if(dbmarker != null)
            {
                dbmarker.MarkerType = markerType;
                dbmarker.Position = new IMVector3(new Vector3(player.Position.X, player.Position.Y, groundz));
                dbmarker.Rotation = new IMVector3(new Vector3(0, 0, 0));
                dbmarker.Direction = new IMVector3(new Vector3(directionX, directionY, directionZ));
                dbmarker.Red = red;
                dbmarker.Blue = blue;
                dbmarker.Green = green;
                dbmarker.Alpha = alpha;
                dbmarker.Scale = scale;
                dbmarker.BobUpAndDown = false;
                dbmarker.Dimension = player.Dimension;
                dbmarker.Update();

                if(dbcolShape != null)
                {
                    dbcolShape.Position = new IMVector3(player.Position);
                    dbcolShape.Range = range;
                    dbcolShape.Height = rangeheight;
                    dbcolShape.Dimension = player.Dimension;
                    dbcolShape.Update();

                    foreach (var kvp in ServerData.ColShapes.ToArray())
                    {
                        if (ServerData.ColShapes[kvp.Key].ColShapeCylinderId == jobConfiguration.JobEmployColShape3DId)
                        {
                            NAPI.Entity.DeleteEntity(kvp.Key);
                            ServerData.ColShapes.Remove(kvp.Key);

                            NAPI.Task.Run(() =>
                            {
                                ColShape shape = NAPI.ColShape.CreateCylinderColShape(dbcolShape.Position.GetVector3(), dbcolShape.Range, dbcolShape.Height, dbcolShape.Dimension);
                                ServerData.ColShapes.Add(shape, dbcolShape);
                            });
                        }
                    }
                }

                foreach (var kvp in ServerData.Markers.ToArray())
                {
                    if (ServerData.Markers[kvp.Key].MarkerId == jobConfiguration.JobEmployPointMarkerId)
                    {
                        Marker mark = NAPI.Entity.GetEntityFromHandle<Marker>(kvp.Key);
                        mark.MarkerType = dbmarker.MarkerType;
                        mark.Position = dbmarker.Position.GetVector3();
                        mark.Rotation = dbmarker.Rotation.GetVector3();
                        mark.Color = new Color(red, green, blue, alpha);
                        mark.Scale = scale;
                        mark.Dimension = dbmarker.Dimension;

                        ServerData.Markers[kvp.Key] = dbmarker;
                    }
                }
            }
            else
            {
                dbmarker = new Database.Collections.Marker();
                dbmarker.MarkerType = markerType;
                dbmarker.Position = new IMVector3(new Vector3(player.Position.X, player.Position.Y, groundz));
                dbmarker.Rotation = new IMVector3(new Vector3(0,0,0));
                dbmarker.Direction = new IMVector3(new Vector3(directionX, directionY, directionZ));
                dbmarker.Red = red;
                dbmarker.Blue = blue;
                dbmarker.Green = green;
                dbmarker.Alpha = alpha;
                dbmarker.Scale = scale;
                dbmarker.BobUpAndDown = false;
                dbmarker.Dimension = player.Dimension;
                dbmarker.AddNew();

                dbcolShape = new Database.Collections.ColShapeCylinder(Enums.ColShapeType.JobEmployPoint);
                dbcolShape.Position = new IMVector3(new Vector3(player.Position.X, player.Position.Y, groundz));
                dbcolShape.Range = range;
                dbcolShape.Height = rangeheight;
                dbcolShape.Dimension = player.Dimension;
                dbcolShape.AddNew();

                jobConfiguration.JobEmployColShape3DId = dbcolShape.ColShapeCylinderId;
                jobConfiguration.JobEmployPointMarkerId = dbmarker.MarkerId;
                jobConfiguration.Update();

                NAPI.Task.Run(() =>
                {
                    Marker mark = NAPI.Marker.CreateMarker(markerType, dbmarker.Position.GetVector3(), dbmarker.Rotation.GetVector3(), dbmarker.Rotation.GetVector3(), scale, new Color(red, green, blue, alpha), dbmarker.BobUpAndDown, player.Dimension);
                    ServerData.Markers.Add(mark.Handle, dbmarker);

                    ColShape col = NAPI.ColShape.CreateCylinderColShape(dbcolShape.Position.GetVector3(), dbcolShape.Range, dbcolShape.Height, dbcolShape.Dimension);
                    ServerData.ColShapes.Add(col.Handle, dbcolShape);
                });

            }
            Util.ChatMessage.SendNotification(player, "Job Employ Point set");
        }
        [Command("createpickup")]
        public async Task CreateMiningPointAsync(Player player, Enums.JobType job)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            Database.Collections.JobConfiguration jobConfiguration =  Database.Collections.JobConfiguration.GetByID(job);
            if (jobConfiguration == null)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "Job does not exist exists");
                return;
            }

            switch(job)
            {
                case Enums.JobType.MiningJob:

                    float groundz = player.Position.Z;
                    groundz =  Util.GroundZ.GetGroundZ(player);

                    jobConfiguration.PickupPoints.Add(new IMVector3(new Vector3(player.Position.X, player.Position.Y, groundz)));
                    jobConfiguration.Update();
                    break;
            }

            Util.ChatMessage.SendNotification(player, "Pickup Point set");
        }
        [Command("setvehiclespawn")]
        public async Task SetVehicleSpawnPointAsync(Player player, Enums.JobType job)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            if(!player.IsInVehicle)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "You must be in an admin vehicle to set the vehicle spawn position for a job.");
                return;
            }
            Vehicle veh = player.Vehicle;

            Database.Collections.JobConfiguration jobConfiguration =  Database.Collections.JobConfiguration.GetByID(job);
            if (jobConfiguration == null)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "Job does not exist exists");
                return;
            }

            jobConfiguration.JobVehicleSpawnPoint = new IMVector3(veh.Position);
            jobConfiguration.JobVehicleSpawnPointRot = new IMVector3(veh.Rotation);
            jobConfiguration.Update();

            Util.ChatMessage.SendNotification(player, "Vehicle Spawn Point set");
        }
    }
}
