using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Threading.Tasks;

namespace IMRP.Events
{
    public class OnplayerLeave : Script
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public async Task OnplayerLeaveEventAsync(Player player, DisconnectionType type, string reason)
        {
            PlayerData.players[player].LastPosition = new IMVector3(player.Position);
            PlayerData.players[player].LastRotation = new IMVector3(player.Rotation);
            PlayerData.players[player].Update();

            string log = $"[{PlayerData.players[player].CharacterId}]{PlayerData.players[player].FirstName} {PlayerData.players[player].LastName} has disconnected from the server. Reason: {reason}";
            Util.Logging.Log(Util.Logging.LogType.ServerInfo, log);

            PlayerData.players.Remove(player);
        }
    }
}
