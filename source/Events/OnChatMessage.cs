using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class OnChatMessage : Script
    {
        [ServerEvent(Event.ChatMessage)]
        public void OnChatMessageEvent(Player player, string message)
        {
            player.SendChatMessage("Voice Only Roleplay.");
            player.SendChatMessage("Have a question? Please use /newb <message>");
            player.SendChatMessage("Need to report a player? Use /report");
            return;
        }

        [RemoteEvent("SendChatMessage")]
        public void OnSendChatMessage(Player player, params object[] arguments)
        {
            player.SendChatMessage(arguments[0].ToString());
        }
    }
}
