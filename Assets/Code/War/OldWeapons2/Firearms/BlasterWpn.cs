using Core;


namespace War.OldWeapons2.Firearms {

    public class BlasterWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "бластер", desc => new BlasterWpn (desc), () => _.War.Assets.BlasterIcon
        );


        private BlasterWpn (WeaponDescriptor desc) : base (desc) {}}

}