using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Modules
{
    public class Hunger
    {
        public Hunger()
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
                    TimeSpan secondDiff = (DateTime.UtcNow - character.LastHungerDecrease) / 1000;
                    if(secondDiff.TotalSeconds >= 72)
                    {
                        PlayerData.players[player.Handle].LastHungerDecrease = DateTime.UtcNow;
                        PlayerData.players[player.Handle].Hunger = PlayerData.players[player.Handle].Hunger--;
                        player.TriggerEvent("updateHudHunger", PlayerData.players[player.Handle].Hunger);
                    }
                }
            }
        }
    }
}
