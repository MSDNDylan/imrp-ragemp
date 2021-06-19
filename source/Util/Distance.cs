using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Util
{
    public class Distance
    {
        public static Vehicle GetNearestVehicle(Player player, float range)
        {
            try
            {
                float cX = player.Position.X;
                float cY = player.Position.Y;
                float cZ = player.Position.Z;

                List<VehicleDistance> nearestVehicles = new List<VehicleDistance>();

                foreach (Vehicle veh in NAPI.Pools.GetAllVehicles())
                {
                    float distance = Vector3.Distance(veh.Position, player.Position);
                    Console.WriteLine(distance);
                    if (distance <= range)
                    {
                        nearestVehicles.Add(new VehicleDistance(veh, distance));
                    }
                }
                Vehicle sortedVeh = (from row in nearestVehicles orderby row.Distance ascending select row.Vehicle).FirstOrDefault();

                return sortedVeh;
            }catch(Exception ex)
            {
                Util.Logging.Log(Logging.LogType.ServerError, $"IMRP.UTIL.GETNEARESTDISTANCEVEHICLE ERROR {ex.Message} {ex.StackTrace}");
                return null;
            }
        }

        public class VehicleDistance
        {
            public Vehicle Vehicle { get; set; }
            public float Distance { get; set; }
            public VehicleDistance(Vehicle vehicle, float distance)
            {
                Vehicle = vehicle;
                Distance = distance;
            }
        }
    }
}
