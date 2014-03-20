using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace nyobaping
{
    class PingResult
    {
        public IPAddress Address { get; private set; }
        public int PingsTotal { get; private set; }
        public int PingsSuccessfull { get; private set; }
        public TimeSpan AverageTime { get; private set; }
        public TimeSpan LastTime { get; private set; }
        public IPStatus LastStatus { get; private set; }

        public PingResult(IPAddress address)
        {
            Address = address;

            LastStatus = IPStatus.Unknown;
        }

        internal void AddResult(PingReply res)
        {
            PingsTotal++;
            LastStatus = res.Status;

            if (res.Status == IPStatus.Success)
            {
                PingsSuccessfull++;
                LastTime = TimeSpan.FromMilliseconds(res.RoundtripTime);
                if (PingsSuccessfull == 1)
                    AverageTime = LastTime;
                else
                {
                    var oldAverage = AverageTime.TotalMilliseconds;
                    AverageTime = TimeSpan.FromMilliseconds(oldAverage + (res.RoundtripTime - oldAverage) / PingsSuccessfull);
                }
            }
            else
            {
                LastTime = TimeSpan.Zero;
            }

        }
    }
}
