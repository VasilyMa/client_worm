using Core;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Мощная граната, взрывающаяся при полной остановке.
     */
    public class HolyHandGrenadeWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "святая граната",
            desc => new HolyHandGrenadeWpn (desc),
            () => _.War.Assets.HolyHandGrenadeIcon
        );


        public HolyHandGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


//        protected override bool ConstPower => false;
//        
//        
//        public override void OnShot () {
//            _.OldWar.World.Spawn (
//                new HolyHandGrenade (),
//                Entity.Position,
//                (_.OldWar.Camera.MouseXY - Entity.Position).WithLength (Balance.ThrowVelocity * Power01)
//            );
//        }

        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new HolyHandGrenade (), wormPosition, v * Balance.ThrowSpeed);
        }

    }

}
