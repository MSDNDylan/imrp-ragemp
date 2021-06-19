using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnplayerDamage : Script
    {
        [ServerEvent(Event.PlayerDamage)]
        public void playerDamage(Player player, float healthLoss, float armorLoss)
        {
            Console.WriteLine($"test {player.Health} {player.Armor}");
           /* Database.Collections.Character character = ClientData.GetCharacter(player);
            character.Health = player.Health;
            character.Armor = player.Armor;
            player.TriggerEvent("updateHudHealth", player.Health);
            player.TriggerEvent("updateHudArmor", player.Armor);
            Console.WriteLine($"{player.Health} {player.Armor}");*/
        }
    }
}
