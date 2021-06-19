using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMRP.Database.Collections;
using GTANetworkAPI;

namespace IMRP
{
    public class PlayerData
    {
        public static Dictionary<NetHandle, Character> players = new Dictionary<NetHandle, Character>();
        public static Character GetCharacter (Player client)
        {
            if (players[client] == null) return null;
            return players[client];
        }
        public bool IsPlayerOnline(int playerId)
        {
            List<Player> clients = NAPI.Pools.GetAllPlayers();
            foreach(Player c in clients)
            {
                Character character = GetCharacter(c);
                if (character.AccountId == playerId)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsCharacterSpawned(Player client)
        {
            if (client.HasData("Character")) return true;
            return false;
        }
        public static Account GetAccount (Player client)
        {
            if (players[client].AccountId == -1) return null;
            int accountID = players[client].AccountId;
            return Account.GetByID(accountID);
        }

        public static bool IsPlayerAuthenticated(Player client)
        {
            if(!client.GetData<bool>("Authenticated"))
            {
                Util.ChatMessage.SendErrorChatMessage(client, "Not authenticated.");
                client.Kick();
            }
            return client.GetData<bool>("Authenticated");
        }
    }
}
