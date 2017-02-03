using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class PayloadProxy : MonoBehaviour
    {
        public static GameObject hook_obj = null;

        private static Payload instance = null;

        public static void Launch()
        {
            try
            {
                hook_obj = new GameObject();

                instance = hook_obj.AddComponent<Payload>();

                DontDestroyOnLoad(instance);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("----- CRASH -----");
                Debug.LogException(ex);
                Debug.LogWarning("------ END ------");
            }
        }
    }
}
