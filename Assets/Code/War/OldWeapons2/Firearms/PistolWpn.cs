using Core;


namespace War.OldWeapons2.Firearms {

    public class PistolWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пистолет", desc => new PistolWpn (desc), () => _.War.Assets.PistolIcon
        );


        private PistolWpn (WeaponDescriptor desc) : base (desc) {}}

}