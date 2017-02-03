using System;
using System.Collections.Generic;
using System.Text;

namespace Payload
{
    public class Setting
    {
        public string name;
        public object value;
        public int type; // 0 = Toggle

        public Setting(string name, object value, int type)
        {
            this.name = name;
            this.value = value;
            this.type = type;
        }
    }
}
