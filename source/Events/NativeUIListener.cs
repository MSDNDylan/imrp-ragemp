using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IMRP.Database.Collections;
using IMRP.Enums;
using IMRP.Models;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class NativeUIListener : Script
    {
        public delegate void NativeUIMenuSelectedHandler(Player player, string argument0="", string argument1="", string argument2="", string argument3="", string argument4="");

        public static event NativeUIMenuSelectedHandler NativeUIMenuSelected;

        [RemoteEvent("InvokeNativeUIListener")]
        public void ListenNativeUI(Player player, params object[] arguments)
        {
            string menuType = (string)arguments[0];
            string activeMenuID = (string)arguments[1];

            if(menuType == "UIMenuListItem")
            {
                 Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{arguments[2]} {arguments[3]}");
                 NativeUIMenuSelected.Invoke(player, menuType, activeMenuID, (string)arguments[2], (string)arguments[3]);
                
            }
            else if(menuType == "UIMenuSliderItem")
            {
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{arguments[2]} {arguments[3]} {arguments[4]}");
                 NativeUIMenuSelected.Invoke(player, menuType, activeMenuID, (string)arguments[2], (string)arguments[3], (string)arguments[4]);
            }
            else
            {
                Util.Logging.Log(Util.Logging.LogType.ServerInfo, $"{arguments[2]}");
                 NativeUIMenuSelected.Invoke(player, menuType, activeMenuID, (string)arguments[2]);
            }
        }
    }
}
