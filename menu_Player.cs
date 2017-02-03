using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Player : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        private bool nightvision_military = false;
        private bool nightvision_civilian = false;
        private bool norain = false;
        private bool nosnow = false;

        private bool prev_night = false;

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
            if (norain)
            {
                LevelLighting.rainyness = ELightingRain.NONE;
            }
            if (nosnow)
            {
                LevelLighting.snowLevel = 0f;
                RenderSettings.fogDensity = 0f;
                typeof(LevelLighting).GetField("isSnow", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, false);
                typeof(LevelLighting).GetField("snownyess", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0f);
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Player, window_Main, onWindow, "Player Hack Menu");
            }

            if (Event.current.type == EventType.Repaint)
            {
                if (nightvision_military)
                {
                    LevelLighting.vision = ELightingVision.MILITARY;
                    LevelLighting.updateLighting();
                    LevelLighting.updateLocal();
                    PlayerLifeUI.updateGrayscale();
                    prev_night = true;
                }
                else if (nightvision_civilian)
                {
                    LevelLighting.vision = ELightingVision.CIVILIAN;
                    LevelLighting.updateLighting();
                    LevelLighting.updateLocal();
                    PlayerLifeUI.updateGrayscale();
                    prev_night = true;
                }
                else
                {
                    if (prev_night)
                    {
                        LevelLighting.vision = ELightingVision.NONE;
                        LevelLighting.updateLighting();
                        LevelLighting.updateLocal();
                        PlayerLifeUI.updateGrayscale();
                        prev_night = false;
                    }
                }
            }
        }

        public void onWindow(int ID)
        {
            nightvision_military = GUILayout.Toggle(nightvision_military, "Military Nightvision");
            nightvision_civilian = GUILayout.Toggle(nightvision_civilian, "Civilian Nightvision");
            norain = GUILayout.Toggle(norain, "No Rain");
            nosnow = GUILayout.Toggle(nosnow, "No Snow");
            if (GUILayout.Button("Drop all items"))
            {
                for (byte i = 0; i < PlayerInventory.PAGES - 1; i++)
                {
                    if (tool_ToolZ.getLocalPlayer().inventory.getItemCount(i) > 0)
                    {
                        for (byte a = 0; a < tool_ToolZ.getLocalPlayer().inventory.getHeight(i); a++)
                        {
                            for (byte b = 0; b < tool_ToolZ.getLocalPlayer().inventory.getWidth(i); b++)
                            {
                                tool_ToolZ.getLocalPlayer().inventory.sendDropItem(i, b, a);
                            }
                        }
                    }
                }
            }
            GUILayout.Label("Time: " + LightingManager.time);
            LightingManager.time = (uint)Math.Round(GUILayout.HorizontalSlider((float)LightingManager.time, (float)0u, (float)3600u));
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }
    }
}
