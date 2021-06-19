using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMRP.Database.Collections;
using GTANetworkAPI;

namespace IMRP
{
    public class VehicleData
    {
        public static Database.Collections.Vehicle GetVehicleData(GTANetworkAPI.Vehicle vehicle)
        {
            return vehicle.GetData<Database.Collections.Vehicle>("Vehicle");
        }
    }
}
