using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnplayerSpawn : Script
    {
        [ServerEvent(Event.PlayerSpawn)]
        public async Task OnPlayerSpawnEvent(Player player)
        {
            try
            {
                if (!player.GetData<bool>("Authenticated"))
                {
                    Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"Authenticating player {player.Address}");
                    player.Position = new Vector3(-915.811, -379.432, 113.6748);
                    player.TriggerEvent("onPlayerConnectedEx");
                    Util.Dimension.AddSelectorSession(player);
                }
            }catch(Exception ex)
            {
                Console.Write(ex.Message + ex.StackTrace);
            }
        }
    }
}
