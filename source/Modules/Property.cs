using System;
using System.Collections.Generic;
using System.Text;
using IMRP.Database.Collections;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using IMRP.Models;

namespace IMRP.Modules
{
    public class Property
    {
        public static Dictionary<NetHandle, PropertyDoor> CreatingDoor = new Dictionary<NetHandle, PropertyDoor>();
        public static Dictionary<NetHandle, Database.Collections.Property> CurrentlyEditingProperty = new Dictionary<NetHandle, Database.Collections.Property>();
        
        [Command("createproperty", GreedyArg = true)]
        public async Task CreatePropertyAsync(Player player, Enums.OwnershipType ownershipType, Enums.PropertyClass classification, Enums.PropertyType type, string name)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            Database.Collections.Property newProperty = new Database.Collections.Property();
            newProperty.OwershipType = ownershipType;
            newProperty.OwnerId = -1;
            newProperty.OwershipType = Enums.OwnershipType.Player;
            newProperty.PropertyClass = classification;
            newProperty.PropertyType = type;
            newProperty.PropertyName = name;
            newProperty.AddNew();

            player.SendChatMessage($"A {Enum.GetName(typeof(Enums.PropertyClass), classification)} property with ID {newProperty.PropertyId} named {name} has been created.");
            player.SendChatMessage($"Currently editing property ID: {newProperty.PropertyId}");
        }

        [Command("editproperty")]
        public void EditProperyAsync(Player player, int propertyId = -1)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;

