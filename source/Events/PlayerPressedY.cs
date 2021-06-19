using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class PlayerPressedY : Script
    {
        [RemoteEvent("PlayerPressedY")]
        public void OnPlayerPressedY(Player player)
        {
            if (!PlayerData.players.ContainsKey(player.Handle)) return;
            if(PlayerData.players[player.Handle].CurrentJob == Enums.JobType.MiningJob)
            {
                Database.Collections.JobConfiguration miningJob = Database.Collections.JobConfiguration.GetByID(Enums.JobType.MiningJob);
                foreach(IMVector3 point in miningJob.PickupPoints)
                {
                    if (Vector3.Distance(player.Position, point.GetVector3()) <= 2f)
                    {
                        Console.WriteLine("Player Pressed Y");
                        if (Modules.MiningJob.IsPlayerMining(player))
                        {
                            Modules.MiningJob.StopMining(player);
                            Util.ChatMessage.SendNotification(player, "You have canceled mining operation.");
                        }
                        else
                        {
                            Util.ChatMessage.SendNotification(player, "You have started mining!");
                            Modules.MiningJob.BeginMining(player);
                        }
                        break;
                    }
                }
            }
        }
    }
}
