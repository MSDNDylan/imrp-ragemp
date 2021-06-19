using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using IMRP.Database.Collections;
using System.Threading.Tasks;

namespace IMRP.Util
{
    public static class ChatMessage
    {
        public static int Range = 50;
        private static string roleplayColor = "!{#ccb3ff}";
        private static string newbColor = "!{#01BEFF}";
        private static string darkGreyColor = "!{#B8B8B8}";
        public static void SendRoleplayMessage(Player player, string message)
        {
            if (PlayerData.players[player].StaffMode) return;

            Character character = PlayerData.GetCharacter(player);
            List<Player> players = NAPI.Pools.GetAllPlayers();

            foreach (Player c in players)
            {
                if (c == player && !PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{roleplayColor}[{character.CharacterId}] {character.FirstName} {character.LastName} {message}");
                }
                else if (PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{roleplayColor}[{character.CharacterId}] {character.FirstName} {character.LastName} {message}");
                }
                else if (playerInRange(player, c))
                {
                    Character ch = PlayerData.GetCharacter(c);
                    if (ch.Aliases.ContainsKey(character.CharacterId))
                    {
                        c.SendChatMessage($"{roleplayColor}[{character.CharacterId}] {ch.Aliases[character.CharacterId]} {message}");
                    }
                    else
                    {
                        c.SendChatMessage($"{roleplayColor}{player.Name} {message}");
                    }
                }
            }
        }
        public static async Task SendDORoleplayMessageAsync(Player player, string message)
        {
            Account account =  PlayerData.GetAccount(player);
            if (PlayerData.players[player].StaffMode) return;

            Character character = PlayerData.players[player];
            List<Player> players = NAPI.Pools.GetAllPlayers();

            foreach (Player c in players)
            {
                if (c == player && !PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{roleplayColor}{message} (([{character.CharacterId}] {character.FirstName} {character.LastName}))");
                }
                else if (PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{roleplayColor}{message} (([{character.CharacterId}] {character.FirstName} {character.LastName}))");
                }
                else if (playerInRange(player, c))
                {
                    Character ch = PlayerData.GetCharacter(c);
                    if (ch.Aliases.ContainsKey(character.CharacterId))
                    {
                        c.SendChatMessage($"{roleplayColor}{message} (([{character.CharacterId}] {ch.Aliases[character.CharacterId]}))");
                    }
                    else
                    {
                        c.SendChatMessage($"{roleplayColor}{message} (([{character.CharacterId}] {player.Name}))");
                    }
                }
            }
        }

        internal static void SendNotification(string v)
        {
            throw new NotImplementedException();
        }

        public static void SendNewbMessage(Player player, string message)
        {
            // Account account =  ClientData.GetAccount(player);
            Character character = PlayerData.GetCharacter(player);
            List<Player> players = NAPI.Pools.GetAllPlayers();

            bool coolDownSet = true;
            Console.WriteLine($"{character.NewbChatCoolDown}      {DateTime.UtcNow}");
            if (DateTime.UtcNow > character.NewbChatCoolDown && character.NewbChatCoolDown.Day == DateTime.UtcNow.Day)
            {
                foreach (Player c in players)
                {
                    if (PlayerData.players[player].PermissionLevel >= Modules.Staff.PermissionLevel.Moderator)
                    {
                        c.SendChatMessage($"{darkGreyColor}(({newbColor}[NEWBIE] ~g~Staff {PlayerData.players[player].StaffName}{newbColor}: {message}{darkGreyColor}))");
                    }
                    else
                    {
                        c.SendChatMessage($"{darkGreyColor}(({newbColor}[NEWBIE] [{character.CharacterId}] {character.FirstName} {character.LastName}: {message}{darkGreyColor}))");

                        if (coolDownSet)
                        {
                            PlayerData.players[player].NewbChatCoolDown = DateTime.UtcNow.AddSeconds(15);
                            coolDownSet = false;
                        }
                    }
                }
            }
            else
            {
                player.SendChatMessage($"{darkGreyColor}Newbie Chat Cooldown in {Math.Round((PlayerData.players[player].NewbChatCoolDown - DateTime.UtcNow).TotalSeconds)} seconds");
            }
        }
        public static void SendOOCRoleplayMessage(Player player, string message)
        {
            // Account account =  ClientData.GetAccount(player);

            Character character = PlayerData.GetCharacter(player);
            List<Player> players = NAPI.Pools.GetAllPlayers();

            foreach (Player c in players)
            {
                if (c == player && !PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{darkGreyColor}(([{character.CharacterId}] {character.FirstName} {character.LastName}: {message}))");
                }
                else if (PlayerData.players[player].StaffMode)
                {
                    c.SendChatMessage($"{darkGreyColor}(([{character.CharacterId}] {character.FirstName} {character.LastName}: {message}))");
                }
                else if (playerInRange(player, c))
                {
                    Character ch = PlayerData.GetCharacter(c);
                    if (ch.Aliases.ContainsKey(character.CharacterId))
                    {
                        c.SendChatMessage($"{darkGreyColor}(([{character.CharacterId}] {ch.Aliases[character.CharacterId]}: {message} ))");
                    }
                    else
                    {
                        c.SendChatMessage($"{darkGreyColor}(([{character.CharacterId}] {player.Name}: {message}))");
                    }
                }
            }
        }
        public static void SendErrorChatMessage(Player player, string message)
        {
            player.SendChatMessage($"~r~{message}");
        }
        public static void SendNotification(Player player, string message)
        {
            player.SendChatMessage($"{message}");
            player.SendNotification(message);
        }
        public static bool playerInRange(Player source, Player target)
        {
            bool isplayerInRange = false;

            float distanceX = Math.Abs(source.Position.X - target.Position.X);
            float distanceY = Math.Abs(source.Position.Y - target.Position.Y);
            float distanceZ = Math.Abs(source.Position.Z - target.Position.Z);

            if (distanceX <= Range || distanceY <= Range || distanceZ <= Range) isplayerInRange = true;

          return isplayerInRange;
        }
    }
}
