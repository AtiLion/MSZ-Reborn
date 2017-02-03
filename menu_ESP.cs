using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_ESP : MonoBehaviour
    {
        private KeyCode key;
        private bool isOn;
        private lib_ESP lib_ESP;

        // Components
        private Rect window_Main = new Rect(5, 5, 200, 10);

        public bool enabled = false;
        public float distance = 1000f;
        public int ref_Rate = 25;
        public bool show_Players = true;
        public bool players_Weapon = false;
        public bool players_Name = false;
        public bool show_Zombies = false;
        public bool show_Items = false;
        public bool items_Name = false;
        public bool show_Animals = false;
        public bool show_Cars = false;
        public bool cars_Name = false;
        public bool show_Storages = false;
        public bool get_Distance = false;

        public SteamPlayer[] players;
        public Zombie[] zombies;
        public Animal[] animals;
        public InteractableVehicle[] vehicles;
        public InteractableItem[] items;
        public InteractableStorage[] storages;
        private DateTime lastTime;
        public List<HighlightedObject> ho = new List<HighlightedObject>();

        public void toggleOn()
        {
            isOn = !isOn;
        }

        public void Start()
        {
            isOn = false;
            lib_ESP = new lib_ESP(this);
        }

        public void Update()
        {
            if (enabled)
            {
                if (lastTime == null || (DateTime.Now - lastTime).TotalMilliseconds >= 1500)
                {
                    players = Provider.clients.ToArray();
                    List<Zombie> temp = new List<Zombie>();
                    for (int i = 0; i < ZombieManager.regions.Length; i++)
                    {
                        temp.AddRange(ZombieManager.regions[i].zombies);
                    }
                    zombies = temp.ToArray();
                    animals = AnimalManager.animals.ToArray();
                    vehicles = VehicleManager.vehicles.ToArray();
                    items = UnityEngine.Object.FindObjectsOfType(typeof(InteractableItem)) as InteractableItem[];
                    storages = UnityEngine.Object.FindObjectsOfType(typeof(InteractableStorage)) as InteractableStorage[];
                    lastTime = DateTime.Now;
                }
            }
            lib_ESP.update();
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_ESP, window_Main, onWindow, "ESP Menu");
            }
            lib_ESP.gui();
        }

        public void onWindow(int ID)
        {
            enabled = GUILayout.Toggle(enabled, "Enabled ESP");
            show_Players = GUILayout.Toggle(show_Players, "Show Players");
            players_Name = GUILayout.Toggle(players_Name, "Get Player Names");
            show_Zombies = GUILayout.Toggle(show_Zombies, "Show Zombies");
            show_Animals = GUILayout.Toggle(show_Animals, "Show Animals");
            show_Cars = GUILayout.Toggle(show_Cars, "Show Vehicles");
            cars_Name = GUILayout.Toggle(cars_Name, "Get Vehicle Names");
            show_Items = GUILayout.Toggle(show_Items, "Show Items");
            items_Name = GUILayout.Toggle(items_Name, "Get Item Names");
            show_Storages = GUILayout.Toggle(show_Storages, "Show Storages");
            get_Distance = GUILayout.Toggle(get_Distance, "Get Distance");
            GUILayout.Label("Distance: " + distance);
            distance = GUILayout.HorizontalSlider((float)Math.Round(distance, 0), 0f, 4000f);
            GUILayout.Label("Refresh Rate(reverse lag): " + ref_Rate);
            ref_Rate = (int)GUILayout.HorizontalSlider((float)Math.Round((double)ref_Rate, 0), 0f, 50f);
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }
    }
}
