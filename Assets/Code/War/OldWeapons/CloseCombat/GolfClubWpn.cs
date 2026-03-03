using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Придает объекту очень сильный импульс в заданном направлении.
     */
    public class GolfClubWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("клюшка", desc => new GolfClubWpn (desc), () => _.War.Assets.GolfClubIcon);

        
        public GolfClubWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            UseAmmo ();
//            var direction = (TurnData.XY - Entity.Position).Normalized;
//            var targets = Entity.CastMelee (direction * Balance.MeleeReach);
//            foreach (var t in targets) {
////                t.TakeDamage (15);
////                t.TakeBlast (direction * Balance.PushExplosionLarge);
//            }
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