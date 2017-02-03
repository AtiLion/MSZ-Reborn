using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Keybind : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        private List<Keybind> keybinds = new List<Keybind>();
        private bool rebinding = false;

        public bool getIsOn()
        {
            return isOn;
        }

        public void setIsOn(bool a)
        {
            isOn = a;
        }

        public void toggleOn()
        {
            isOn = !isOn;
        }

        private void loadKeybinds()
        {
            if (lib_FileSystem.existKeybinds())
            {
                try
                {
                    keybinds = lib_FileSystem.readKeybinds();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    lib_FileSystem.deleteKeybinds();
                    loadKeybinds();
                    return;
                }
                checkKeybinds();
            }
            else
            {
                setKeybind("menu", "Main Menu", (int)KeyCode.F1);
                setKeybind("aimbot", "Aimbot", 0);
                setKeybind("triggerbot", "Triggerbot", 0);
                setKeybind("aimlock", "Aimlock", 0);
                setKeybind("autoitempickup", "Auto Item Pickup", 0);
            }
        }

        private void checkKeybinds()
        {
            if (!keybindExists("menu") || !keybindExists("aimbot") || !keybindExists("triggerbot") || !keybindExists("aimlock") || !keybindExists("autoitempickup"))
            {
                lib_FileSystem.deleteKeybinds();
                loadKeybinds();
            }
        }

        private void setKeybind(string name, string text, int key = 0)
        {
            if (keybindExists(name))
            {
                Array.Find(keybinds.ToArray(), a => a.name == name).key = (KeyCode)key;
            }
            else
            {
                keybinds.Add(new Keybind(name, text, (KeyCode)key));
            }
            saveKeybinds();
        }

        private void saveKeybinds()
        {
            lib_FileSystem.writeKeybinds(keybinds.ToArray());
        }

        private bool keybindExists(string name)
        {
            return Array.Exists(keybinds.ToArray(), a => a.name == name);
        }

        private void useKey(string name)
        {
            if (name == "menu")
            {
                ctrl_Connector.hack_Main.toggleOn();
                ctrl_Connector.isOn = !ctrl_Connector.isOn;
            }
            else if (name == "aimbot")
            {
                if (ctrl_Connector.isPremium(Provider.client.m_SteamID))
                {
                    ctrl_Connector.hack_Aimbot.enabled = !ctrl_Connector.hack_Aimbot.enabled;
                }
            }
            else if (name == "triggerbot")
            {
                ctrl_Connector.hack_AimlockTriggerbot.triggerbot = !ctrl_Connector.hack_AimlockTriggerbot.triggerbot;
            }
            else if (name == "aimlock")
            {
                ctrl_Connector.hack_AimlockTriggerbot.aimlock = !ctrl_Connector.hack_AimlockTriggerbot.aimlock;
            }
            else if (name == "autoitempickup")
            {
                ctrl_Connector.hack_ItemPickup.itemPickup = !ctrl_Connector.hack_ItemPickup.itemPickup;
            }
        }

        public void Start()
        {
            isOn = false;
            loadKeybinds();
        }

        public void Update()
        {
            Event cur = Event.current;
            if (cur.type == EventType.KeyDown && cur.keyCode != null)
            {
                KeyCode k = cur.keyCode;
                if (rebinding)
                {
                    if (k == KeyCode.Escape || k == KeyCode.Backspace)
                    {
                        Array.Find(keybinds.ToArray(), a => a.getting == true).key = (KeyCode)0;
                    }
                    else
                    {
                        Array.Find(keybinds.ToArray(), a => a.getting == true).key = k;
                    }
                    Array.Find(keybinds.ToArray(), a => a.getting == true).getting = false;
                    rebinding = false;
                    saveKeybinds();
                }
                else
                {
                    if (Array.Exists(keybinds.ToArray(), a => a.key != 0 && a.key == k))
                    {
                        useKey(Array.Find(keybinds.ToArray(), a => a.key != 0 && a.key == k).name);
                    }
                    cur = null;
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Keybind, window_Main, onWindow, "Keybind Menu");
            }
        }

        public void onWindow(int ID)
        {
            foreach (Keybind k in keybinds)
            {
                if (GUILayout.Button((k.getting ? "Press any key" : k.text + ": " + k.key)))
                {
                    rebinding = true;
                    k.getting = true;
                }
            }
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }
    }
}
