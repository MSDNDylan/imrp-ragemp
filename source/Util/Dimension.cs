using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GTANetworkAPI;

namespace IMRP.Util
{
    public class Dimension
    {
        public static Dictionary<Player, uint> CharacterSelectorCreatorSessions = new Dictionary<Player, uint>();
        public static void AddSelectorSession(Player player)
        {
            uint nextSession = (from row in CharacterSelectorCreatorSessions
                            orderby row.Value descending
                            select row.Value).FirstOrDefault();
            nextSession = nextSession++;

            if(!CharacterSelectorCreatorSessions.ContainsKey(player))
            {
                CharacterSelectorCreatorSessions.Add(player, nextSession);
                player.Dimension = nextSession;
            }
            else
            {
                player.Dimension = CharacterSelectorCreatorSessions[player];
            }
        }
        public static void DeleteSelectorSession(Player player)
        {
            CharacterSelectorCreatorSessions.Remove(player);
            player.Dimension = 0;
        }
    }
}
