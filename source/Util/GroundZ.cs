using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Util
{
    public class GroundZ : Script
    {
        public static float GetGroundZ(Player player)
        {
            if(Events.GetGroundZ.PendingGroundZ.ContainsKey(player.Handle)) Events.GetGroundZ.PendingGroundZ.Remove(player.Handle);
            player.TriggerEvent("getGroundZForMethod");

            int retry = 0;
            float groundZ = -1.0f;

            while (true)
            {
                if (retry == 10) break;
                if (IMRP.Events.GetGroundZ.PendingGroundZ.ContainsKey(player.Handle))
                {
                    groundZ = IMRP.Events.GetGroundZ.PendingGroundZ[player.Handle];
                    break;
                }

                retry = retry++;
                Task.Delay(1000);
            }

            return groundZ;
        }
    }
}
