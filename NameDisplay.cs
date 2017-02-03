using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Payload
{
    public class NameDisplay
    {
        public float distance;
        public GameObject go;
        public Vector3 rPos;
        public string name;

        public NameDisplay(float distance, GameObject go, Vector3 rPos, string name)
        {
            this.distance = distance;
            this.go = go;
            this.rPos = rPos;
            this.name = name;
        }
    }
}
