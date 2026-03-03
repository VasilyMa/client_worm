using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Выпускает направленную очередь огоньков.
     */
    public class FlamethrowerWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("огнемет", desc => new FlamethrowerWpn (desc), () => _.War.Assets.FlamethrowerIcon);


//        public FlamethrowerWpn (WeaponDescriptor desc) : base (desc) {
//            Shots        = 40;
//            ShotCooldown = 4;
//        }
//        
//        
//        public override void OnShot () {
//            UseAmmo ();
//            _.OldWar.World.Spawn (
//                new Fire (),
//                Entity.Position,
//                (_.OldWar.Camera.MouseXY - Entity.Position).WithLength (Balance.LiquidVelocity)
//            );
//        }

        public FlamethrowerWpn (WeaponDescriptor desc) : base (desc) {}

        
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