            if(propertyId == -1)
            {
                if(CurrentlyEditingProperty.ContainsKey(player.Handle))
                {
                    Util.ChatMessage.SendNotification(player, $"You are no longer editing property {CurrentlyEditingProperty[player.Handle].PropertyId}");
                    CurrentlyEditingProperty.Remove(player.Handle);
                }
            }
            else
            {
                foreach (var kvp in CurrentlyEditingProperty.ToArray())
                {
                    if (CurrentlyEditingProperty[kvp.Key].PropertyId == propertyId)
                    {
                        Util.ChatMessage.SendErrorChatMessage(player, $"Property {propertyId} is already being edited by {PlayerData.players[kvp.Key].StaffName}");
                        return;
                    }
                }

                Database.Collections.Property property =  Database.Collections.Property.GetByID(propertyId);
                if (CurrentlyEditingProperty.ContainsKey(player.Handle))
                {
                    CurrentlyEditingProperty.Add(player.Handle, property);
                }
                else
                {
                    CurrentlyEditingProperty[player.Handle] = property;
                }

                Util.ChatMessage.SendNotification(player, $"You are currently editing property: {propertyId}");
            }
        }

        [Command("deleteproperty")]
        public void DeletePropertyAsync(Player player, int propertyId)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;
            if (!IsCurrentlyEditingProperty(player)) return;

             CurrentlyEditingProperty[player.Handle].Delete();
            if(CurrentlyEditingProperty.ContainsKey(player.Handle)) CurrentlyEditingProperty.Remove(player.Handle);

            Util.ChatMessage.SendNotification(player, $"Property {CurrentlyEditingProperty[player.Handle].PropertyId} has been deleted");
        }

        [Command("setaddress", GreedyArg = true)]
        public void SetPropertyAddressAsync(Player player, string newAddress)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;
            if (!IsCurrentlyEditingProperty(player)) return;

            CurrentlyEditingProperty[player.Handle].PropertyAddress = newAddress;
             CurrentlyEditingProperty[player.Handle].Update();

            Util.ChatMessage.SendNotification(player, $"Property {CurrentlyEditingProperty[player.Handle].PropertyId} address changed to {CurrentlyEditingProperty[player.Handle].PropertyAddress}");
        }
        public static bool IsCurrentlyEditingProperty(Player player)
        {
            bool currentlyEditing = CurrentlyEditingProperty.ContainsKey(player.Handle);
            if (!currentlyEditing) Util.ChatMessage.SendErrorChatMessage(player, "You are not currently editing any properties! /editproperty [propertyId]");
            return currentlyEditing;
        }

        [Command("setname", GreedyArg = true)]
        public void SetPropertyNameAsync(Player player, string name)
        {
            if (!Staff.IsSufficentStaffLevel(player, Staff.PermissionLevel.Administrator)) return;
            if (!IsCurrentlyEditingProperty(player)) return;

            CurrentlyEditingProperty[player.Handle].PropertyName = name;
             CurrentlyEditingProperty[player.Handle].Update();

            Util.ChatMessage.SendNotification(player, $"Property {CurrentlyEditingProperty[player.Handle].PropertyId} name changed to {CurrentlyEditingProperty[player.Handle].PropertyName}");
        }

        [Command("createdoor")]
        public void CreateDoor(Player player)
        {
            if(CreatingDoor.ContainsKey(player.Handle))
            {
                Util.ChatMessage.SendErrorChatMessage(player, $"You are already currently creating a door.");
                return;
            }
            else
            {
                PropertyDoor propertyDoor = new PropertyDoor();
                CreatingDoor.Add(player.Handle, propertyDoor);
                Util.ChatMessage.SendNotification(player, "You have begun creating a door!");
            }
        }

        [Command("setdoor")]
        public void setdoorAsync(Player player, string property)
        {
            if (!CreatingDoor.ContainsKey(player.Handle))
            {
                Util.ChatMessage.SendErrorChatMessage(player, $"You are not currently creating a door.");
                return;
            }

            switch(property)
            {
                case "interior":

                    CreatingDoor[player.Handle].InteriorPosition = player.Position;
                    CreatingDoor[player.Handle].InteriorRotation = player.Rotation;

                    if(CreatingDoor[player.Handle].InteriorMarkerId == -1)
                    {
                        Database.Collections.Marker newMarker = new Database.Collections.Marker();
                        newMarker.Direction = new IMVector3(new Vector3(0, 0, 0));
                        newMarker.Position = new IMVector3(player.Position);
                        newMarker.Rotation = new IMVector3(player.Rotation);
                        newMarker.MarkerType = 6;
                        newMarker.Red = 255;
                        newMarker.Green = 255;
                        newMarker.Blue = 255;
                        newMarker.Alpha = 255;
                        newMarker.BobUpAndDown = false;
                        newMarker.Dimension = player.Dimension;
                        newMarker.Scale = 1.0f;
                         newMarker.AddNew();
                        CreatingDoor[player.Handle].InteriorMarkerId = newMarker.MarkerId;
                    }
                    else
                    {
                        Database.Collections.Marker marker =  Database.Collections.Marker.GetByID(CreatingDoor[player.Handle].InteriorMarkerId);
                        marker.Position = new IMVector3(player.Position);
                        marker.Rotation = new IMVector3(player.Rotation);
                        marker.Dimension = player.Dimension;
                         marker.Update();
                    }

                    break;
                case "exterior":
                    CreatingDoor[player.Handle].ExteriorPosition = player.Position;
                    CreatingDoor[player.Handle].ExteriorRotation = player.Rotation;

                    if (CreatingDoor[player.Handle].ExteriorMarkerId == -1)
                    {
                        Database.Collections.Marker newMarker = new Database.Collections.Marker();
                        newMarker.Direction = new IMVector3(new Vector3(0, 0, 0));
                        newMarker.Position = new IMVector3(player.Position);
                        newMarker.Rotation = new IMVector3(player.Rotation);
                        newMarker.MarkerType = 6;
                        newMarker.Red = 255;
                        newMarker.Green = 255;
                        newMarker.Blue = 255;
                        newMarker.Alpha = 255;
                        newMarker.BobUpAndDown = false;
                        newMarker.Dimension = player.Dimension;
                        newMarker.Scale = 1.0f;
                         newMarker.AddNew();
                        CreatingDoor[player.Handle].ExteriorMarkerId = newMarker.MarkerId;
                    }
                    else
                    {
                        Database.Collections.Marker marker =  Database.Collections.Marker.GetByID(CreatingDoor[player.Handle].ExteriorMarkerId);
                        marker.Position = new IMVector3(player.Position);
                        marker.Rotation = new IMVector3(player.Rotation);
                        marker.Dimension = player.Dimension;
                         marker.Update();
                    }
                    break;
            }
        }
    }
}
