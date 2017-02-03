using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_CustomItem : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 200);
        private Vector2 scroll;

        private List<ushort> items = new List<ushort>();
        private string text = "";

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

        private void loadCustomItem()
        {
            if (lib_FileSystem.existCustomItem())
            {
                try
                {
                    items = lib_FileSystem.readCustomItem();
                }
                catch (Exception ex)
                {
                    lib_FileSystem.deleteCustomItem();
                    loadCustomItem();
                }
            }
            else
            {
                lib_FileSystem.createCustomItem();
            }
        }

        private void saveCustomItem()
        {
            lib_FileSystem.writeCustomItem(items.ToArray());
        }

        private void addCustomItem(ushort id)
        {
            items.Add(id);
            saveCustomItem();
        }

        private void removeCustomItem(ushort id)
        {
            items.Remove(id);
            saveCustomItem();
        }

        public bool existsCustomItem(ushort id)
        {
            return Array.Exists(items.ToArray(), a => a == id);
        }

        public ushort[] getCustomItem()
        {
            return items.ToArray();
        }

        public void Start()
        {
            isOn = false;
            loadCustomItem();
        }

        public void Update()
        {
            if (Event.current.type == EventType.KeyDown && Event.current.character == '\n')
            {
                if (text != "" && text != null)
                {
                    ushort num;
                    if (ushort.TryParse(text, out num))
                    {
                        addCustomItem(num);
                    }
                    text = "";
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_CustomItem, window_Main, onWindow, "Custom Item Selection");
            }
        }

        public void onWindow(int ID)
        {
            text = GUILayout.TextField(text);
            scroll = GUILayout.BeginScrollView(scroll);
            foreach (ushort i in items)
            {
                if (GUILayout.Button(i.ToString()))
                {
                    removeCustomItem(i);
                }
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }
    }
}
