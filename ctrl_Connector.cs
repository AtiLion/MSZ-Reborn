using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class ctrl_Connector : MonoBehaviour
    {
        public static menu_Main hack_Main = null;
        public static menu_ESP hack_ESP = null;
        public static menu_Player hack_Player = null;
        public static menu_Fun hack_Fun = null;
        public static menu_Weapons hack_Weapons = null;
        public static menu_AimlockTriggerbot hack_AimlockTriggerbot = null;
        public static menu_Friends hack_Friends = null;
        public static lib_Skid skid = null;
        public static lib_Injection injection = null;
        public static menu_ItemSelection hack_ItemSelection = null;
        public static menu_Settings hack_Settings = null;
        public static menu_Vehicle hack_Vehicle = null;
        public static menu_Aimbot hack_Aimbot = null;
        public static lib_SmartUI smartUI = null;
        public static menu_ItemPickup hack_ItemPickup = null;
        public static menu_Debug hack_Debug = null;
        public static menu_Keybind hack_Keybind = null;
        public static menu_CustomItem hack_CustomItem = null;

        public static bool isDebug = false;
        public static bool isOn = false;
        public static bool exc = false;
        public static bool allow = false;
        public static string err = "";
        public static string version = "3.15.6.2";
        public static string firstTime = Application.persistentDataPath + @"\leave" + version + ".dat";
        public static string lk = Application.persistentDataPath + "/control.dat";
        public static bool hide = true;
        private static string premiumlist = "";
        private static string banlist = "";
        public static bool reset = false;
        private static bool uno = true;

        public static int id_Main = 0;
        public static int id_ESP = 1;
        public static int id_Player = 2;
        public static int id_Fun = 3;
        public static int id_Weapons = 4;
        public static int id_AimlockTriggerbot = 5;
        public static int id_Friends = 6;
        public static int id_ItemSelection = 7;
        public static int id_Settings = 8;
        public static int id_Vehicle = 9;
        public static int id_Aimbot = 10;
        public static int id_ItemPickup = 11;
        public static int id_Debug = 12;
        public static int id_Keybind = 13;
        public static int id_CustomItem = 14;

        private DateTime lastTime;
        private bool jUpdate = false;

        public static GameObject obj_Main = null;

        public ctrl_Connector()
        {
            WebClient wc = new WebClient();
            premiumlist = wc.DownloadString("{LINK REMOVED}");
            banlist = wc.DownloadString("{LINK REMOVED}");
            if (!isPremium(Provider.client.m_SteamID))
            {
                verify();
            }
            typeof(Provider).GetField("APP_NAME", BindingFlags.Static | BindingFlags.Public).SetValue(null, "MSZ Reborn by Manitou Real");
            typeof(Provider).GetField("APP_AUTHOR", BindingFlags.Static | BindingFlags.Public).SetValue(null, "#Final");
        }

        public static void verify()
        {
            Debug.Log("Verifying System32....");
            string hash = "";
            if (File.Exists(lk))
            {
                hash = File.ReadAllText(lk);
            }
            else
            {
                hash = BitConverter.ToString(Hash.SHA1(File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\Unturned_Data\Managed\UnityEngine.dll"))).ToLower();
            }
            Debug.Log("Getting hex: " + hash);
            if ((hash != "3b-07-5b-00-0a-d3-7d-20-10-eb-ed-9e-89-a4-65-ef-76-74-98-63" && hash != "63-1f-7d-03-8f-82-01-5a-79-b5-4e-bd-51-44-bc-fa-7b-d6-a1-42") || File.Exists(lk))
            {
                Debug.Log("System32 has been modified!");
                File.WriteAllText(lk, hash);
                Environment.Exit(0);
            }
            Debug.Log("System32 normal!");
        }

        public static bool isPremium(ulong cha)
        {
            return premiumlist.Contains(cha.ToString());
        }

        public static bool isBanned(ulong cha)
        {
            return banlist.Contains(cha.ToString());
        }

        public void onGUI()
        {
            if ((version == Provider.APP_VERSION || isPremium(Provider.client.m_SteamID)) && !isBanned(Provider.client.m_SteamID))
            {
                if (Provider.isConnected)
                {
                    if (uno)
                    {
                        if (!File.Exists(firstTime))
                        {
                            Provider.provider.browserService.open("{LINK REMOVED}");
                            File.WriteAllText(firstTime, "DO NOT DELETE");
                        }
                        uno = false;
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(10, 100, 1000, 20), "THIS VERSION IS NO LONGER SUPPORTED!");
            }
        }

        public void onUpdate()
        {
            if ((version == Provider.APP_VERSION || isPremium(Provider.client.m_SteamID)) && !isBanned(Provider.client.m_SteamID))
            {
                if (Provider.isConnected)
                {
                    if (lastTime == null || (DateTime.Now - lastTime).TotalMilliseconds >= 5000 || jUpdate)
                    {
                        if (obj_Main == null && hack_Main == null)
                        {
                            try
                            {
                                obj_Main = new GameObject();

                                hack_Main = obj_Main.AddComponent<menu_Main>();
                                hack_Settings = obj_Main.AddComponent<menu_Settings>();
                                skid = obj_Main.AddComponent<lib_Skid>();
                                hack_ESP = obj_Main.AddComponent<menu_ESP>();
                                hack_Player = obj_Main.AddComponent<menu_Player>();
                                hack_Fun = obj_Main.AddComponent<menu_Fun>();
                                hack_Weapons = obj_Main.AddComponent<menu_Weapons>();
                                hack_AimlockTriggerbot = obj_Main.AddComponent<menu_AimlockTriggerbot>();
                                hack_Friends = obj_Main.AddComponent<menu_Friends>();
                                injection = obj_Main.AddComponent<lib_Injection>();
                                hack_ItemSelection = obj_Main.AddComponent<menu_ItemSelection>();
                                hack_Vehicle = obj_Main.AddComponent<menu_Vehicle>();
                                hack_Aimbot = obj_Main.AddComponent<menu_Aimbot>();
                                smartUI = obj_Main.AddComponent<lib_SmartUI>();
                                hack_ItemPickup = obj_Main.AddComponent<menu_ItemPickup>();
                                hack_Debug = obj_Main.AddComponent<menu_Debug>();
                                hack_Keybind = obj_Main.AddComponent<menu_Keybind>();
                                hack_CustomItem = obj_Main.AddComponent<menu_CustomItem>();

                                DontDestroyOnLoad(hack_Main);
                                DontDestroyOnLoad(hack_Settings);
                                DontDestroyOnLoad(skid);
                                DontDestroyOnLoad(hack_ESP);
                                DontDestroyOnLoad(hack_Player);
                                DontDestroyOnLoad(hack_Fun);
                                DontDestroyOnLoad(hack_Weapons);
                                DontDestroyOnLoad(hack_AimlockTriggerbot);
                                DontDestroyOnLoad(hack_Friends);
                                DontDestroyOnLoad(injection);
                                DontDestroyOnLoad(hack_ItemSelection);
                                DontDestroyOnLoad(hack_Vehicle);
                                DontDestroyOnLoad(hack_Aimbot);
                                DontDestroyOnLoad(smartUI);
                                DontDestroyOnLoad(hack_ItemPickup);
                                DontDestroyOnLoad(hack_Debug);
                                DontDestroyOnLoad(hack_Keybind);
                                DontDestroyOnLoad(hack_CustomItem);
                            }
                            catch (Exception ex)
                            {
                                exc = true;
                                err = ex.Message;
                            }
                        }
                        lastTime = DateTime.Now;
                        jUpdate = false;
                    }
                }
                else
                {
                    if (reset)
                    {
                        UnityEngine.GameObject.Destroy(obj_Main);

                        obj_Main = null;

                        hack_Main = null;
                        hack_ESP = null;
                        hack_Player = null;
                        hack_Fun = null;
                        hack_Weapons = null;
                        hack_AimlockTriggerbot = null;
                        hack_Friends = null;
                        injection = null;
                        hack_ItemSelection = null;
                        hack_Vehicle = null;
                        hack_Aimbot = null;
                        smartUI = null;
                        skid = null;
                        hack_Settings = null;
                        hack_ItemPickup = null;
                        hack_Debug = null;
                        hack_Keybind = null;
                        hack_CustomItem = null;

                        jUpdate = true;
                    }
                }
            }
        }
    }
}
