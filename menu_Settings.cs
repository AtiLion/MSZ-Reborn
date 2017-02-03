using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Settings : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        private List<Setting> settings = new List<Setting>();

        private DateTime lastCheck;

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

        public void Start()
        {
            isOn = false;
            loadSettings();
        }

        public void Update()
        {
            if (lastCheck == null || (DateTime.Now - lastCheck).TotalMilliseconds >= 10000)
            {
                if ((bool)getSetting("sys_reset").value != ctrl_Connector.reset)
                {
                    ctrl_Connector.reset = (bool)getSetting("sys_reset").value;
                }
                lastCheck = DateTime.Now;
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Settings, window_Main, onWindow, "Settings Menu");
            }
        }

        public void onWindow(int ID)
        {
            /*if (GUILayout.Button((!(bool)getSetting("enable_smartUI").value ? "Enable" : "Disable") + " SmartUI"))
            {
                setSetting("enable_smartUI", !(bool)getSetting("enable_smartUI").value);
            }*/
            if (GUILayout.Button((!(bool)getSetting("enable_instantDisconnect").value ? "Enable" : "Disable") + " Instant Disconnect"))
            {
                setSetting("enable_instantDisconnect", !(bool)getSetting("enable_instantDisconnect").value);
            }
            if (GUILayout.Button((!(bool)getSetting("sys_reset").value ? "Enable" : "Disable") + " Hack Reset"))
            {
                setSetting("sys_reset", !(bool)getSetting("sys_reset").value);
            }
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        private void checkSettings()
        {
            if (!settingExists("enable_smartUI") || !settingExists("enable_instantDisconnect") || !settingExists("hack_banned") || !settingExists("hack_nojoin") || !settingExists("sys_reset"))
            {
                lib_FileSystem.deleteSettings();
                loadSettings();
            }
        }

        public void loadSettings()
        {
            if (lib_FileSystem.existSettings())
            {
                try
                {
                    settings = lib_FileSystem.readSettings();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    lib_FileSystem.deleteSettings();
                    loadSettings();
                    return;
                }
                checkSettings();
            }
            else
            {
                setSetting("enable_smartUI", false, 0);
                setSetting("enable_instantDisconnect", true, 0);
                setSetting("hack_banned", false, 2);
                setSetting("hack_nojoin", false, 2);
                setSetting("sys_reset", false, 0);
            }
        }

        public void saveSettings()
        {
            lib_FileSystem.writeSettings(settings.ToArray());
        }

        public void setSetting(string name, object value, int type = 0)
        {
            if (settingExists(name))
            {
                Array.Find(settings.ToArray(), a => a.name == name).value = value;
            }
            else
            {
                settings.Add(new Setting(name, value, type));
            }
            saveSettings();
        }

        public bool settingExists(string name)
        {
            return Array.Exists(settings.ToArray(), a => a.name == name);
        }

        public Setting getSetting(string name)
        {
            if (settingExists(name))
            {
                return Array.Find(settings.ToArray(), a => a.name == name);
            }
            return new Setting(name, "ERROR", 0);
        }

        public Setting[] getSettings()
        {
            return settings.ToArray();
        }
    }
}
