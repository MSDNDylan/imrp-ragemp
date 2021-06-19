using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using IMRP.Database.Collections;
using IMRP.Enums;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class NativeUIMenuEvents : Script
    {
        public NativeUIMenuEvents()
        {
            NativeUIListener.NativeUIMenuSelected += NativeUIListener_NativeUIMenuSelected;
        }

        private void NativeUIListener_NativeUIMenuSelected(Player player, string argument0 = "", string argument1 = "", string argument2 = "", string argument3 = "", string argument4 = "")
        {
            try
            {
                string menuItemType = argument0.Trim();
                string menuID = argument1;
                string input = argument2.Trim();

                switch(menuID)
                {
                    case "EMPLOYJOB_MINING":
                        if(menuItemType == "UIMenuItem")
                        {
                            Database.Collections.JobConfiguration job = Database.Collections.JobConfiguration.GetByID(JobType.MiningJob);
                            switch (input)
                            {
                                case "Join Job":
                                    PlayerData.players[player.Handle].CurrentJob = JobType.MiningJob;
                                    PlayerData.players[player.Handle].Update();
                                    Util.ChatMessage.SendNotification(player, "You have joined job Mining!");
                                     Modules.MiningJob.InitalizeMiningPoints(player);
                                    Menus.EmployJob.Open(player, JobType.MiningJob);
                                    break;
                                case "Quit Job":
                                    Menus.EmployJob.Close(player);
                                    Modules.MiningJob.ReturnMiningTruck(player);
                                    PlayerData.players[player.Handle].CurrentJob = JobType.None;
                                    PlayerData.players[player.Handle].Update();
                                    Util.ChatMessage.SendNotification(player, "You have quit job Mining!");
                                    player.TriggerEvent("cleanupPickups");
                                    break;
                                case "Spawn Job Vehicle":
                                     Modules.MiningJob.SpawnMiningTruck(player);
                                    Menus.EmployJob.Close(player);
                                    break;
                                case "Return Job Vehicle":
                                    Modules.MiningJob.ReturnMiningTruck(player);
                                    Menus.EmployJob.Close(player);
                                    break;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"NativeUIMenuEvents {ex.Message} {ex.StackTrace}");
            }
        }

    }
}
