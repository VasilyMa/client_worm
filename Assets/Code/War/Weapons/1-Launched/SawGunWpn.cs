using Core;


namespace War.Weapons {

    public class SawGunWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пиломет",
            desc => new SawGunWpn (desc),
            () => _.War.Assets.SawGunIcon
        );

        protected SawGunWpn (WeaponDescriptor desc) : base (desc) {}

    }

}