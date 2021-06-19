using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using GTANetworkInternals;
using IMRP.Database.Collections;
using BCrypt.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IMRP.Commands
{
    public class Developer : Script
    {
        [Command("register")]
        public static void Register(Player player, string username, string password)
        {
            if (Account.GetByUsername(username) == null)
            {
                Console.WriteLine(Account.GetByUsername(username));
                player.SendChatMessage("User already exists! Please login. /login <username> <password>");
                return;
            }
            
            int workFactor = ((DateTime.Now.Year - 2000) / 2) + 6;
            string salt = BCrypt.Net.BCrypt.GenerateSalt(workFactor);
            password = BCrypt.Net.BCrypt.HashPassword(password, salt);

            Account newAccount = new Account();
            newAccount.Username = username;
            newAccount.Password = password;
            newAccount.Salt = salt;
            newAccount.IPAddress.Add(player.Address);
            newAccount.AddNew();
        }

        [Command("scenario")]
        public static void Scenario(Player player, string scenario)
        {
            NAPI.Player.PlayPlayerScenario(player, scenario);
        }
        [Command("setvehtorque")]
        public static void SetTorque(Player player, float torque)
        {
            if (!player.IsInVehicle) return;
            NAPI.Native.SendNativeToPlayer(player, 0xB59E4BD37AE292DB, player.Vehicle.Handle, torque);
            player.SendChatMessage("Set torque");
        }
        [Command("setvehpower")]
        public static void SetPower(Player player, float power)
        {
            if (!player.IsInVehicle) return;
            NAPI.Native.SendNativeToPlayer(player, 0x93A3996368C94158, player.Vehicle.Handle, power);
        }
        [Command("setvehspeed")]
        public static void SetSpeed(Player player, float speed)
        {
            if (!player.IsInVehicle) return;
            NAPI.Native.SendNativeToPlayer(player, 0xAB54A438726D25D5, player.Vehicle.Handle, speed);
        }


        [Command("stopanim")]
        public static void StopAnim(Player player)
        {
            player.StopAnimation();
        }

        [Command("setclothes")]
        public static void SetClothes(Player player, int slot, int drawable, int texture)
        {
            player.SetClothes(slot, drawable, texture);
        }

        [Command("addburger")]
        public static async Task Register(Player player, int qty)
        {
            //burger-item
            PlayerInventory pinv = PlayerInventory.GetByID(PlayerData.players[player].CharacterId);
            Models.InventoryItem invItem = new Models.InventoryItem("Burger", qty, 1.0f, "burger-item");
            pinv.Items.Add(invItem);
            pinv.Update();

            List<Models.InventoryItem> inventoryItems = Modules.Inventory.Character.GetInventoryItems(player);
            string json = JsonConvert.SerializeObject(inventoryItems);
            player.TriggerEvent("refreshPlayerInventory", json);
        }
        [Command("veh")]
        private static void SpawnVehicle(Player player, VehicleHash vehicle)
        {
            GTANetworkAPI.Vehicle newVehicle = NAPI.Vehicle.CreateVehicle(vehicle, new Vector3(player.Position.X, player.Position.Y-3, player.Position.Z+0.5), 1.0f, 0, 0);
            newVehicle.Dimension = player.Dimension;
        }
        [Command("menu")]
        public static void MenuTest(Player player)
        {
            NativeUI.MenuBuilder builder = new NativeUI.MenuBuilder("DEVTEST","Test Title", "Test subtitle");

            NativeUI.MenuItem newMenuItem = new NativeUI.MenuItem();
            newMenuItem.UIMenuItem("Submit", "Complete the Form");
            builder.MenuItems.Add(newMenuItem);

            string json = JsonConvert.SerializeObject(builder);
            player.TriggerEvent("buildMenu", json);
        }
        [Command("tp")]
        public static void Teleport(Player player, float x, float y, float z)
        {
            player.Position =  new Vector3(x,y,z);
        }
        [Command("loadipl")]
        public static void LoadIPL(Player player, string ipl)
        {
            NAPI.World.RequestIpl(ipl);
            
            player.SendChatMessage($"{ipl} loaded.");
        }
        [Command("getrot")]
        public static void GetRotation(Player player)
        {
            Console.WriteLine($"Position: {player.Position}");
            Console.WriteLine($"Rotation: {player.Rotation}");
            player.SendChatMessage($"Position: {player.Position}");
            player.SendChatMessage($"Rotation: {player.Rotation}");
        }
        [Command("getground")]
        public static void GetGround(Player player)
        {
            player.TriggerEvent("getGroundZ");

        }


        /* [Command("createmarker")]
        public static void CreateMarker (Player player, int markerType)
        {
            Marker newMarker = NAPI.Marker.CreateMarker(markerType, player.Position, player.Position, player.Rotation, 1.0f, 255, 255, 255, false, player.Dimension);
            player.TriggerEvent("createMarker", newMarker);
        }

        [Command("rotatemarker")]
        public static void RotateMarker (Player player, double x, double y, double z)
        {
            newMarker.Rotation = new Vector3(x, y, z);
        }

        [Command("dirmarker")]
        public static void MarkerDirection(Player player, double x, double y, double z)
        {
            newMarker.Direction = new Vector3(x, y, z);
            
        }
        [Command("getground")]
        public static void GetGround(Player player)
        {
            player.TriggerEvent("getGroundZ");

        }

        [Command("positionmarker")]
        public static void PositionMarker(Player player, double x, double y, double z)
        {
            newMarker.Position = new Vector3(x, y, z);
        }

        [Command("getmarker")]
        public static void GetMarker(Player player)
        {
            newMarker.Position = player.Position;
        }
        [Command("getmarkercoords")]
        public static void CoordsMarker(Player player)
        {
            Console.WriteLine($"{newMarker.Position.X} {newMarker.Position.Y} {newMarker.Position.Z}");
            Console.WriteLine($"{newMarker.Rotation.X} {newMarker.Rotation.Y} {newMarker.Rotation.Z}");
        }*/
    }
}
