using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IMRP.Database.Collections;
using IMRP.Enums;
using GTANetworkAPI;
using IMRP.Models;

namespace IMRP.Events
{
    public class CharacterSelectorEvents : Script
    {
        [RemoteEvent("getCharacters")]
        public void GetCharacters(Player player, params object[] arguments)
        {
            try
            {
                PlayerData.IsPlayerAuthenticated(player);
                Account account = Account.GetByID(player.GetData<int>("AccountId"));
                List<Character> characters = Character.GetCharactersByAccount(account.AccountId);
                List<SelectorData> selector = new List<SelectorData>();

                if (characters.Count > 0)
                {
                    foreach (Character c in characters)
                    {
                        SelectorData newSelector = new SelectorData();
                        newSelector.Index = characters.IndexOf(c);
                        newSelector.FirstName = c.FirstName;
                        newSelector.LastName = c.LastName;
                        newSelector.CustomizeData = c.CustomizationData;
                        selector.Add(newSelector);
                    }
                }
                string json = JsonConvert.SerializeObject(selector);
                Util.Logging.Log(Util.Logging.LogType.CharacterInfo, $"{account.Username} requested character selector data.");

                if (characters.Count == 0)
                {
                    player.TriggerEvent("initalizeSelectorData", "empty");
                }
                else
                {
                    player.TriggerEvent("initalizeSelectorData", json);
                }
            }catch(Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"CharacterSelectorEvents.GetCharacter ERROR {ex.Message} {ex.StackTrace}");
            }

        }
        [RemoteEvent("deleteCharacter")]
        public async Task DeleteCharacter(Player player, params object[] arguments)
        {
            PlayerData.IsPlayerAuthenticated(player);
            int index = (int)arguments[0];

            Account account = Account.GetByID(player.GetData<int>("AccountId"));
            List<Character> characters = Character.GetCharactersByAccount(account.AccountId);
            Util.Logging.Log(Util.Logging.LogType.CharacterInfo, $"{account.Username} has deleted their character {characters[index].CharacterId} {characters[index].FirstName}  {characters[index].LastName}");
            characters[index].CharacterDeleted = true;
            characters[index].Update();
            GetCharacters(player);
        }
        [RemoteEvent("writeToConsole")]
        public async Task WriteToConsole(Player player, params object[] arguments)
        {
            string message = (string)arguments[0];

            Util.Logging.Log(Util.Logging.LogType.ServerInfo, message);
   
        }
    }
}
