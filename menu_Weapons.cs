using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using System.Reflection;

namespace Payload
{
    public class menu_Weapons : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        private bool norecoil = true;
        private bool nospread = true;
        private bool rapidfire = false;
        private float new_range = 0f;

        private List<GunAssetInfo> backups_Asset = new List<GunAssetInfo>();

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

        private bool isBackupAsset(ItemGunAsset asset)
        {
            return Array.Exists(backups_Asset.ToArray(), a => a.hash == asset.hash);
        }

        private GunAssetInfo getGunAsset(ItemGunAsset asset)
        {
            return Array.Find(backups_Asset.ToArray(), a => a.hash == asset.hash);
        }

        public void Update()
        {
            if (tool_ToolZ.getLocalPlayer().equipment.asset is ItemGunAsset)
            {
                ItemGunAsset gun = (ItemGunAsset)tool_ToolZ.getLocalPlayer().equipment.asset;
                if (nospread)
                {
                    if (!isBackupAsset(gun))
                    {
                        backups_Asset.Add(new GunAssetInfo(gun));
                    }
                    gun.GetType().GetField("_spreadAim", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                    gun.GetType().GetField("_spreadHip", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                }
                if (norecoil)
                {
                    if (!isBackupAsset(gun))
                    {
                        backups_Asset.Add(new GunAssetInfo(gun));
                    }
                    gun.GetType().GetField("_recoilMax_x", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                    gun.GetType().GetField("_recoilMax_y", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                    gun.GetType().GetField("_recoilMin_x", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                    gun.GetType().GetField("_recoilMin_y", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, 0f);
                }

                if (!nospread)
                {
                    if (isBackupAsset(gun))
                    {
                        GunAssetInfo back = getGunAsset(gun);
                        gun.GetType().GetField("_recoilMax_x", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.recoilMax_x);
                        gun.GetType().GetField("_recoilMax_y", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.recoilMax_y);
                        gun.GetType().GetField("_recoilMin_x", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.recoilMin_x);
                        gun.GetType().GetField("_recoilMin_y", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.recoilMin_y);
                    }
                }
                if (!norecoil)
                {
                    if (isBackupAsset(gun))
                    {
                        GunAssetInfo back = getGunAsset(gun);
                        gun.GetType().GetField("_spreadAim", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.spreadAim);
                        gun.GetType().GetField("_spreadHip", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, back.spreadHip);
                    }
                }
            }
            if (tool_ToolZ.getLocalPlayer().equipment.useable is UseableGun)
            {
                UseableGun gun = (UseableGun)tool_ToolZ.getLocalPlayer().equipment.useable;
                if (rapidfire)
                {
                    gun.GetType().GetField("firemode", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(gun, EFiremode.AUTO);
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Weapons, window_Main, onWindow, "Weapons Hack Menu");
            }
        }

        public void onWindow(int ID)
        {
            norecoil = GUILayout.Toggle(norecoil, "NoRecoil");
            nospread = GUILayout.Toggle(nospread, "NoSpread");
            rapidfire = GUILayout.Toggle(rapidfire, "RapidFire");
            if (tool_ToolZ.getLocalPlayer().equipment.asset != null && tool_ToolZ.getLocalPlayer().equipment.asset is ItemWeaponAsset && tool_ToolZ.getLocalPlayer().equipment.asset is ItemMeleeAsset)
            {
                ItemWeaponAsset ima = (ItemWeaponAsset)tool_ToolZ.getLocalPlayer().equipment.asset;
                new_range = ima.range;
                GUILayout.Label("Reach: " + ima.range);
                new_range = (float)Math.Round(GUILayout.HorizontalSlider(new_range, 1f, 26f), 0);
                updateFarReach();
            }
            if (GUILayout.Button("Close Menu"))
            {
                isOn = false;
            }
            GUI.DragWindow();
        }

        private void updateFarReach()
        {
            typeof(ItemWeaponAsset).GetField("_range", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tool_ToolZ.getLocalPlayer().equipment.asset, new_range);
        }
    }
}
