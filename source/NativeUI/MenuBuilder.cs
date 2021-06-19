using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace IMRP.NativeUI
{
    public class MenuBuilder
    {
        public string MenuID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Point Point { get; set; } = new Point(1480,25);

        public List<MenuItem> MenuItems = new List<MenuItem>();

        public MenuBuilder(string menuId, string title, string subtitle)
        {
            MenuID = menuId;
            Title = title;
            SubTitle = subtitle;
        }

        public MenuItem NewMenuItem()
        {
            return new MenuItem();
        }

        public void SendMenuToplayer(Player player)
        {
            if(PlayerData.GetCharacter(player) != null)
            {
                if(PlayerData.GetCharacter(player).CanAcceptNativeUI)
                {
                    player.TriggerEvent("buildMenu", JsonConvert.SerializeObject(this));
                }
            }
        }
        public static void CloseMenu(Player player)
        {
            player.TriggerEvent("destroyMenu");
        }
    }
    public class MenuItem
    {
        public string MenuType { get; set; }
        public dynamic Properties { get; set; } = new object();

        public MenuItem UIMenuListItem(string caption, string descrtipion, string[] fields)
        {
            MenuType = "UIMenuListItem";
            Properties = new UIMenuListItem(caption, descrtipion, fields);
            return this;
        }
        public MenuItem UIMenuCheckboxItem(string caption, string description, bool defaultValue = false)
        {
            MenuType = "UIMenuCheckboxItem";
            Properties = new UIMenuCheckboxItem(caption, description, defaultValue);
            return this;
        }
        public MenuItem UIMenuSliderItem(string caption, string description,string[] fields, bool divider = false, int startIndex = 0)
        {
            MenuType = "UIMenuSliderItem";
            Properties = new UIMenuSliderItem(caption, description, fields, divider, startIndex);
            return this;
        }

        public MenuItem UIMenuItem(string caption, string description)
        {
            MenuType = "UIMenuItem";
            Properties = new UIMenuItem(caption, description);
            return this;
        }
    }
    public class UIMenuItem
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        
        public int SortOrder { get; set; }
        public Color BackColor { get; set; } = null;
        public Color HighlightedBackColor { get; set; } = null;
        public Color ForgroundColor { get; set; } = null;
        public Color HighlightedForeColor { get; set; } = null;
        public UIMenuItem(string caption, string description)
        {
            Caption = caption;
            Description = description;
        }
        public void ForeColor(int red, int green, int blue, int alpha)
        {
            ForgroundColor = new Color(red, green, blue, alpha);
        }
    }
    public class UIMenuSliderItem
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Divider { get; set; }
        public string[] Fields { get; set; }
        public int StartIndex { get; set; }

        public UIMenuSliderItem(string caption, string description, string[] fields, bool divider = false, int startIndex = 0)
        {
            Caption = caption;
            Description = description;
            Divider = divider;
            Fields = fields;
            StartIndex = startIndex;
        }
    }
    public class UIMenuCheckboxItem
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public bool Checked { get; set; }
        public int SortOrder { get; set; }

        public UIMenuCheckboxItem(string caption, string description, bool defaultValue = false)
        {
            Caption = caption;
            Description = description;
            Checked = defaultValue;
        }
    }
    public class UIMenuListItem
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string[] Fields { get; set; }
        public int SortOrder { get; set; }
        public UIMenuListItem(string caption, string descrtipion, string[] fields)
        {
            Caption = caption;
            Description = descrtipion;
            Fields = fields;
        }
    }
    public enum BadgeStyle
    {
        None,
        BronzeMedal,
        GoldMedal,
        SilverMedal,
        Alert,
        Crown,
        Ammo,
        Armour,
        Barber,
        Clothes,
        Franklin,
        Bike,
        Car,
        Gun,
        Heart,
        Makeup,
        Mask,
        Michael,
        Star,
        Tatoo,
        Trevor,
        Lock,
        Tick
    }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public enum Font
    {
        ChaletLondon = 0,
        HouseScript = 1,
        Monospace = 2,
        CharletComprimeColonge = 4,
        Pricedown = 7
    }
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    public class Color
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int Alpha { get; set; }

        public Color(int red, int green, int blue, int alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
