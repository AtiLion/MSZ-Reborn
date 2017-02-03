using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class lib_Injection : MonoBehaviour
    {
        private DateTime lastTime;

        public void Start()
        {
        }

        public void Update()
        {
            if (Math.Round(tool_ToolZ.getLocalPlayer().look.yaw, 0) == 360f || Math.Round(tool_ToolZ.getLocalPlayer().look.yaw, 0) == -360f)
            {
                tool_ToolZ.getLocalPlayer().look.simulate(0f, tool_ToolZ.getLocalPlayer().look.pitch, 0f);
            }
            if (lastTime == null || (DateTime.Now - lastTime).TotalMilliseconds >= 1000)
            {
                if (Provider.isPvP && (bool)ctrl_Connector.hack_Settings.getSetting("enable_instantDisconnect").value && ctrl_Connector.allow && !ctrl_Connector.hack_Main.banned_hack)
                {
                    typeof(PlayerPauseUI).GetField("TIMER_LEAVE", BindingFlags.Static | BindingFlags.Public).SetValue(null, 0f);
                }
                else
                {
                    typeof(PlayerPauseUI).GetField("TIMER_LEAVE", BindingFlags.Static | BindingFlags.Public).SetValue(null, 10f);
                }
            }
        }

        public void OnGUI()
        {

        }
    }
}
