using Core;


namespace War.Weapons {

    public class MineGunWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "миномет",
            desc => new MineGunWpn (desc),
            () => _.War.Assets.MineGunIcon
        );

        protected MineGunWpn (WeaponDescriptor desc) : base (desc) {}

    }

}