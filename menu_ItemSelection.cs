using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class menu_ItemSelection : MonoBehaviour
    {
        private bool isOn;

        private Rect window_Main = new Rect(10, 10, 200, 10);

        public bool clothing = false;
        public bool guns = false;
        public bool melee = false;
        public bool ammo = false;
        public bool foodnwater = false;
        public bool attachments = false;
        public bool bags = false;
        public bool throwable = false;
        public bool other = false;
        public bool medical = false;
        public bool custom = false;

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
        }

        public void OnGUI()
        {
            if (isOn && ctrl_Connector.isOn)
            {
                window_Main = GUILayout.Window(ctrl_Connector.id_ItemSelection, window_Main, onWindow, "Item Selection Menu");
            }
        }

        public void onWindow(int ID)
        {
            guns = GUILayout.Toggle(guns, "Guns");
            melee = GUILayout.Toggle(melee, "Melee");
            ammo = GUILayout.Toggle(ammo, "Ammo");
            attachments = GUILayout.Toggle(attachments, "Attachments");
            throwable = GUILayout.Toggle(throwable, "Throwables");
            clothing = GUILayout.Toggle(clothing, "Clothing");
            bags = GUILayout.Toggle(bags, "Backpacks");
            medical = GUILayout.Toggle(medical, "Medical");
            foodnwater = GUILayout.Toggle(foodnwater, "Food and Water");
            custom = GUILayout.Toggle(custom, "Custom");
            if (GUILayout.Button("Custom Editor"))
            {
                ctrl_Connector.hack_CustomItem.toggleOn();
            }
            if (GUILayout.Button("Close Menu"))
            {
                toggleOn();
            }
            GUI.DragWindow();
        }
    }
}
