using System.Collections.Generic;
using War.OldWeapons2.Firearms;
using War.OldWeapons2.Launched;
using War.OldWeapons2.Thrown;


namespace War.OldWeapons2 {

    public static class WeaponDescriptors {

        private static List <WeaponDescriptor> _descriptors = new List <WeaponDescriptor> {null};
        public static IReadOnlyList <WeaponDescriptor> All => _descriptors;


        public static WeaponDescriptor Get (int id) => _descriptors [id];


        public static void Init () {
            Add (BazookaWpn.Descriptor);
            Add (MinegunWpn.Descriptor);
            Add (HomingMissileWpn.Descriptor);
            Add (Launched4Wpn.Descriptor);
            Add (MiniRocketsWpn.Descriptor);
            Add (CryogunWpn.Descriptor);
            Add (BirdLauncherWpn.Descriptor);
            
            Add (GrenadeWpn.Descriptor);
            Add (LimonkaWpn.Descriptor);
            Add (MolotovWpn.Descriptor);
            Add (GasGrenadeWpn.Descriptor);
            Add (GhostGrenadeWpn.Descriptor);
            Add (FlashbangWpn.Descriptor);
            Add (HolyHandGrenadeWpn.Descriptor);
            
            Add (AssaultRifleWpn.Descriptor);
            Add (BlasterWpn.Descriptor);
            Add (PistolWpn.Descriptor);
            Add (CrossbowWpn.Descriptor);
            Add (HeatGunWpn.Descriptor);
            Add (UltraRifleWpn.Descriptor);
            Add (GsomRaycasterWpn.Descriptor);
        }


        private static void Add (WeaponDescriptor desc) {
            if (desc == null) return;
            desc.Id = _descriptors.Count;
            _descriptors.Add (desc);
        }

    }

}
