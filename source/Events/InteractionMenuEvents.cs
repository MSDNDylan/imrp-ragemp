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
    public class InteractionMenuEvents : Script
    {
        public InteractionMenuEvents()
        {
            NativeUIListener.NativeUIMenuSelected += NativeUIListener_NativeUIMenuSelected;
        }
        private static void NativeUIListener_NativeUIMenuSelected(Player player, string argument0 = "", string argument1 = "", string argument2 = "", string argument3 = "", string argument4 = "")
        {
            try
            {
                if (player.IsInVehicle)
                {
                    if (argument0.Trim() == "UIMenuItem" && argument1 == "INTERACTION_VEH")
                    {
                        argument2 = argument2.Trim();
                        switch (argument2)
                        {
                            case "Start Engine":
                                NativeUI.MenuBuilder.CloseMenu(player);
                                player.Vehicle.EngineStatus = true;
                                Util.ChatMessage.SendRoleplayMessage(player, "has started their engine.");
                                break;
                            case "Stop Engine":
                                NativeUI.MenuBuilder.CloseMenu(player);
                                player.Vehicle.EngineStatus = false;
                                Util.ChatMessage.SendRoleplayMessage(player, "has stopped their engine.");
                                break;
                            case "Buckle Seatbelt":
                                NativeUI.MenuBuilder.CloseMenu(player);
                                NativeUI.MenuBuilder.CloseMenu(player);
                                Util.ChatMessage.SendRoleplayMessage(player,$"has buckled their seatbelt.");
                                PlayerData.players[player].Seatbelt = true;
                                player.TriggerEvent("seatbeltBuckled", true);
                                break;
                            case "Unbuckle Seatbelt":
                                NativeUI.MenuBuilder.CloseMenu(player);
                                Util.ChatMessage.SendRoleplayMessage(player, "has unbuckled their seatbelt.");
                                PlayerData.players[player].Seatbelt = false;
                                player.TriggerEvent("seatbeltBuckled", false);
                                break;
                        }
                    }
                }
                else if (argument0.Trim() == "UIMenuItem" && argument1 == "INTERACTION")
                {
                    switch (argument2)
                    {
                        case "Lock Vehicle":
                            GTANetworkAPI.Vehicle nearestVehicle =  Util.Distance.GetNearestVehicle(player, 5);
                            nearestVehicle.Locked = true;
                            NativeUI.MenuBuilder.CloseMenu(player);
                            Util.ChatMessage.SendRoleplayMessage(player, "has locked a vehicle");
                            break;
                        case "Unlock Vehicle":
                            GTANetworkAPI.Vehicle nearestVehicle2 =  Util.Distance.GetNearestVehicle(player, 5);
                            nearestVehicle2.Locked = false;
                            NativeUI.MenuBuilder.CloseMenu(player);
                            Util.ChatMessage.SendRoleplayMessage(player, "has unlocked a vehicle");
                            break;
                    }
                }
                Console.WriteLine($"EVENT CAUGHT {argument0}, {argument1}, {argument2}, {argument3}, {argument4}");
            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"InteractionMenuEvents {ex.Message} {ex.StackTrace}");
            }
        }

        [RemoteEvent("openInteractionMenu")]
        public void OpenInteractionMenu(Player player, params object[] arguments)
        {
             Menus.InteractionMenu.Open(player);
        }
    }
}
