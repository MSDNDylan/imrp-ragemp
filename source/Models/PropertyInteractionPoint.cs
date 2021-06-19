using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Models
{
    public class PropertyInteractionPoint
    {
        public Vector3 Position { get; set; }
        public Marker Marker { get; set; }
        public bool Enabled { get; set; }
        public PropertyInteractionPoint(Vector3 position, Marker marker, bool enabled = true)
        {
            Position = position;
            Marker = marker;
            Enabled = enabled;
        }
    }
}
