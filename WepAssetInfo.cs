using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class WepAssetInfo
    {
        public float range;

        public byte[] hash;

        public WepAssetInfo(ItemMeleeAsset iwa)
        {
            range = iwa.range;

            hash = iwa.hash;
        }
    }
}
