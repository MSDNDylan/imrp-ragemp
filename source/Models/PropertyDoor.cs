using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace IMRP.Models
{
    public class PropertyDoor
    {
        public Vector3 ExteriorPosition { get; set; }
        public Vector3 ExteriorRotation { get; set; }
        public Vector3 InteriorPosition { get; set; }
        public Vector3 InteriorRotation { get; set; }
        public int InteriorColShapeId { get; set; }
        public int ExteriorColShapeId { get; set; }
        public bool Locked { get; set; }
        public int InteriorMarkerId { get; set; }
        public int ExteriorMarkerId { get; set; }
        public int TextLabelId { get; set; }

        public PropertyDoor()
        {
            InteriorMarkerId = -1;
            ExteriorMarkerId = -1;
            TextLabelId = -1;
        }
    }
}
