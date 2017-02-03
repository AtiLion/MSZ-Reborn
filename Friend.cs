using System;
using System.Collections.Generic;
using System.Text;

namespace Payload
{
    public class Friend
    {
        public string name;
        public string displayName;
        public ulong ID;

        public Friend(string name, string displayName, ulong ID)
        {
            this.name = name;
            this.displayName = displayName;
            this.ID = ID;
        }
    }
}
