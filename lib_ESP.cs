using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using HighlightingSystem;

namespace Payload
{
    public class lib_ESP : MonoBehaviour
    {
        private menu_ESP esp;
        private menu_ItemSelection itemsel;

        private DateTime lastTime;
        private DateTime lastRendTime;
        private DateTime lastFilterTime;

        private bool wasPlayer = false;
        public bool wasZombie = false;
        public bool wasItem = false;
        public bool wasAnimal = false;
        public bool wasCar = false;
        public bool wasStorage = false;

        private List<InteractableItem> itemFilter = new List<InteractableItem>();
        private List<NameDisplay> renderDisplay = new List<NameDisplay>();

        public lib_ESP(menu_ESP esp)
        {
            this.esp = esp;
            this.itemsel = ctrl_Connector.hack_ItemSelection;
        }

        private void DrawLabel(Vector3 point, string label, float fromY = 0f)
        {
            GUI.Label(new Rect(point.x, point.y + fromY, 500f, 24f), label);
        }

        private void update_Zombie() // -------------------------------- ZOMBIE UPDATE
        {
            if (esp.show_Zombies)
            {
                for (int i = 0; i < esp.zombies.Length; i++)
                {
                    if (esp.zombies[i] != null && esp.zombies[i].gameObject != null)
                    {
                        GameObject go = esp.zombies[i].gameObject;
                        if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                        {
                            Highlighter h = go.GetComponent<Highlighter>();
                            if (h == null)
                            {
                                h = go.AddComponent<Highlighter>();
                                h.OccluderOn();
                                h.SeeThroughOn();
                                h.ConstantOn(ctrl_Settings.esp_Zombie_color);
                                esp.ho.Add(new HighlightedObject(1, go, h));
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 1 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance);
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasZombie = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasZombie && !esp.show_Zombies)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 1);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasZombie = false;
            }
        }

        private void update_Player() // ------------------------------ PLAYER UPDATE
        {
            if (esp.show_Players)
            {
                for (int i = 0; i < esp.players.Length; i++)
                {
                    if (esp.players[i] != null && esp.players[i].player != null && esp.players[i].player.gameObject != null)
                    {
                        if (esp.players[i].player != tool_ToolZ.getLocalPlayer())
                        {
                            GameObject go = esp.players[i].player.gameObject;
                            if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                            {
                                Highlighter h = go.GetComponent<Highlighter>();
                                if (h == null)
                                {
                                    h = go.AddComponent<Highlighter>();
                                    h.OccluderOn();
                                    h.SeeThroughOn();
                                    if (ctrl_Connector.hack_Friends.isFriend(esp.players[i]))
                                    {
                                        h.ConstantOn(ctrl_Settings.esp_Friend_color);
                                    }
                                    else
                                    {
                                        h.ConstantOn(ctrl_Settings.esp_Player_color);
                                    }
                                    esp.ho.Add(new HighlightedObject(0, go, h));
                                }
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 0 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance);
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasPlayer = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasPlayer && !esp.show_Players)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 0);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasPlayer = false;
            }
        }

        private void update_Animals() // ------------------------ ANIMAL UPDATE
        {
            if (esp.show_Animals)
            {
                for (int i = 0; i < esp.animals.Length; i++)
                {
                    if (esp.animals[i] != null && esp.animals[i].gameObject != null)
                    {
                        GameObject go = esp.animals[i].gameObject;
                        if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                        {
                            Highlighter h = go.GetComponent<Highlighter>();
                            if (h == null)
                            {
                                h = go.AddComponent<Highlighter>();
                                h.OccluderOn();
                                h.SeeThroughOn();
                                h.ConstantOn(ctrl_Settings.esp_Animal_color);
                                esp.ho.Add(new HighlightedObject(4, go, h));
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 4 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance);
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasAnimal = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasAnimal && !esp.show_Animals)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 4);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasAnimal = false;
            }
        }

        private void update_Cars() // ----------------- CAR UPDATE
        {
            if (esp.show_Cars)
            {
                for (int i = 0; i < esp.vehicles.Length; i++)
                {
                    if (esp.vehicles[i] != null && esp.vehicles[i].gameObject != null && esp.vehicles[i].health > 0)
                    {
                        GameObject go = esp.vehicles[i].gameObject;
                        if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                        {
                            Highlighter h = go.GetComponent<Highlighter>();
                            if (h == null)
                            {
                                h = go.AddComponent<Highlighter>();
                                h.OccluderOn();
                                h.SeeThroughOn();
                                h.ConstantOn(ctrl_Settings.esp_Car_color);
                                esp.ho.Add(new HighlightedObject(2, go, h));
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 2 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance && (a.go.GetComponent<InteractableVehicle>() != null && a.go.GetComponent<InteractableVehicle>().health <= 0));
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasCar = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasCar && !esp.show_Cars)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 2);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasCar = false;
            }
        }

        private void update_Storages() // ---------- STORAGE UPDATE
        {
            if (esp.show_Storages)
            {
                for (int i = 0; i < esp.storages.Length; i++)
                {
                    if (esp.storages[i] != null && esp.storages[i].gameObject != null)
                    {
                        GameObject go = esp.storages[i].gameObject;
                        if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                        {
                            Highlighter h = go.GetComponent<Highlighter>();
                            if (h == null)
                            {
                                h = go.AddComponent<Highlighter>();
                                h.OccluderOn();
                                h.SeeThroughOn();
                                h.ConstantOn(ctrl_Settings.esp_Storage_color);
                                esp.ho.Add(new HighlightedObject(5, go, h));
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 5 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance);
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasStorage = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasStorage && !esp.show_Storages)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 5);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasStorage = false;
            }
        }

        private void update_ItemFilter() // ------------- ITEM FILTER UPDATE
        {
            if (esp.show_Items)
            {
                itemFilter.Clear();
                if (itemsel.guns || itemsel.melee || itemsel.ammo || itemsel.attachments || itemsel.throwable || itemsel.clothing || itemsel.bags || itemsel.medical || itemsel.foodnwater || itemsel.custom)
                {
                    for (int i = 0; i < esp.items.Length; i++)
                    {
                        if (esp.items[i] != null && esp.items[i].asset != null && esp.items[i].gameObject != null)
                        {
                            if (itemsel.guns && esp.items[i].asset is ItemGunAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.melee && esp.items[i].asset is ItemMeleeAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.ammo && esp.items[i].asset is ItemMagazineAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.attachments && (esp.items[i].asset is ItemBarrelAsset || esp.items[i].asset is ItemCaliberAsset || esp.items[i].asset is ItemGripAsset || esp.items[i].asset is ItemOpticAsset || esp.items[i].asset is ItemTacticalAsset))
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.throwable && esp.items[i].asset is ItemThrowableAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.clothing && (esp.items[i].asset is ItemHatAsset || esp.items[i].asset is ItemGlassesAsset || esp.items[i].asset is ItemMaskAsset || esp.items[i].asset is ItemShirtAsset || esp.items[i].asset is ItemPantsAsset || esp.items[i].asset is ItemClothingAsset))
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.bags && esp.items[i].asset is ItemBackpackAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.medical && esp.items[i].asset is ItemMedicalAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.foodnwater && esp.items[i].asset is ItemConsumeableAsset)
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                            else if (itemsel.custom && ctrl_Connector.hack_CustomItem.existsCustomItem(esp.items[i].item.id))
                            {
                                itemFilter.Add(esp.items[i]);
                            }
                        }
                    }
                }
            }
        }

        private void update_Items() // --------------- ITEM UPDATE
        {
            if (esp.show_Items)
            {
                for (int i = 0; i < itemFilter.Count; i++)
                {
                    if (itemFilter[i] != null && itemFilter[i].gameObject != null && itemFilter[i].asset != null)
                    {
                        GameObject go = itemFilter[i].gameObject;
                        if (tool_ToolZ.getDistance(go.transform.position) <= esp.distance)
                        {
                            Highlighter h = go.GetComponent<Highlighter>();
                            if (h == null)
                            {
                                h = go.AddComponent<Highlighter>();
                                h.OccluderOn();
                                h.SeeThroughOn();
                                h.ConstantOn(ctrl_Settings.esp_Item_color);
                                esp.ho.Add(new HighlightedObject(3, go, h));
                            }
                        }
                    }
                }

                try
                {
                    HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 3 && a != null && a.h != null && a.go != null && a.go.transform != null && tool_ToolZ.getDistance(a.go.transform.position) > esp.distance || (!Array.Exists(itemFilter.ToArray(), b => b.gameObject == a.go) && a.dType == 3));
                    if (objs.Length > 0)
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            UnityEngine.GameObject.Destroy(objs[i].h);
                            esp.ho.Remove(objs[i]);
                        }
                    }
                    wasItem = true;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("----------------- ERROR ----------------");
                    Debug.LogException(ex);
                    Debug.LogWarning("----------------- ERROR ----------------");
                }
            }
            else if (wasItem && !esp.show_Items)
            {
                HighlightedObject[] objs = Array.FindAll(esp.ho.ToArray(), a => a.dType == 3);
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i] != null && objs[i].h != null)
                    {
                        UnityEngine.GameObject.Destroy(objs[i].h);
                        esp.ho.Remove(objs[i]);
                    }
                }
                wasItem = false;
            }
        }

        public void update_Name_Display() // ------------- UPDATE GUI CALCS
        {
            renderDisplay.Clear();
            if (esp.players_Name)
            {
                for (int i = 0; i < esp.players.Length; i++)
                {
                    if (esp.players[i] != null && esp.players[i].player != null && esp.players[i].player.gameObject != null && esp.players[i].player != tool_ToolZ.getLocalPlayer())
                    {
                        GameObject g = esp.players[i].player.gameObject;
                        float dist = (float)Math.Round(tool_ToolZ.getDistance(g.transform.position), 0);
                        if (dist <= esp.distance)
                        {
                            Vector3 pos = Camera.main.WorldToScreenPoint(g.transform.position);
                            if (pos.z > 0 && pos.y < Screen.width - 2)
                            {
                                pos.y = (Screen.height - (pos.y + 1f)) - 12f;
                                pos.x = pos.x - 64f;

                                renderDisplay.Add(new NameDisplay(dist, g, pos, esp.players[i].playerID.playerName));
                            }
                        }
                    }
                }
            }

            if (esp.items_Name)
            {
                for (int i = 0; i < itemFilter.Count; i++)
                {
                    if (itemFilter[i] != null && itemFilter[i].gameObject != null && itemFilter[i].asset != null)
                    {
                        GameObject g = itemFilter[i].gameObject;
                        float dist = (float)Math.Round(tool_ToolZ.getDistance(g.transform.position), 0);
                        if (dist <= esp.distance)
                        {
                            Vector3 pos = Camera.main.WorldToScreenPoint(g.transform.position);
                            if (pos.z > 0 && pos.y < Screen.width - 2)
                            {
                                pos.y = Screen.height - (pos.y + 1f);
                                pos.x = pos.x - 32f;

                                renderDisplay.Add(new NameDisplay(dist, g, pos, itemFilter[i].asset.itemName));
                            }
                        }
                    }
                }
            }

            if (esp.cars_Name)
            {
                for (int i = 0; i < esp.vehicles.Length; i++)
                {
                    if (esp.vehicles[i] != null && esp.vehicles[i].gameObject != null && esp.vehicles[i].asset != null && esp.vehicles[i].health > 0)
                    {
                        GameObject g = esp.vehicles[i].gameObject;
                        float dist = (float)Math.Round(tool_ToolZ.getDistance(g.transform.position), 0);
                        if (dist <= esp.distance)
                        {
                            Vector3 pos = Camera.main.WorldToScreenPoint(g.transform.position);
                            if (pos.z > 0 && pos.y < Screen.width - 2)
                            {
                                pos.y = Screen.height - (pos.y + 1f);
                                pos.x = pos.x - 32f;

                                renderDisplay.Add(new NameDisplay(dist, g, pos, esp.vehicles[i].asset.vehicleName + " [" + esp.vehicles[i].fuel + "]"));
                            }
                        }
                    }
                }
            }
        }

        public void update() // ------------------------------------ UPDATE
        {
            if (lastTime == null || (DateTime.Now - lastTime).TotalMilliseconds >= 1000)
            {
                if (esp.enabled)
                {
                    update_Player();
                    update_Zombie();
                    update_Animals();
                    update_Storages();
                    update_Items();
                    update_Cars();
                }
                lastTime = DateTime.Now;
            }
            if (esp.enabled && (esp.players_Name || esp.items_Name || esp.cars_Name) && (lastRendTime == null || (DateTime.Now - lastRendTime).TotalMilliseconds >= esp.ref_Rate))
            {
                update_Name_Display();
                lastRendTime = DateTime.Now;
            }
            if (esp.enabled && esp.show_Items && (lastFilterTime == null || (DateTime.Now - lastFilterTime).TotalMilliseconds >= 1500))
            {
                update_ItemFilter();
                lastFilterTime = DateTime.Now;
            }
            if (!esp.enabled && esp.ho.Count > 0)
            {
                for (int i = 0; i < esp.ho.Count; i++)
                {
                    UnityEngine.GameObject.Destroy(esp.ho[i].h);
                    esp.ho.Remove(esp.ho[i]);
                }
            }
        }

        public void gui() // ------------------------ GUI
        {
            if (Event.current.type == EventType.Repaint)
            {
                if (esp.enabled && (esp.players_Name || esp.items_Name || esp.cars_Name))
                {
                    for (int i = 0; i < renderDisplay.Count; i++)
                    {
                        DrawLabel(renderDisplay[i].rPos, renderDisplay[i].name);
                        int mult = 1;
                        if (esp.get_Distance)
                        {
                            DrawLabel(renderDisplay[i].rPos, "Distance: " + renderDisplay[i].distance, 12f * mult);
                            mult++;
                        }
                    }
                }
            }
        }
    }
}
