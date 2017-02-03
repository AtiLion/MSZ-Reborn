using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace Payload
{
    public class lib_SmartUI : MonoBehaviour
    {
        private List<string> displayInfo = new List<string>();
        private bool hasCrack = true;
        private Interactable inte;
        private Rect info_window = new Rect(Screen.width - 500, Screen.height - 200, 500, 200);

        private bool enabled = false;
        private DateTime lastCheck;

        public void Start()
        {

        }

        public void Update()
        {
            if (lastCheck == null || (DateTime.Now - lastCheck).TotalMilliseconds >= 1500)
            {
                //enabled = (bool)ctrl_Connector.hack_Settings.getSetting("enable_smartUI").value;
                lastCheck = DateTime.Now;
            }
            if (enabled)
            {
                displayInfo.Clear();
                if (PlayerInteract.interactable != null)
                {
                    if (PlayerInteract.interactable is InteractableStorage)
                    {
                        InteractableStorage storage = (InteractableStorage)PlayerInteract.interactable;
                        displayInfo.Add("Type: Storage");
                        //if (storage.owner != CSteamID.Nil)
                        //{
                            //displayInfo.Add("Owner: " + tool_ToolZ.getSteamPlayer(storage.owner.m_SteamID).playerID.playerName);
                        //}
                        displayInfo.Add("Locked: " + (getLocked(storage) ? "Yes" : "No"));
                        displayInfo.Add("HasItems: " + (storage.items.getItemCount() > 0 ? "Yes" : "No"));
                        //hasCrack = getLocked(storage);
                        int disp = getDisplay();
                        info_window.width = disp;
                        info_window.x = Screen.width - disp - 10;
                        inte = PlayerInteract.interactable;
                    }
                    else if (PlayerInteract.interactable is InteractableDoor)
                    {
                        InteractableDoor door = (InteractableDoor)PlayerInteract.interactable;
                        displayInfo.Add("Type: Door");
                        //if (door.owner != CSteamID.Nil)
                        //{
                            //displayInfo.Add("Owner: " + tool_ToolZ.getSteamPlayer(door.owner.m_SteamID).playerID.playerName);
                        //}
                        displayInfo.Add("Locked: " + (getLocked(door) ? "Yes" : "No"));
                        //hasCrack = getLocked(door);
                        int disp = getDisplay();
                        info_window.width = disp;
                        info_window.x = Screen.width - disp - 10;
                        inte = PlayerInteract.interactable;
                    }
                }
                else
                {
                    hasCrack = false;
                    RaycastHit hit;
                    if (tool_ToolZ.getLookingAt(out hit))
                    {
                        if (DamageTool.getPlayer(hit.transform) && DamageTool.getPlayer(hit.transform) != tool_ToolZ.getLocalPlayer())
                        {
                            Player p = DamageTool.getPlayer(hit.transform);
                            displayInfo.Add("Type: Player");
                            displayInfo.Add("Name: " + p.name);
                            displayInfo.Add("Health: " + p.life.health);
                            displayInfo.Add("Food: " + p.life.food);
                            displayInfo.Add("Water: " + p.life.water);
                            displayInfo.Add("Stamina: " + p.life.stamina);
                            displayInfo.Add("Distance: " + Math.Round(tool_ToolZ.getDistance(p.transform.position), 0));
                            if (tool_ToolZ.getLocalPlayer().equipment.asset != null && tool_ToolZ.getLocalPlayer().equipment.asset is ItemWeaponAsset)
                                displayInfo.Add("Will hit: " + (tool_ToolZ.getDistance(p.transform.position) <= ((ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset).range ? "Yes" : "No"));
                            int disp = getDisplay();
                            info_window.width = disp;
                            info_window.x = Screen.width - disp - 10;
                        }
                        else if (DamageTool.getZombie(hit.transform))
                        {
                            Zombie t = DamageTool.getZombie(hit.transform);
                            displayInfo.Add("Type: Zombie");
                            displayInfo.Add("Health: " + getHealth(t));
                            displayInfo.Add("Distance: " + Math.Round(tool_ToolZ.getDistance(t.transform.position), 0));
                            if (tool_ToolZ.getLocalPlayer().equipment.asset != null && tool_ToolZ.getLocalPlayer().equipment.asset is ItemWeaponAsset)
                                displayInfo.Add("Will hit: " + (tool_ToolZ.getDistance(t.transform.position) <= ((ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset).range ? "Yes" : "No"));
                            int disp = getDisplay();
                            info_window.width = disp;
                            info_window.x = Screen.width - disp - 10;
                        }
                        else if (DamageTool.getAnimal(hit.transform))
                        {
                            Animal t = DamageTool.getAnimal(hit.transform);
                            displayInfo.Add("Type: Animal");
                            displayInfo.Add("Health: " + getHealth(t));
                            displayInfo.Add("Distance: " + Math.Round(tool_ToolZ.getDistance(t.transform.position), 0));
                            if (tool_ToolZ.getLocalPlayer().equipment.asset != null && tool_ToolZ.getLocalPlayer().equipment.asset is ItemWeaponAsset)
                                displayInfo.Add("Will hit: " + (tool_ToolZ.getDistance(t.transform.position) <= ((ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset).range ? "Yes" : "No"));
                            int disp = getDisplay();
                            info_window.width = disp;
                            info_window.x = Screen.width - disp - 10;
                        }
                    }
                }
            }
        }

        public void OnGUI()
        {
            if (displayInfo.Count > 0 && enabled)
            {
                GUILayout.BeginArea(info_window);
                for (int i = 0; i < displayInfo.Count; i++)
                {
                    GUILayout.Label(displayInfo[i]);
                }
                if (hasCrack)
                {
                    if (GUILayout.Button("Crack/Unlock"))
                    {
                        setLocked(inte, false);
                    }
                }
                GUILayout.EndArea();
            }
        }

        private int getDisplay()
        {
            int display = int.MinValue;
            for (int i = 0; i < displayInfo.Count; i++)
            {
                if (displayInfo[i].Length > display)
                {
                    display = displayInfo[i].Length;
                }
            }
            if (hasCrack)
            {
                if (12 > display)
                {
                    display = 12;
                }
            }
            return display * 12;
        }

        private ushort getHealth(object obj)
        {
            return (ushort)obj.GetType().GetField("health", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        private bool getLocked(Interactable storage)
        {
            return (bool)storage.GetType().GetField("isLocked", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(storage);
        }

        private void setLocked(Interactable storage, bool locked)
        {
            storage.GetType().GetField("isLocked", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(storage, locked);
        }
    }
}
