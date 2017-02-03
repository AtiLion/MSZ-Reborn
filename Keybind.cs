using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Payload
{
    public class Keybind
    {
        public string name;
        public string text;
        public KeyCode key;
        public bool getting;

        public Keybind(string name, string text, KeyCode key)
        {
            this.name = name;
            this.text = text;
            this.key = key;
            this.getting = false;
        }
    }
}
