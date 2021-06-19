using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMRP.Database.Collections;
using GTANetworkAPI;

namespace IMRP.Commands
{
    public class Property : Script
    {
        [Command("createproperty", GreedyArg = true)]
        public async Task CreateProperty(Player player, string message)
        {
            //NAPI.Marker.CreateMarker(int markerType, Vector3 pos, Vector3 dir, Vector3 rot, float scale, int red, int green, int blue, bool bobUpAndDown = false, uint dim);
        }
    }
}
