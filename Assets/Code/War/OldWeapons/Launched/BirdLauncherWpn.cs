using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Launched {

    /**
     * Мощное оружие, но наносит урон самому стрелявшему.
     */
    public class BirdLauncherWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("птичкопушка", desc => new BirdLauncherWpn (desc), () => _.War.Assets.BirdLauncherIcon);


        public BirdLauncherWpn (WeaponDescriptor desc) : base (desc) {}
        
        
//        public override void OnShot () {
//            _.OldWar.World.Spawn (
//                new AngryBird (),
//                Entity.Position,
//                (_.OldWar.Camera.MouseXY - Entity.Position).WithLength (Balance.ThrowVelocity)
//            );
//        }

        public override void OnEquip () {
            throw new System.NotImplementedException ();
        }


        public override void OnDraw () {
            throw new System.NotImplementedException ();
        }


        public override void Update (TurnData td) {
            throw new System.NotImplementedException ();
        }


        public override void OnInterrupt () {
            throw new System.NotImplementedException ();
        }


        public override void OnUnequip () {
            throw new System.NotImplementedException ();
        }

    }

}