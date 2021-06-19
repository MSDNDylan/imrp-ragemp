using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMRP.Util
{
    public class Timers
    {
        public delegate void Timer1SecondElapsedHandler();
        public static event Timer1SecondElapsedHandler Timer1SecondElapsedEvent;

        private static System.Timers.Timer OneSecondTimer;
        public static void NewTimer(int interval, CancellationToken cancellationToken)
        {
            OneSecondTimer = new System.Timers.Timer(interval);
            OneSecondTimer.Elapsed += OneSecondTimer_Elapsed;
        }

        private static void OneSecondTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Timer1SecondElapsedEvent.Invoke();
        }
    }
}
