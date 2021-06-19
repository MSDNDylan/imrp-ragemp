using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnplayerVehicle : Script
    {
        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnplayerEnterVehicle(Player player, Vehicle vehicle, sbyte seatID)
        {
            vehicle.EngineStatus = false;

            player.SetData("SeatID", seatID);

            if(Modules.MiningJob.MiningTrucks.ContainsKey(player.Handle) && Modules.MiningJob.MiningTrucks[player.Handle] == vehicle.Handle)
            {
                player.TriggerEvent("setWayPoint", -596.7063, 2091.1438);
                player.SendChatMessage("~y~Welcome Miner, your GPS coordinates have been pre-programmed to guide you to the mine shaft.");
                //-596.7063  2091.1438
            }
            Console.WriteLine($"{player.Name} has entered vehicle");
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void OnplayerExitVehicle(Player player, Vehicle vehicle)
        {
            if (PlayerData.players[player].Seatbelt)
            {
                player.SetIntoVehicle(vehicle.Handle, player.GetData<sbyte>("SeatID"));
                player.SendChatMessage("~r~Your seatbelt is on. You can not exit the vehicle until your seatbelt is off.");
            }
            else
            {
                PlayerData.players[player].Seatbelt = false;
            }
        }
        
    }
}
