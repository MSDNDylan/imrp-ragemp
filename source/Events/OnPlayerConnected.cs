using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnplayerConnected : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        public async Task OnplayerConnectedEvent(Player player)
        {
            player.SetData("Authenticated", false);
            Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{player.Address} has connected.");
        }
    }
}
