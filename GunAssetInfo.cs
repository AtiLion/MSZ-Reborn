using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SDG.Unturned;

namespace Payload
{
    public class GunAssetInfo
    {
        public float recoilMax_x;
        public float recoilMax_y;
        public float recoilMin_x;
        public float recoilMin_y;

        public float spreadAim;
        public float spreadHip;

        public byte _firerate;

        public byte[] hash;

        public GunAssetInfo(ItemGunAsset iga)
        {
            recoilMax_x = iga.recoilMax_x;
            recoilMax_y = iga.recoilMax_y;
            recoilMin_x = iga.recoilMin_x;
            recoilMin_y = iga.recoilMin_y;

            spreadAim = iga.spreadAim;
            spreadHip = iga.spreadHip;

            _firerate = iga.firerate;

            hash = iga.hash;
        }
    }
}
