using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;
using IMRP.Models;
using IMRP.Enums;
using System.Threading.Tasks;

namespace IMRP.Modules
{
    public class MiningJob : Script
    {
        public static JobType JobID = JobType.MiningJob;

        public static Dictionary<NetHandle, int> PlayersMining = new Dictionary<NetHandle, int>(); //Nethandle of player, seconds
        public static Dictionary<NetHandle, Vehicle> MiningTrucks = new Dictionary<NetHandle, Vehicle>(); //Player, Vehicle
        public MiningJob()
        {
            Util.Timers.Timer1SecondElapsedEvent += Timers_Timer1SecondElapsedEvent;
        }

        private void Timers_Timer1SecondElapsedEvent()
        {
            foreach (var kvp in PlayersMining.ToArray())
            {
                if(kvp.Value <= 0)
                {
                    Player player = NAPI.Entity.GetEntityFromHandle<Player>(kvp.Key);
                    StopMining(player, true);
                }
                else
                {
                    Console.WriteLine(PlayersMining[kvp.Key]);
                    PlayersMining[kvp.Key] = kvp.Value - 1;
                }
            }
        }

        public static void InitalizeMiningPoints(Player player)
        {
            Database.Collections.JobConfiguration job =  Database.Collections.JobConfiguration.GetByID(JobType.MiningJob);
            if (job == null) return;
            foreach (IMVector3 point in job.PickupPoints)
            {
                player.TriggerEvent("miningJobData", point, point, new Vector3(0, 0, 0), new Color(52, 158, 235, 255), player.Dimension);
            }
        }

        public static void BeginMining(Player player)
        {
            Database.Collections.PlayerInventory pinventory =  Database.Collections.PlayerInventory.GetByID(PlayerData.players[player.Handle].CharacterId);
            float totalWeight = pinventory.Items.Select(x => x.Weight).Sum();
            if((totalWeight+(3*0.05f)) >= 50f)
            {
                Util.ChatMessage.SendNotification(player, "Your inventory is full!");
            }

            SetPlayerMining(player, true);
            NAPI.Player.PlayPlayerScenario(player, "WORLD_HUMAN_CONST_DRILL");
        }

        public static void ReturnMiningTruck(Player player)
        {
            if (Modules.MiningJob.MiningTrucks.ContainsKey(player.Handle))
            {
                NAPI.Task.Run(() =>
                {
                    if (Modules.MiningJob.MiningTrucks.ContainsKey(player.Handle)) Modules.MiningJob.MiningTrucks[player.Handle].Delete();
                });

                Modules.MiningJob.MiningTrucks.Remove(player.Handle);
            }
        }

        public static void StopMining(Player player, bool Completed = false)
        {
            if (PlayersMining.ContainsKey(player.Handle)) PlayersMining.Remove(player.Handle);
            NAPI.Player.StopPlayerAnimation(player);
            SetPlayerMining(player, false);

            if(Completed)
            {
                Random rand = new Random();
                int ores = rand.Next(3,15);
                Util.ChatMessage.SendNotification(player, $"You have mined {ores}  ores!");
                Modules.Inventory.Character.AddInventoryItem(player, new InventoryItem("Ores", ores, 0.05f, "resource-item"));
            }
        }

        public static void SpawnMiningTruck(Player player)
        {
            if (PlayerData.players[player.Handle].CurrentJob != JobType.MiningJob) return;
            if(!MiningTrucks.ContainsKey(player.Handle))
            {
                Database.Collections.JobConfiguration jobconfig =  Database.Collections.JobConfiguration.GetByID(JobType.MiningJob);

                NAPI.Task.Run(() =>
                {
                    Vehicle veh = NAPI.Vehicle.CreateVehicle(VehicleHash.Tiptruck, jobconfig.JobVehicleSpawnPoint.GetVector3(), jobconfig.JobVehicleSpawnPointRot.GetVector3(), 0, 0, "MINING CO", 255, true, false, player.Dimension);
                    veh.EngineStatus = false;
                    veh.Locked = true;
                    MiningTrucks.Add(player.Handle, veh);
                    Util.ChatMessage.SendNotification(player, "A Mining Truck is ready for you to use!");
                });
            }
            else
            {
                Util.ChatMessage.SendErrorChatMessage(player, "You already have a Mining Truck!");
            }
        }

        public static bool IsPlayerMining(Player player)
        {
            if (player.HasData("IsMining"))
            {
                return player.GetData<bool>("IsMining");
            }
            else
            {
                return false;
            }
        }

        private static void SetPlayerMining(Player player, bool isPlayerMining)
        {
            if (isPlayerMining && !PlayersMining.ContainsKey(player.Handle))
            {
                Random rnd = new Random();
                int seconds = rnd.Next(4, 10);
                PlayersMining.Add(player.Handle, seconds);
            }
            else if(!isPlayerMining && PlayersMining.ContainsKey(player.Handle))
            {
                PlayersMining.Remove(player.Handle);
            }

            player.SetData("IsMining", isPlayerMining);
        }
    }
}
