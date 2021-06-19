using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class GetGroundZ : Script
    {
        public static Dictionary<NetHandle, float> PendingGroundZ = new Dictionary<NetHandle, float>();

        [RemoteEvent("SendGroundZ")]
        public void OnGetGroundZ (Player player, params object[] arguments)
        {
            PendingGroundZ.Add(player, (float)arguments[0]);
        }
    }
}
