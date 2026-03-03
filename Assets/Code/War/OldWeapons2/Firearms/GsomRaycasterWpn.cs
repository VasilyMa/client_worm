using Core;


namespace War.OldWeapons2.Firearms {

    public class GsomRaycasterWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пыщ-лучемет", desc => new GsomRaycasterWpn (desc), () => _.War.Assets.GsomRaycasterIcon
        );


        private GsomRaycasterWpn (WeaponDescriptor desc) : base (desc) {}

    }

}