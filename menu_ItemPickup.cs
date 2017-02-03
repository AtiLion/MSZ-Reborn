using System;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;
using UnityEngine;

namespace Payload
{
    public class menu_ItemPickup : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        public bool itemPickup = false;
        private bool ignoreInvalidAmmo = true;
        private bool ignoreDropped = true;
        private int checkTime = 1000;
        private bool ignoreWalls = true;

        private DateTime lastCheck;
        private menu_ItemSelection itemsel;
        private List<ItemAsset> dropped = new List<ItemAsset>();

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
            itemsel = ctrl_Connector.hack_ItemSelection;
        }

        public void Update()
        {
            if (itemPickup)
            {
                if (lastCheck == null || (DateTime.Now - lastCheck).TotalMilliseconds >= checkTime)
                {
                    InteractableItem[] items = getItems();
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i].use();
                    }
                    lastCheck = DateTime.Now;
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_ItemPickup, window_Main, onWindow, "Auto Item Pickup Menu");
            }
        }

        public void onWindow(int ID)
        {
            itemPickup = GUILayout.Toggle(itemPickup, "Auto Item Pickup");
            //ignoreInvalidAmmo = GUILayout.Toggle(ignoreInvalidAmmo, "Ignore Invalid Ammo");
            //ignoreDropped = GUILayout.Toggle(ignoreDropped, "Ignore Dropped Items");
            ignoreWalls = GUILayout.Toggle(ignoreWalls, "Pickup Through Walls");
            GUILayout.Label("Item check time(miliseconds): " + checkTime);
            checkTime = (int)Math.Round(GUILayout.HorizontalSlider((float)checkTime, 1f, 2000f));
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        private InteractableItem[] getItems()
        {
            Collider[] array = Physics.OverlapSphere(tool_ToolZ.getLocalPlayer().look.aim.position, 10f, RayMasks.ITEM);
            List<InteractableItem> items = new List<InteractableItem>();

            if (itemsel.guns || itemsel.melee || itemsel.ammo || itemsel.attachments || itemsel.throwable || itemsel.clothing || itemsel.bags || itemsel.medical || itemsel.foodnwater || itemsel.custom)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != null && array[i].gameObject != null)
                    {
                        InteractableItem item = array[i].GetComponent<InteractableItem>();
                        if (item != null && tool_ToolZ.noWall(item.transform))
                        {
                            if (itemsel.guns && item.asset is ItemGunAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.melee && item.asset is ItemMeleeAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.ammo && item.asset is ItemMagazineAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.attachments && (item.asset is ItemBarrelAsset || item.asset is ItemCaliberAsset || item.asset is ItemGripAsset || item.asset is ItemOpticAsset || item.asset is ItemTacticalAsset))
                            {
                                items.Add(item);
                            }
                            else if (itemsel.throwable && item.asset is ItemThrowableAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.clothing && (item.asset is ItemHatAsset || item.asset is ItemGlassesAsset || item.asset is ItemMaskAsset || item.asset is ItemShirtAsset || item.asset is ItemPantsAsset || item.asset is ItemClothingAsset))
                            {
                                items.Add(item);
                            }
                            else if (itemsel.bags && item.asset is ItemBackpackAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.medical && item.asset is ItemMedicalAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.foodnwater && item.asset is ItemConsumeableAsset)
                            {
                                items.Add(item);
                            }
                            else if (itemsel.custom && ctrl_Connector.hack_CustomItem.existsCustomItem(item.item.id))
                            {
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            return items.ToArray();
        }

        /*private bool checkAmmo(InteractableItem item)
        {
            
        }*/

        /*private bool checkDropped(InteractableItem item)
        {

        }*/
    }
}
