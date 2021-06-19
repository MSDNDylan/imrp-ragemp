using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class MarkerEvents
    {
        [RemoteEvent("CreateMarker")]
        public void CreateMarker (Player player, params object[] arguments)
        {
            Marker marker = (Marker)arguments[0];
        }
    }
}
