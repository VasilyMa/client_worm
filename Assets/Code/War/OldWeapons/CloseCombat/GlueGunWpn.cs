using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Стреляет приклеивающими каплями. Если они попадут в червяка или он в них наступит, он не сможет ходить.
     * Капли высыхают через некоторое время. Эффект пропадает, если червяк телепортируется, под ним разрушается
     * земля или его сносит взрывом.
     */
    public class GlueGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("суперклей", desc => new GlueGunWpn (desc), () => _.War.Assets.GlueGunIcon);


//        public GlueGunWpn (WeaponDescriptor desc) : base (desc) {
//            Shots        = 30;
//            ShotCooldown = 4;
//        }
//        
//        
//        public override void OnShot () {
//            UseAmmo ();
//            var entity = new LiquidStub ();
//            _.OldWar.World.Spawn (
//                entity,
//                Entity.Position,
//                (_.OldWar.Camera.MouseXY - Entity.Position).WithLength (Balance.LiquidVelocity)
//            );
//        }

        public GlueGunWpn (WeaponDescriptor desc) : base (desc) {}

        
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