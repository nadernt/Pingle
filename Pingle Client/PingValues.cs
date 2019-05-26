using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PingleClient
{
    [DataContract]
    public class PingValues
    {
        [DataMember()]
        public string Address { get; private set; }
        [DataMember()]
        public bool Sucess { get; private set; }
        [DataMember()]
        public long RoundtripTime { get; private set; }
        [DataMember()]
        public int Ttl { get; private set; }
        [DataMember()]
        public bool DontFragment { get; private set; }
        [DataMember()]
        public int Lengh { get; private set; }
        [DataMember()]
        public long Time { get; private set; }


        public PingValues(string Addres, bool Sucess, long RoundtripTime, int Ttl,bool DontFragment, int Lengh, long Time)
        {
            this.Address = Addres;
            this.Sucess = Sucess;
            this.RoundtripTime = RoundtripTime;
            this.Ttl = Ttl;
            this.DontFragment = DontFragment;
            this.Lengh = Lengh;
            this.Time = Time;
        }

      
    }
}
