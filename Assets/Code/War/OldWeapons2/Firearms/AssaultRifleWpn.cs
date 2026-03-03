using Core;


namespace War.OldWeapons2.Firearms {

    public class AssaultRifleWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "автомат", desc => new AssaultRifleWpn (desc), () => _.War.Assets.AssaultRifleIcon
        );


        private AssaultRifleWpn (WeaponDescriptor desc) : base (desc) {}

    }

}
