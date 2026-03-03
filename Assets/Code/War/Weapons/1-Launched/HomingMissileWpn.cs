using Core;


namespace War.Weapons {

    public class HomingMissileWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "самонаводящаяся ракета",
            desc => new HomingMissileWpn (desc),
            () => _.War.Assets.HomingMissileIcon
        );

        protected HomingMissileWpn (WeaponDescriptor desc) : base (desc) {}

    }

}