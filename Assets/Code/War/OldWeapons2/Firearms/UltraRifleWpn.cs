using Core;


namespace War.OldWeapons2.Firearms {

    public class UltraRifleWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "ультравинтовка", desc => new UltraRifleWpn (desc), () => _.War.Assets.UltraRifleIcon
        );


        private UltraRifleWpn (WeaponDescriptor desc) : base (desc) {}

    }

}