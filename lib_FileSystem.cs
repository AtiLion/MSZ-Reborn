using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Payload
{
    public class lib_FileSystem
    {
        private static string cDir = Directory.GetCurrentDirectory();
        private static string sDir = Application.persistentDataPath;
        private static string file_Settings = sDir + @"\Settings.dat";
        private static string file_Friends = sDir + @"\Friends.dat";
        private static string file_Keybinds = sDir + @"\Keybinds.dat";
        private static string file_CustomItem = sDir + @"\CustomItem.dat";

        public static void writeSettings(Setting[] lines)
        {
            File.WriteAllText(file_Settings, JsonConvert.SerializeObject(lines));
        }

        public static List<Setting> readSettings()
        {
            return JsonConvert.DeserializeObject<List<Setting>>(File.ReadAllText(file_Settings));
        }

        public static bool existSettings()
        {
            return File.Exists(file_Settings);
        }

        public static void deleteSettings()
        {
            File.Delete(file_Settings);
        }

        public static void writeCustomItem(ushort[] lines)
        {
            File.WriteAllText(file_CustomItem, JsonConvert.SerializeObject(lines));
        }

        public static List<ushort> readCustomItem()
        {
            return JsonConvert.DeserializeObject<List<ushort>>(File.ReadAllText(file_CustomItem));
        }

        public static bool existCustomItem()
        {
            return File.Exists(file_CustomItem);
        }

        public static void deleteCustomItem()
        {
            File.Delete(file_CustomItem);
        }

        public static void createCustomItem()
        {
            File.WriteAllText(file_CustomItem, "[]");
        }

        public static void writeKeybinds(Keybind[] lines)
        {
            File.WriteAllText(file_Keybinds, JsonConvert.SerializeObject(lines));
        }

        public static List<Keybind> readKeybinds()
        {
            return JsonConvert.DeserializeObject<List<Keybind>>(File.ReadAllText(file_Keybinds));
        }

        public static bool existKeybinds()
        {
            return File.Exists(file_Keybinds);
        }

        public static void deleteKeybinds()
        {
            File.Delete(file_Keybinds);
        }

        public static void writeFriends(Friend[] lines)
        {
            string json = JsonConvert.SerializeObject(lines);
            File.WriteAllText(file_Friends, json);
        }

        public static List<Friend> readFriends()
        {
            return JsonConvert.DeserializeObject<List<Friend>>(File.ReadAllText(file_Friends));
        }

        public static bool existFriends()
        {
            return File.Exists(file_Friends);
        }

        public static void createFriends()
        {
            File.WriteAllText(file_Friends, "[]");
        }

        public static void deleteFriends()
        {
            File.Delete(file_Friends);
        }
    }
}
