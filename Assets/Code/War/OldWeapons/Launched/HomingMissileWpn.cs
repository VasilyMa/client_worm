using Core;
using DataTransfer.Data;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Launched {

    /**
     * Выпускает ракету, которая потом летит к указанной цели. Если не может достичь, падает.
     */
    public class HomingMissileWpn : VariablePowerWpn {

        private XY _target;
        

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "самонаводящаяся ракета",
            desc => new HomingMissileWpn (desc),
            () => _.War.Assets.HomingMissileIcon
        );


        public HomingMissileWpn (WeaponDescriptor desc) : base (desc) {}


        public override void Update (TurnData td) {
            
        }


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new HomingMissile (_target, null), wormPosition, v);
        }

    }

}
