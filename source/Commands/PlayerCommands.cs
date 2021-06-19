using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMRP.Database.Collections;
using GTANetworkAPI;

namespace IMRP.Commands
{
    public class playerCommands : Script
    {
        [Command("alias", GreedyArg = true)]
        public async Task AliasAsync(Player player, int playerId, string alias)
        {
            Character character = Character.GetByID(playerId);
            if(character == null)
            {
                Util.ChatMessage.SendErrorChatMessage(player, "player not found!");
                return;
            }
            else
            {
                Character currentplayerChar = PlayerData.GetCharacter(player);
                if(currentplayerChar.Aliases.ContainsKey(character.CharacterId))
                {
                    Util.ChatMessage.SendErrorChatMessage(player, "You've already aliased this character!");
                    return;
                }
                else
                {
                    currentplayerChar.Aliases.Add(character.CharacterId, alias);
                     currentplayerChar.Update();

                    Util.ChatMessage.SendNotification(player, $"You have aliased Stranger_{character.StrangerId} as {alias}");
                }
            }
        }
        [Command("me", GreedyArg = true)]
        public void MeCommand(Player player, string message)
        {
            Util.ChatMessage.SendRoleplayMessage(player, message);

        }
        [Command("do", GreedyArg = true)]
        public void DoCommand(Player player, string message)
        {
             Util.ChatMessage.SendDORoleplayMessageAsync(player, message);
        }
        [Command("b", GreedyArg = true)]
        public void SendOOCCommand(Player player, string message)
        {
            Util.ChatMessage.SendOOCRoleplayMessage(player, message);
        }
        [Command("newb", GreedyArg = true)]
        public void SendNewbMessage(Player player, string message)
        {
            Util.ChatMessage.SendNewbMessage(player, message);
        }
    }
}
