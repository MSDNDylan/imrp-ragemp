using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IMRP.Modules;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class PlayerInventoryEvents : Script
    {
        [RemoteEvent("refreshPlayerInventory")]
        public async Task OnRequestPlayerInventoryAsync(Player player)
        {
            List<Models.InventoryItem> inventoryItems =  Inventory.Character.GetInventoryItems(player);
            string json = JsonConvert.SerializeObject(inventoryItems);
            player.TriggerEvent("refreshPlayerInventory", json);
        }

        [RemoteEvent("DropPlayerInventoryItem")]
        public async Task OnPlayerDropInventoryItemAsync(Player player, params object[] arguments)
        {
            Models.InventoryItem item = (Models.InventoryItem)arguments[0];
            Inventory.Character.DropInventoryItem(player, item);
            Util.ChatMessage.SendNotification(player, $"You have dropped {item.Qty} {item.Name}");
            Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{PlayerData.players[player].CharacterId} has dropped {item.Qty} {item.Name}");
        }
    }
}
