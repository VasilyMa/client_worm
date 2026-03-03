using Core;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Launched {

    /**
     * Выпускает несколько ракет очередью.
     */
    public class MultiLauncherWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "мини-ракеты",
            desc => new MultiLauncherWpn (desc),
            () => _.War.Assets.MiniRocketsIcon
        );


        public MultiLauncherWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new MiniRocket (), wormPosition, v * Balance.ThrowSpeed);
        }

    }

}