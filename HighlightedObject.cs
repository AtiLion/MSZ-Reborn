using System;
using System.Collections.Generic;
using System.Text;
using HighlightingSystem;
using UnityEngine;

namespace Payload
{
    public class HighlightedObject
    {
        public int dType; // 0 = Player, 1 = Zombie, 2 = Vehicle, 3 = Item, 4 = Animal, Storage = 5
        public GameObject go;
        public Highlighter h;

        public HighlightedObject(int dType, GameObject go, Highlighter h)
        {
            this.dType = dType;
            this.go = go;
            this.h = h;
        }
    }
}
