using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Friends : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 200);
        private Vector2 scroll;

        public bool sw = false;

        private List<Friend> friends = new List<Friend>();

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
            loadFriends();
        }

        public void Update()
        {

        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Friends, window_Main, onWindow, "Friends Menu");
            }
        }

        public void onWindow(int ID)
        {
            window_Main.width = 200;
            scroll = GUILayout.BeginScrollView(scroll);
            if (sw)
            {
                foreach (SteamPlayer sp in Provider.clients.ToArray())
                {
                    if (!isFriend(sp) && sp.player != tool_ToolZ.getLocalPlayer())
                    {
                        if (sp.playerID.playerName.Length * 12 > window_Main.width)
                        {
                            window_Main.width = sp.playerID.playerName.Length * 12;
                        }
                        if (GUILayout.Button(sp.playerID.playerName))
                        {
                            addFriend(sp);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    foreach (Friend f in friends)
                    {
                        if (f.displayName.Length * 12 > window_Main.width)
                        {
                            window_Main.width = f.displayName.Length * 12;
                        }
                        if (GUILayout.Button(f.displayName))
                        {
                            removeFriend(f);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lib_FileSystem.deleteFriends();
                    loadFriends();
                }
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        public void loadFriends()
        {
            if (lib_FileSystem.existFriends())
            {
                try
                {
                    friends = lib_FileSystem.readFriends();
                }
                catch (Exception ex)
                {
                    lib_FileSystem.deleteFriends();
                    loadFriends();
                }
            }
            else
            {
                lib_FileSystem.createFriends();
            }
        }

        public void saveFriends()
        {
            lib_FileSystem.writeFriends(friends.ToArray());
        }

        public void addFriend(SteamPlayer player)
        {
            friends.Add(new Friend(player.player.name, player.playerID.playerName, player.playerID.steamID.m_SteamID));
            saveFriends();
        }

        public bool isFriend(SteamPlayer player)
        {
            return Array.Exists(friends.ToArray(), a => a.ID == player.playerID.steamID.m_SteamID);
        }

        public bool isFriend(Player player)
        {
            return Array.Exists(friends.ToArray(), a => a.ID == tool_ToolZ.getPlayerID(player));
        }

        public void removeFriend(Friend f)
        {
            friends.Remove(f);
            saveFriends();
        }

        public Friend[] getFriends()
        {
            return friends.ToArray();
        }
    }
}
