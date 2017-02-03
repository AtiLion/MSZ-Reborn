using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Main : MonoBehaviour
    {
        private bool isOn;

        // Components
        private Rect window_Main = new Rect(10, 10, 200, 10);
        private Vector2 scroll;

        private DateTime lastCheck;
        public bool banned_hack = false;
        public bool banned_game = false;

        public bool getIsOn()
        {
            return isOn;
        }

        public void setIsOn(bool a)
        {
            isOn = a;
            onToggleUpdate();
        }

        public void toggleOn()
        {
            isOn = !isOn;
            onToggleUpdate();
        }

        public void onToggleUpdate()
        {
            if (isOn)
            {
                PlayerUI.window.showCursor = true;
            }
            else
            {
                PlayerUI.window.showCursor = false;
            }
        }

        public void Start()
        {
            isOn = false;
        }

        public void Update()
        {
            if (lastCheck == null || (DateTime.Now - lastCheck).TotalMilliseconds >= 5000)
            {
                banned_hack = (bool)ctrl_Connector.hack_Settings.getSetting("hack_banned").value;
                banned_game = (bool)ctrl_Connector.hack_Settings.getSetting("hack_nojoin").value;
                if (ctrl_Connector.isDebug)
                {
                    if (ctrl_Connector.skid.isBeta(tool_ToolZ.getPlayerID(tool_ToolZ.getLocalPlayer())) || ctrl_Connector.skid.isWhitelist(tool_ToolZ.getPlayerID(tool_ToolZ.getLocalPlayer())))
                    {
                        ctrl_Connector.allow = true;
                    }
                    else
                    {
                        ctrl_Connector.allow = false;
                    }
                }
                else
                {
                    ctrl_Connector.allow = true;
                }
            }
            if (banned_game)
            {
                Provider.disconnect();
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                PlayerUI.window.showCursor = true;
                window_Main = GUILayout.Window(ctrl_Connector.id_Main, window_Main, onWindow, tool_ToolZ.getTitle());
            }
        }

        public void onWindow(int ID)
        {
            if (ctrl_Connector.allow && !banned_hack)
            {
                if (GUILayout.Button("Player Hacks"))
                {
                    ctrl_Connector.hack_Player.toggleOn();
                }
                if (ctrl_Connector.isPremium(Provider.client.m_SteamID))
                {
                    if (GUILayout.Button("Aimbot"))
                    {
                        ctrl_Connector.hack_Aimbot.toggleOn();
                    }
                }
                if (GUILayout.Button("ESP"))
                {
                    ctrl_Connector.hack_ESP.toggleOn();
                }
                if (GUILayout.Button("Aimlock/Triggerbot"))
                {
                    ctrl_Connector.hack_AimlockTriggerbot.toggleOn();
                }
                if (GUILayout.Button("Weapon Hacks"))
                {
                    ctrl_Connector.hack_Weapons.toggleOn();
                }
                if (GUILayout.Button("Vehicle Hacks"))
                {
                    ctrl_Connector.hack_Vehicle.toggleOn();
                }
                if (GUILayout.Button("Auto Item Pickup"))
                {
                    ctrl_Connector.hack_ItemPickup.toggleOn();
                }
                if (ctrl_Connector.skid.isWhitelist(tool_ToolZ.getPlayerID(tool_ToolZ.getLocalPlayer())) && ctrl_Connector.hide)
                {
                    if (GUILayout.Button("Item Spawner"))
                    {
                        ctrl_Connector.hack_Debug.toggleOn();
                    }
                    if (GUILayout.Button("/buy Exploiter"))
                    {
                    }
                }
                if (GUILayout.Button("Item Selection"))
                {
                    ctrl_Connector.hack_ItemSelection.toggleOn();
                }
                if (GUILayout.Button("Add Friend"))
                {
                    ctrl_Connector.hack_Friends.sw = true;
                    ctrl_Connector.hack_Friends.toggleOn();
                }
                if (GUILayout.Button("Remove Friend"))
                {
                    ctrl_Connector.hack_Friends.sw = false;
                    ctrl_Connector.hack_Friends.toggleOn();
                }
                if (GUILayout.Button("Keybinds"))
                {
                    ctrl_Connector.hack_Keybind.toggleOn();
                }
                if (GUILayout.Button("Settings"))
                {
                    ctrl_Connector.hack_Settings.toggleOn();
                }
                if (GUILayout.Button("Fun Menu"))
                {
                    ctrl_Connector.hack_Fun.toggleOn();
                }
            }
            else
            {
                if (banned_hack)
                {
                    GUILayout.Label("YOU HAVE BEEN BANNED FROM USING THE HACK!");
                }
                else
                {
                    GUILayout.Label("YOU ARE NOT A BETA TESTER!");
                }
            }
            GUI.DragWindow();
        }
    }
}
