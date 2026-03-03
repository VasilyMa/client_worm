using Core;


namespace War.OldWeapons2.Firearms {

    public class CrossbowWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "арбалет", desc => new CrossbowWpn (desc), () => _.War.Assets.CrossbowIcon
        );


        private CrossbowWpn (WeaponDescriptor desc) : base (desc) {}

    }

}