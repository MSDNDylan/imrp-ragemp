using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace IMRP.Events
{
    public class ColShapeEvents : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void OnPlayerEnterColShapeAsync (ColShape col, Player player)
        {
            if(ServerData.ColShapes.ContainsKey(col.Handle))
            {
                switch(ServerData.ColShapes[col.Handle].ColShapeType)
                {
                    case Enums.ColShapeType.JobEmployPoint:
                        List<Database.Collections.JobConfiguration> allJobs = Database.Collections.JobConfiguration.GetAll();
                        foreach(Database.Collections.JobConfiguration job in allJobs)
                        {
                            if(job.JobEmployColShape3DId == ServerData.ColShapes[col.Handle].ColShapeCylinderId)
                            {
                                Menus.EmployJob.Open(player, Enums.JobType.MiningJob);
                            }
                        }
                        break;
                }
            }
        }
        [ServerEvent(Event.PlayerExitColshape)]
        public void OnPlayerExitColShape (ColShape col, Player player)
        {
            if (ServerData.ColShapes.ContainsKey(col.Handle))
            {
                switch (ServerData.ColShapes[col.Handle].ColShapeType)
                {
                    case Enums.ColShapeType.JobEmployPoint:
                        List<Database.Collections.JobConfiguration> allJobs = Database.Collections.JobConfiguration.GetAll();
                        foreach (Database.Collections.JobConfiguration job in allJobs)
                        {
                            if (job.JobEmployColShape3DId == ServerData.ColShapes[col.Handle].ColShapeCylinderId)
                            {
                                Menus.EmployJob.Close(player);
                            }
                        }
                        break;
                }
            }
        }
    }
}
