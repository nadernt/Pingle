using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pingle
{
    class NetResponse
    {
        public string Address { get; set; }
        public bool Sucess { get; set; }
        public long RoundtripTime { get; set; }
        public int Ttl { get; set; }
        public bool DontFragment { get; set; }
        public int Lengh { get; set; }
    }
}
