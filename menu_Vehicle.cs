using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_Vehicle : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        private InteractableVehicle car;
        private EEngine startEngine = EEngine.HELICOPTER;
        private bool car_load = false;

        private bool fly = false;
        private float car_speed = 0f;

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

        public void Update()
        {
            car = tool_ToolZ.getLocalPlayer().movement.getVehicle();
            if (car == null)
            {
                fly = false;
                car_load = false;
            }
            else
            {
                if (!car_load)
                {
                    startEngine = car.asset.engine;
                    car_load = true;
                }
                if (Input.GetKeyDown(ControlsSettings.interact))
                {
                    VehicleManager.exitVehicle();
                }
            }
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_Vehicle, window_Main, onWindow, "Vehicle Hack Menu");
            }
        }

        public void onWindow(int ID)
        {
            if (!car_load)
            {
                GUILayout.Label("Please enter a vehicle!");
            }
            else
            {
                if (ctrl_Connector.isPremium(Provider.client.m_SteamID))
                {
                    if (GUILayout.Button((!fly ? "Enable" : "Disable") + " flight"))
                    {
                        fly = !fly;
                        if (car.asset.engine == startEngine)
                        {
                            setEngine(EEngine.PLANE);
                            setLockMouse(true);
                            car.GetComponent<Rigidbody>().useGravity = false;
                        }
                        else
                        {
                            setEngine(startEngine);
                            setLockMouse(false);
                            car.GetComponent<Rigidbody>().useGravity = true;
                        }
                    }
                }
                car_speed = car.asset.speedMax;
                GUILayout.Label("Max speed: " + car.asset.speedMax);
                car_speed = (float)Math.Round(GUILayout.HorizontalSlider(car_speed, 1f, 18f), 1);
                updateMaxSpeed();
            }
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }

        private void setEngine(EEngine engine)
        {
            VehicleAsset asset = car.asset;
            asset.GetType().GetField("_engine", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(asset, engine);
        }

        private void setLockMouse(bool lockMouse)
        {
            VehicleAsset asset = car.asset;
            asset.GetType().GetField("_hasLockMouse", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(asset, lockMouse);
        }

        private void updateMaxSpeed()
        {
            VehicleAsset asset = car.asset;
            asset.GetType().GetField("_speedMax", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).SetValue(asset, car_speed);
        }
    }
}
