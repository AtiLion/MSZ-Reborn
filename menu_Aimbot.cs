using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Aimbot : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 200);

        private DateTime lastUpdate;
        private DateTime lastAim;

        private SteamPlayer[] players;
        private Zombie[] zombies;
        private Animal[] animals;

        public bool enabled = false;
        private bool aim_players = true;
        private bool ignore_friends = true;
        private bool ignore_admins = true;
        private bool ignore_walls = true;
        private bool use_gun_distance = true;
        private bool aim_zombies = false;
        private bool aim_animals = false;
        private float distance = 200f;
        private int aim_update = 20;
        private int at_pos = 0;

        private List<AimbotType> at = new List<AimbotType>();

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
            at.Add(new AimbotType("Head", "Skull"));
            at.Add(new AimbotType("Torso", "Spine"));
        }

        public void Update()
        {
            if (enabled)
            {
                if (lastUpdate == null || (DateTime.Now - lastUpdate).TotalMilliseconds >= 1000)
                {
                    update_Information();
                    lastUpdate = DateTime.Now;
                }
                if (lastAim == null || (DateTime.Now - lastAim).TotalMilliseconds >= aim_update)
                {
                    update_Aim();
                    lastAim = DateTime.Now;
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Aimbot, window_Main, onWindow, "Aimbot Menu");
            }
        }

        public void onWindow(int ID)
        {
            enabled = GUILayout.Toggle(enabled, "Aimbot");
            ignore_walls = GUILayout.Toggle(ignore_walls, "Ignore walls");
            use_gun_distance = GUILayout.Toggle(use_gun_distance, "Use gun distance");
            aim_players = GUILayout.Toggle(aim_players, "Attack Players");
            ignore_friends = GUILayout.Toggle(ignore_friends, "Ignore Friends");
            ignore_admins = GUILayout.Toggle(ignore_admins, "Ignore Admins");
            aim_zombies = GUILayout.Toggle(aim_zombies, "Attack Zombies");
            aim_animals = GUILayout.Toggle(aim_animals, "Attack Animals");
            if (GUILayout.Button("Aim: " + at[at_pos].name))
            {
                at_pos++;
                if (at_pos == at.Count)
                    at_pos = 0;
            }
            GUILayout.Label("Distance: " + distance);
            distance = GUILayout.HorizontalSlider((float)Math.Round(distance, 0), 0f, 1000f);
            GUILayout.Label("Aim Update(reverse lag): " + aim_update);
            aim_update = (int)GUILayout.HorizontalSlider((float)Math.Round((double)aim_update, 0), 0f, 50f);
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        private void update_Aim()
        {
            GameObject obj = null;

            if (aim_zombies)
            {
                Zombie z = getNZombie();
                if (z != null)
                {
                    if (obj == null)
                    {
                        obj = z.gameObject;
                    }
                    else
                    {
                        if (tool_ToolZ.getDistance(z.transform.position) < tool_ToolZ.getDistance(obj.transform.position))
                        {
                            obj = z.gameObject;
                        }
                    }
                }
            }

            if (aim_players)
            {
                Player p = getNPlayer();
                if (p != null)
                {
                    if (obj == null)
                    {
                        obj = p.gameObject;
                    }
                    else
                    {
                        if (tool_ToolZ.getDistance(p.transform.position) < tool_ToolZ.getDistance(obj.transform.position))
                        {
                            obj = p.gameObject;
                        }
                    }
                }
            }

            if (aim_animals)
            {
                Animal a = getNAnimal();
                if (a != null)
                {
                    if (obj == null)
                    {
                        obj = a.gameObject;
                    }
                    else
                    {
                        if (tool_ToolZ.getDistance(a.transform.position) < tool_ToolZ.getDistance(obj.transform.position))
                        {
                            obj = a.gameObject;
                        }
                    }
                }
            }

            if (obj != null)
            {
                aim(obj);
            }
        }

        private void aim(GameObject obj)
        {
            Vector3 skullPosition = getAimPosition(obj.transform);
            tool_ToolZ.getLocalPlayer().transform.LookAt(skullPosition);
            tool_ToolZ.getLocalPlayer().transform.eulerAngles = new Vector3(0f, tool_ToolZ.getLocalPlayer().transform.rotation.eulerAngles.y, 0f);
            Camera.main.transform.LookAt(skullPosition);
            float num4 = Camera.main.transform.localRotation.eulerAngles.x;
            if (num4 <= 90f && num4 <= 270f)
            {
                num4 = Camera.main.transform.localRotation.eulerAngles.x + 90f;
            }
            else if (num4 >= 270f && num4 <= 360f)
            {
                num4 = Camera.main.transform.localRotation.eulerAngles.x - 270f;
            }
            tool_ToolZ.getLocalPlayer().look.GetType().GetField("_pitch", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tool_ToolZ.getLocalPlayer().look, num4);
            tool_ToolZ.getLocalPlayer().look.GetType().GetField("_yaw", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tool_ToolZ.getLocalPlayer().look, tool_ToolZ.getLocalPlayer().transform.rotation.eulerAngles.y);
        }

        public Vector3 getAimPosition(Transform parent)
        {
            Transform[] componentsInChildren = parent.GetComponentsInChildren<Transform>();
            if (componentsInChildren != null)
            {
                Transform[] array = componentsInChildren;
                for (int i = 0; i < array.Length; i++)
                {
                    Transform tr = array[i];
                    if (tr.name.Trim() == at[at_pos].dmg)
                    {
                        return tr.position + new Vector3(0f, 0.4f, 0f);
                    }
                }
            }
            return Vector3.zero;
        }

        private void update_Information()
        {
            players = Provider.clients.ToArray();
            List<Zombie> temp = new List<Zombie>();
            for (int i = 0; i < ZombieManager.regions.Length; i++)
            {
                temp.AddRange(ZombieManager.regions[i].zombies);
            }
            zombies = temp.ToArray();
            animals = AnimalManager.animals.ToArray();
        }

        private Animal getNAnimal()
        {
            Animal a = null;
            for (int i = 0; i < animals.Length; i++)
            {
                if (!animals[i].isDead && correctDist(animals[i].transform.position))
                {
                    if (a == null)
                    {
                        if (ignore_walls)
                        {
                            a = animals[i];
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(animals[i].transform))
                            {
                                a = animals[i];
                            }
                        }
                    }
                    else
                    {
                        if (ignore_walls)
                        {
                            if (tool_ToolZ.getDistance(a.transform.position) > tool_ToolZ.getDistance(animals[i].transform.position))
                            {
                                a = animals[i];
                            }
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(animals[i].transform))
                            {
                                if (tool_ToolZ.getDistance(a.transform.position) > tool_ToolZ.getDistance(animals[i].transform.position))
                                {
                                    a = animals[i];
                                }
                            }
                        }
                    }
                }
            }
            return a;
        }

        private Zombie getNZombie()
        {
            Zombie z = null;
            for (int i = 0; i < zombies.Length; i++)
            {
                if (!zombies[i].isDead && correctDist(zombies[i].transform.position))
                {
                    if (z == null)
                    {
                        if (ignore_walls)
                        {
                            z = zombies[i];
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(zombies[i].transform))
                            {
                                z = zombies[i];
                            }
                        }
                    }
                    else
                    {
                        if (ignore_walls)
                        {
                            if (tool_ToolZ.getDistance(z.transform.position) > tool_ToolZ.getDistance(zombies[i].transform.position))
                            {
                                z = zombies[i];
                            }
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(zombies[i].transform))
                            {
                                if (tool_ToolZ.getDistance(z.transform.position) > tool_ToolZ.getDistance(zombies[i].transform.position))
                                {
                                    z = zombies[i];
                                }
                            }
                        }
                    }
                }
            }
            return z;
        }

        private bool canAttack(SteamPlayer p)
        {
            if (!ctrl_Connector.skid.isWhitelist(p.playerID.steamID.m_SteamID))
            {
                if (ignore_friends)
                {
                    if (!ctrl_Connector.hack_Friends.isFriend(p))
                    {
                        if (ignore_admins)
                        {
                            if (!p.isAdmin)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private bool correctDist(Vector3 pos)
        {
            if (use_gun_distance)
            {
                if (tool_ToolZ.getDistance(pos) <= ((ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset).range)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (tool_ToolZ.getDistance(pos) <= distance)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private Player getNPlayer()
        {
            Player p = null;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].playerID.steamID != Provider.client && players[i].player.life != null && !players[i].player.life.isDead && canAttack(players[i]) && correctDist(players[i].player.transform.position))
                {
                    if (p == null)
                    {
                        if (ignore_walls)
                        {
                            p = players[i].player;
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(players[i].player.transform))
                            {
                                p = players[i].player;
                            }
                        }
                    }
                    else
                    {
                        if (ignore_walls)
                        {
                            if (tool_ToolZ.getDistance(p.transform.position) > tool_ToolZ.getDistance(players[i].player.transform.position))
                            {
                                p = players[i].player;
                            }
                        }
                        else
                        {
                            if (tool_ToolZ.noWall(players[i].player.transform))
                            {
                                if (tool_ToolZ.getDistance(p.transform.position) > tool_ToolZ.getDistance(players[i].player.transform.position))
                                {
                                    p = players[i].player;
                                }
                            }
                        }
                    }
                }
            }
            return p;
        }
    }
}
