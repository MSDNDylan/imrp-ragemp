using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using IMRP.Database.Collections;
namespace IMRP
{
    public class ServerData
    {
        public static Dictionary<GTANetworkAPI.Vehicle, Database.Collections.Vehicle> SpawnedVehicles = new Dictionary<GTANetworkAPI.Vehicle, Database.Collections.Vehicle>();

        public static Dictionary<Database.Collections.GroundItem, NetHandle> GroundItems = new Dictionary<GroundItem, NetHandle>();

        public static Dictionary<NetHandle, Database.Collections.Marker> Markers = new Dictionary<NetHandle, Database.Collections.Marker>();

        public static Dictionary<NetHandle, Database.Collections.ColShapeCylinder> ColShapes = new Dictionary<NetHandle, Database.Collections.ColShapeCylinder>();
    }
}
