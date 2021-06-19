using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using IMRP.Database.Collections;
using Newtonsoft.Json;

namespace IMRP.Events
{
    public class OnCharacterSpawn : Script
    {
        [RemoteEvent("spawnCharacter")]
        public async Task SpawnAsCharacter(Player player, params object[] arguments)
        {
            try
            {
                int index = (int)arguments[0];
                Account account = Account.GetByID(player.GetData<int>("AccountId"));
                List<Character> characters = Character.GetCharactersByAccount(account.AccountId);

                Util.Logging.Log(Util.Logging.LogType.CharacterInfo, $"{account.Username} has selected to play as their character {characters[index].CharacterId} {characters[index].FirstName}  {characters[index].LastName}");

                Character character = characters[index];


                if (character.CustomizationData.gender == 0)
                {
                    NAPI.Player.SetPlayerSkin(player, PedHash.FreemodeMale01);
                }
                else if (character.CustomizationData.gender == 1)
                {
                    NAPI.Player.SetPlayerSkin(player, PedHash.FreemodeFemale01);
                }

                HeadBlend headBlend = new HeadBlend();
                headBlend.ShapeFirst = (byte)character.CustomizationData.face.mother;
                headBlend.ShapeSecond = (byte)character.CustomizationData.face.father;
                headBlend.ShapeThird = (byte)character.CustomizationData.face.character;
                headBlend.SkinFirst = (byte)character.CustomizationData.skinColor;
                headBlend.SkinSecond = (byte)character.CustomizationData.skinColor;
                headBlend.SkinThird = (byte)character.CustomizationData.skinColor;
                headBlend.ShapeMix = character.CustomizationData.face.resemblance;
                headBlend.SkinMix = character.CustomizationData.face.resemblance;
                headBlend.ThirdMix = 0;
                NAPI.Player.SetPlayerHeadBlend(player, headBlend);

                Dictionary<int, HeadOverlay> headOverlays = new Dictionary<int, HeadOverlay>();
                for (int i = 0; i < character.CustomizationData.head.Count; i++)
                {
                    HeadOverlay ho = new HeadOverlay();
                    ho.Index = (byte)character.CustomizationData.head[i].index;
                    ho.Opacity = character.CustomizationData.head[i].opacity;
                    ho.Color = (byte)character.CustomizationData.head[i].color;
                    headOverlays.Add(i, ho);
                    player.SetHeadOverlay(i, ho);
                }

                for (int i = 0; i < character.CustomizationData.face.data.Count; i++)
                {
                    player.SetFaceFeature(i, character.CustomizationData.face.data[i]);
                }

                player.SetClothes(2, character.CustomizationData.hair.style, 0);
                NAPI.Player.SetPlayerHairColor(player, (byte)character.CustomizationData.hair.color, (byte)character.CustomizationData.hair.highlights);

                // player.SetCustomization(isMale, headBlend, (byte)character.CustomizationData.eyeColor, (byte)character.CustomizationData.hair.color, (byte)character.CustomizationData.hair.highlights, character.CustomizationData.face.data.ToArray(), headOverlays, null);

                player.Position = characters[index].LastPosition.GetVector3();
                player.Rotation = characters[index].LastRotation.GetVector3();

                characters[index].CanAcceptNativeUI = true;
                // player.SetData("Character", characters[index]);
                // player.SetData("Seatbelt", false);
                player.Name = $"[{characters[index].CharacterId}]Stranger_{characters[index].StrangerId}";
                player.Nametag = $"[{characters[index].CharacterId}]Stranger_{characters[index].StrangerId}";
                // player.SetSharedData("StaffMode", false);
                // player.SetData("PermissionLevel", account.PermissionLevel);
                // player.SetData("StaffName", account.StaffName);

                PlayerData.players.Add(player, characters[index]);
                PlayerData.players[player] = characters[index];
                PlayerData.players[player].Seatbelt = false;
                PlayerData.players[player].StaffMode = false;
                PlayerData.players[player].PermissionLevel = account.PermissionLevel;
                PlayerData.players[player].StaffName = account.StaffName;
                PlayerData.players[player].NewbChatCoolDown = DateTime.UtcNow.AddSeconds(-30.0);
                PlayerData.players[player].TimeOfSpawn = DateTime.UtcNow;

                PlayerData.players[player].LastHungerDecrease = DateTime.UtcNow;
                PlayerData.players[player].LastThirstDecrease = DateTime.UtcNow;

                character.LastActive = DateTime.UtcNow;
                character.Update();
                Util.Dimension.DeleteSelectorSession(player);
                player.Dimension = 0;

                player.Health = character.Health;
                player.Armor = character.Armor;

                player.TriggerEvent("displayHud", character.MoneyOnHand, player.Health, player.Armor, character.Hunger, character.Thirst, $"{characters[index].FirstName} {characters[index].LastName}");

                player.SendChatMessage($"~o~Welcome to Impact Roleplay {account.Username}!");
                player.SendChatMessage($"~o~Playing as: ~y~{characters[index].FirstName} {characters[index].LastName}");
                if (PlayerData.players[player].PermissionLevel == Modules.Staff.PermissionLevel.None)
                {
                    player.SendChatMessage($"~o~For questions please type ~y~/newb <question>");
                    player.SendChatMessage($"~o~Need to report a player? Type ~y~/report ~o~to submit a report.");
                }
                if (PlayerData.players[player].PermissionLevel > Modules.Staff.PermissionLevel.None) player.SendChatMessage($"~o~You've logged in as a: ~y~{Enum.GetName(typeof(Modules.Staff.PermissionLevel), PlayerData.players[player].PermissionLevel)}");

                PlayerInventory pinv =  PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
                if (pinv == null)
                {
                    PlayerInventory pinventory = new PlayerInventory();
                    pinventory.CharacterId = PlayerData.players[player].CharacterId;
                     pinventory.AddNew();
                }

                if (PlayerData.players[player.Handle].CurrentJob == Enums.JobType.MiningJob)  Modules.MiningJob.InitalizeMiningPoints(player);
            }
            catch (Exception ex)
            {
                Util.Logging.Log(Util.Logging.LogType.ServerError, $"OnCharacterSpawn.cs {ex.Message} {ex.StackTrace}");
            }
        }
    }
}
