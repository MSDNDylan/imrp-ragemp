using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using IMRP.NativeUI;

namespace IMRP.Menus
{
    public class EmployJob
    {
        public static void Open (Player player, Enums.JobType job)
        {
            if (player.IsInVehicle) return;

            switch (job)
            {
                case Enums.JobType.MiningJob:
                    MenuBuilder builder;
                    MenuItem newMenuItem;

                    builder = new MenuBuilder("EMPLOYJOB_MINING", "Mining Job", "");

                    if(PlayerData.players[player.Handle].CurrentJob == Enums.JobType.MiningJob)
                    {
                        if(!Modules.MiningJob.MiningTrucks.ContainsKey(player.Handle))
                        {
                            builder.MenuItems.Add(builder.NewMenuItem().UIMenuItem("Spawn Job Vehicle", "Spawn Job Vehicle"));
                        }
                        else
                        {
                            builder.MenuItems.Add(builder.NewMenuItem().UIMenuItem("Return Job Vehicle", "Return Job Vehicle"));
                        }

                        builder.MenuItems.Add(builder.NewMenuItem().UIMenuItem("Quit Job", "Quit Job").Properties.ForeColor(255, 0, 0, 255));
                    }
                    else
                    {
                        newMenuItem = new MenuItem();
                        newMenuItem.UIMenuItem("Join Job", "Join Job");
                        //newMenuItem.Properties.ForeColor = new NativeUI.Color(72, 255, 0, 255); //Green
                        builder.MenuItems.Add(newMenuItem);
                    }
                    builder.SendMenuToplayer(player);
                    break;
            }
        }
        public static void Close (Player player)
        {
            player.TriggerEvent("destroyMenu");
        }
    }
}
