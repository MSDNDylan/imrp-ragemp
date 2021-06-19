using System;
using System.Collections.Generic;
using System.Text;

namespace IMRP.Models
{
    public class InventoryItem
    {
        public int InventoryID { get; set; } = -1;
        public string Name { get; set; }
        public int Qty { get; set; }
        public int Quality { get; set; }
        public float Weight { get; set; }
        public string Image { get; set; }
        public InventoryItem(string name, int qty, float weight, string image, int quality = -1)
        {
            Name = name;
            Qty = qty;
            Weight = weight;
            Image = image;
            Quality = quality;
        }
    }
}
