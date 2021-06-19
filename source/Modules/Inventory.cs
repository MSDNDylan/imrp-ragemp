using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IMRP.Models;

namespace IMRP.Modules
{
    public class Inventory
    {
        public class Character : Script
        {
            public static void UseInventoryItem(Player player, InventoryItem item)
            {
                RemoveInventoryItem(player, item);
            }
            public static List<InventoryItem> GetInventoryItems(Player player)
            {
                Database.Collections.PlayerInventory pinventory = Database.Collections.PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
                return pinventory.Items;
            }
            public static async void DropInventoryItem(Player player, InventoryItem item)
            {
                if (!HasInventoryItem(player, item))
                {
                    Util.ChatMessage.SendNotification(player, $"Insufficent inventory for ${item.Name}");
                    return;
                }

                Database.Collections.GroundItem groundItem = new Database.Collections.GroundItem();
                GroundItem newGroundItem = new GroundItem(item.Name, item.Qty, item.Weight, item.Image, item.Quality);
                groundItem.Items.Add(newGroundItem);
                 groundItem.AddNew();

                GTANetworkAPI.Object obj = NAPI.Object.CreateObject(4014693584, player.Position, player.Rotation, (byte)player.Dimension);
               
                ServerData.GroundItems.Add(groundItem, obj.Handle);
            }
            public static void AddInventoryItem(Player player, InventoryItem item)
            {
                bool alreadyUpdated = false;
                Database.Collections.PlayerInventory pinventory =  Database.Collections.PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
                if(pinventory == null)
                {
                    pinventory = new Database.Collections.PlayerInventory();
                    pinventory.CharacterId = PlayerData.players[player].CharacterId;
                     pinventory.AddNew();
                    pinventory =  Database.Collections.PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
                }
                for(int i = 0; i < pinventory.Items.Count; i++)
                {
                    if(pinventory.Items[i].Name == item.Name && pinventory.Items[i].Quality == item.Quality)
                    {
                        if (item.InventoryID == -1) item.InventoryID =  Database.Collections.PlayerInventory.GetNextIDAsync(PlayerData.players[player].CharacterId);
                        pinventory.Items[i].Qty += item.Qty;
                         pinventory.Update();
                        alreadyUpdated = true;
                    }
                }

                if(!alreadyUpdated)
                {
                    pinventory.Items.Add(item);
                     pinventory.Update();
                }
            }
            public static void RemoveInventoryItem(Player player, InventoryItem item)
            {
                Database.Collections.PlayerInventory pinventory =  Database.Collections.PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
                for(int i = 0; i < pinventory.Items.Count; i++)
                {
                    if(pinventory.Items[i].Name == item.Name && pinventory.Items[i].Qty == item.Qty)
                    {
                        pinventory.Items.RemoveAt(i);
                         pinventory.Update();
                    }
                    else if(pinventory.Items[i].Name == item.Name && pinventory.Items[i].Qty > item.Qty)
                    {
                        pinventory.Items[i].Qty -= item.Qty;
                         pinventory.Update();
                    }
                    else
                    {
                        Util.Logging.Log(Util.Logging.LogType.ServerError, $"Potential dupe! Character ID: {PlayerData.players[player].CharacterId} Item quantities didn't match when attempting to remove item from inventory.");
                    }
                }
            }

            public static bool HasInventoryItem(Player player, InventoryItem item)
            {
                bool inventorySufficent = false;
                Database.Collections.PlayerInventory playerInventory = Database.Collections.PlayerInventory.GetByID(PlayerData.players[player].CharacterId);

                foreach(InventoryItem it in playerInventory.Items)
                {
                    if(it.Name == item.Name && it.Qty >= item.Quality)
                    {
                        inventorySufficent = true;
                    }
                }

                return inventorySufficent;
            }
        }
    }
}
