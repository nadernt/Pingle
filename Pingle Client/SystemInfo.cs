using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingleClient
{
    class SystemInfo
    {
        public string Item {get; private set; }
        public string Value { get; private set; } 

        public SystemInfo(string Item, string Value)
        {
            this.Item = Item;
            this.Value = Value;
        }
    }
}
