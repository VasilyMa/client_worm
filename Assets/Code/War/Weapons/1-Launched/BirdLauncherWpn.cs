using Core;


namespace War.Weapons {

    public class BirdLauncherWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "птичкопушка",
            desc => new BirdLauncherWpn (desc),
            () => _.War.Assets.BirdLauncherIcon
        );


        private BirdLauncherWpn (WeaponDescriptor desc) : base (desc) {}

    }

}