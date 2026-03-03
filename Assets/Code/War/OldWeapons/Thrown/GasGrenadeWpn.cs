using Core;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Граната, выпускающая ядовитый газ в полете и при взрыве.
     */
    public class GasGrenadeWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("газовая граната", desc => new GasGrenadeWpn (desc), () => _.War.Assets.GasGrenadeIcon);


        public GasGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


//        protected override bool ConstPower => false;
//        
//        
//        public override void OnShot () {
//            _.OldWar.World.Spawn (
//                new GasGrenade (),
//                Entity.Position,
//                (_.OldWar.Camera.MouseXY - Entity.Position).WithLength (Balance.ThrowVelocity * Power01)
//            );
//        }

        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new GasGrenade (), wormPosition, v * Balance.ThrowSpeed);
        }

    }

}