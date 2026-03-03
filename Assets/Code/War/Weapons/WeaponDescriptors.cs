using System;
using System.Collections.Generic;


namespace War.Weapons {
    
    public static class WeaponDescriptors {

        private static List <WeaponDescriptor> _descriptors = new List <WeaponDescriptor> {null};
        public static IReadOnlyList <WeaponDescriptor> All => _descriptors;


        public static WeaponDescriptor Get (int id) => _descriptors [id];


        public static void Init () {
            Add (BazookaWpn        .Descriptor);
            Add (MiniRocketsWpn    .Descriptor);
            Add (CryoGunWpn        .Descriptor);

            Add (GrenadeWpn        .Descriptor);
            Add (LimonkaWpn        .Descriptor);
            Add (MolotovWpn        .Descriptor);
            Add (GasGrenadeWpn     .Descriptor);
            Add (GhostGrenadeWpn   .Descriptor);
            Add (HolyHandGrenadeWpn.Descriptor);

            Add (AssaultRifleWpn   .Descriptor);
            Add (BlasterWpn        .Descriptor);
            Add (PistolWpn         .Descriptor);
            Add (HeatGunWpn        .Descriptor);
            Add (CrossbowWpn       .Descriptor);
            Add (UltraRifleWpn     .Descriptor);

            Add (FingerWpn         .Descriptor);
            Add (CriticalStrikeWpn .Descriptor);
            Add (AxeWpn            .Descriptor);
            Add (GolfClubWpn       .Descriptor);
            Add (FlamethrowerWpn   .Descriptor);

            // мины/динамит/овцы

            // авиаудары
            
            Add (AirstrikeWpn   .Descriptor);
            Add (NapalmStrikeWpn.Descriptor);
            Add (PoisonStrikeWpn.Descriptor);

            // спеллы

            // транспорт

            // утилиты
        }


        private static void Add (WeaponDescriptor desc) {
            if (desc == null) throw new ArgumentException ();
            desc.Id = _descriptors.Count;
            _descriptors.Add (desc);
        }

    }

}