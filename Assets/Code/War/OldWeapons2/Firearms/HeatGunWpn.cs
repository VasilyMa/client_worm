using Core;


namespace War.OldWeapons2.Firearms {

    public class HeatGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "тепловой пистолет", desc => new HeatGunWpn (desc), () => _.War.Assets.HeatGunIcon
        );


        private HeatGunWpn (WeaponDescriptor desc) : base (desc) {}

    }

}