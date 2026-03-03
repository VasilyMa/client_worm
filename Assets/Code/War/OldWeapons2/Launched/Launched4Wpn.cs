using Core;


namespace War.OldWeapons2.Launched {

    public class Launched4Wpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пользователь не должен был увидеть этот текст",
            desc => new Launched4Wpn (desc),
            () => _.War.Assets.BazookaIcon
        );

        private Launched4Wpn (WeaponDescriptor desc) : base (desc) {}

    }

}
