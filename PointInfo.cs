using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class PointInfo
    {
        public int x;
        public int y;
        public Color color;

        public PointInfo(int x, int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }
}
