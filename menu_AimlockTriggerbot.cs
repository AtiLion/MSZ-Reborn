using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_AimlockTriggerbot : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        public bool aimlock = false;
        public bool triggerbot = false;
        private bool animals = false;
        private bool players = true;
        private bool zombies = false;
        private bool nofriends = true;
        private bool noadmins = true;
        private bool nodistance = false;

        private float aimlock_sensitivity = 1f;

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
        }

        public void Update()
        {
            
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_AimlockTriggerbot, window_Main, onWindow, "Aimlock/Triggerbot");
            }

            if (aimlock || triggerbot && tool_ToolZ.getLocalPlayer().equipment.asset is ItemWeaponAsset)
            {
                RaycastHit hit;
                if (tool_ToolZ.getLookingAt(out hit, (nodistance ? Mathf.Infinity : ((ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset).range)))
                {
                    if (players && DamageTool.getPlayer(hit.transform) && DamageTool.getPlayer(hit.transform) != tool_ToolZ.getLocalPlayer() && !ctrl_Connector.skid.isWhitelist(tool_ToolZ.getPlayerID(DamageTool.getPlayer(hit.transform))))
                    {
                        if (nofriends && !ctrl_Connector.hack_Friends.isFriend(DamageTool.getPlayer(hit.transform)))
                        {
                            if (noadmins && !tool_ToolZ.getSteamPlayer(DamageTool.getPlayer(hit.transform)).isAdmin)
                            {
                                useAttack();
                            }
                            else
                            {
                                if (!noadmins)
                                {
                                    useAttack();
                                }
                            }
                        }
                        else
                        {
                            if (!nofriends)
                            {
                                useAttack();
                            }
                        }
                    }
                    else if (zombies && DamageTool.getZombie(hit.transform))
                    {
                        useAttack();
                    }
                    else if (animals && DamageTool.getAnimal(hit.transform))
                    {
                        useAttack();
                    }
                    else
                    {
                        useReset();
                    }
                }
                else
                {
                    useReset();
                }
            }
            else
            {
                useReset();
            }
        }

        public void onWindow(int ID)
        {
            aimlock = GUILayout.Toggle(aimlock, "Aimlock");
            triggerbot = GUILayout.Toggle(triggerbot, "Triggerbot");
            nofriends = GUILayout.Toggle(nofriends, "Ignore friends");
            noadmins = GUILayout.Toggle(noadmins, "Ignore admins");
            nodistance = GUILayout.Toggle(nodistance, "Ignore distance");
            players = GUILayout.Toggle(players, "Lock players");
            zombies = GUILayout.Toggle(zombies, "Lock zombies");
            animals = GUILayout.Toggle(animals, "Lock animals");
            GUILayout.Label("Aimlock sensitivity: " + aimlock_sensitivity.ToString());
            aimlock_sensitivity = (float)Math.Round(GUILayout.HorizontalSlider(aimlock_sensitivity, 1f, 10f), 0);
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        private void useAttack()
        {
            if (triggerbot)
                attack(true);
            if (aimlock)
            {
                if (tool_ToolZ.getLocalPlayer().equipment.useable is Useable && ((UseableGun)tool_ToolZ.getLocalPlayer().equipment.useable).isAiming)
                    tool_ToolZ.getLocalPlayer().look.sensitivity = (getZoom((UseableGun)tool_ToolZ.getLocalPlayer().equipment.useable) / 45f) - (aimlock_sensitivity / 10);
                else
                    tool_ToolZ.getLocalPlayer().look.sensitivity = (aimlock_sensitivity / 10);
            }
        }

        private void useReset()
        {
            if (triggerbot)
                attack(false);
            if (aimlock)
            {
                if (tool_ToolZ.getLocalPlayer().equipment.useable is Useable && ((UseableGun)tool_ToolZ.getLocalPlayer().equipment.useable).isAiming)
                    tool_ToolZ.getLocalPlayer().look.sensitivity = getZoom((UseableGun)tool_ToolZ.getLocalPlayer().equipment.useable) / 45f;
                else
                    tool_ToolZ.getLocalPlayer().look.sensitivity = 1f;
            }
        }

        private void attack(bool att)
        {
            tool_ToolZ.getLocalPlayer().equipment.GetType().GetField("prim", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tool_ToolZ.getLocalPlayer().equipment, att);
        }

        private float getZoom(UseableGun gun)
        {
            return (float)gun.GetType().GetField("zoom", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gun);
        }
    }
}
