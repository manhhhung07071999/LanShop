using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Mvc;

namespace System
{
    public class AppTimer : System.Windows.Threading.DispatcherTimer, ITimer
    {
        int _delay;
        public int Delay
        {
            get { return _delay; }
            set {  base.Interval = TimeSpan.FromMilliseconds(_delay = value); }
        }
    }
    public class Counter : SystemClock
    {
        protected override ITimer CreateTimer()
        {
            return new AppTimer { Delay = 100 };
        }

        public static ITimer Start(int delay, Func<int, bool> callback)
        {
            var ts = new AppTimer { Delay = delay };
            int count = 0;
            ts.Tick += delegate {
                if (callback(count++) == false)
                    ts.Stop();
            };
            ts.Start();

            return ts;
        }
    }
}
