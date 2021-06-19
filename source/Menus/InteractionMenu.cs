using System;
using System.Collections.Generic;
using System.Text;
using IMRP.NativeUI;
using GTANetworkAPI;

namespace IMRP.Menus
{
    public class InteractionMenu
    {
        public static void Open(Player player)
        {
            try
            {
                if (player.IsInVehicle)
                {
                    MenuBuilder builder = new MenuBuilder("INTERACTION_VEH", "Interaction Menu", "");
                    MenuItem newMenuItem = new MenuItem();

                    if (!PlayerData.players[player].Seatbelt)
                    {
                        newMenuItem.UIMenuItem("Buckle Seatbelt", "Buckle Seatbelt");
                        newMenuItem.Properties.ForeColor = new NativeUI.Color(72, 255, 0, 255);
                        builder.MenuItems.Add(newMenuItem);
                    }

                    if (!player.Vehicle.EngineStatus)
                    {
                        newMenuItem = new MenuItem();
                        newMenuItem.UIMenuItem("Start Engine", "Start Engine");
                        newMenuItem.Properties.ForeColor = new NativeUI.Color(72, 255, 0, 255); //Green
                        builder.MenuItems.Add(newMenuItem);
                    }
                    if (player.Vehicle.EngineStatus)
                    {
                        newMenuItem = new MenuItem();
                        newMenuItem.UIMenuItem("Stop Engine", "Stop Engine");
                        newMenuItem.Properties.ForeColor = new NativeUI.Color(255, 0, 0, 255); //Red
                        builder.MenuItems.Add(newMenuItem);
                    }

                    if (PlayerData.players[player].Seatbelt)
                    {
                        newMenuItem = new MenuItem();
                        newMenuItem.UIMenuItem("Unbuckle Seatbelt", "Unbuckle Seatbelt");
                        newMenuItem.Properties.ForeColor = new NativeUI.Color(255, 0, 0, 255);
                        builder.MenuItems.Add(newMenuItem);
                    }

                    newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("More Options...", "More Options...");
                    //   newMenuItem.Properties.ForeColor = new NativeUI.Color(181, 181, 181, 255);
                    builder.MenuItems.Add(newMenuItem);
                    Console.WriteLine("Sent UI to player");
                    builder.SendMenuToplayer(player);
                }
                else
                {
                    MenuBuilder builder = new MenuBuilder("INTERACTION", "Interaction Menu", "");

                    GTANetworkAPI.Vehicle nearestVehicle =  Util.Distance.GetNearestVehicle(player, 2.7f);
                    if (nearestVehicle != null)
                    {
                        if (!nearestVehicle.Locked)
                        {
                            MenuItem newMenuItem1 = new MenuItem();
                            newMenuItem1 = new MenuItem();
                            newMenuItem1.UIMenuItem("Lock Vehicle", "Lock Vehicle");
                            newMenuItem1.Properties.ForeColor = new NativeUI.Color(72, 255, 0, 255);
                            builder.MenuItems.Add(newMenuItem1);
                        }
                        else
                        {
                            MenuItem newMenuItem1 = new MenuItem();
                            newMenuItem1 = new MenuItem();
                            newMenuItem1.UIMenuItem("Unlock Vehicle", "Unlock Vehicle");
                            newMenuItem1.Properties.ForeColor = new NativeUI.Color(255, 0, 0, 255);
                            builder.MenuItems.Add(newMenuItem1);
                        }
                    }

                    MenuItem newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("Phone", "Phone");
                    builder.MenuItems.Add(newMenuItem);

                    newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("player", "player");
                    builder.MenuItems.Add(newMenuItem);

                    newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("Group", "Group");
                    builder.MenuItems.Add(newMenuItem);

                    newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("Properties", "Properties");
                    builder.MenuItems.Add(newMenuItem);

                    newMenuItem = new MenuItem();
                    newMenuItem.UIMenuItem("Vehicles", "Vehicles");
                    builder.MenuItems.Add(newMenuItem);

                    builder.SendMenuToplayer(player);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
