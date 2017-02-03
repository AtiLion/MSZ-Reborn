using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SDG.Unturned;
using UnityEngine;

namespace Payload
{
    class Payload : MonoBehaviour
    {
        static GameObject hookObj = null;

        private Camera cam = Camera.main;

        private ctrl_Connector con = new ctrl_Connector();

        public void Start()
        {

        }

        public void Update()
        {
            con.onUpdate();
        }

        public void OnGUI()
        {
            con.onGUI();
        }
    }
}
