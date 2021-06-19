using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Modules
{
    public class Thirst
    {
        public Thirst()
        {
            Util.Timers.Timer1SecondElapsedEvent += Timers_Timer1SecondElapsedEvent;
        }

        private void Timers_Timer1SecondElapsedEvent()
        {
            List<Player> onlinePlayers = NAPI.Pools.GetAllPlayers();
            for (int i = 0; i < onlinePlayers.Count; i++)
            {
                Player player = onlinePlayers[i];
                if (PlayerData.players.ContainsKey(player.Handle))
                {
                    Database.Collections.Character character = PlayerData.players[player.Handle];
                    TimeSpan secondDiff = (DateTime.UtcNow - character.LastThirstDecrease) / 1000;
                    if (secondDiff.TotalSeconds >= 54)
                    {
                        PlayerData.players[player.Handle].LastThirstDecrease = DateTime.UtcNow;
                        PlayerData.players[player.Handle].Thirst = PlayerData.players[player.Handle].Thirst--;
                        player.TriggerEvent("updateHudThirst", PlayerData.players[player.Handle].Thirst);
                    }
                }
            }
        }
    }
}
