using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Слабо толкает червяка, не нанося урона.
     */
    public class FingerWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("толчок", desc => new FingerWpn (desc), () => _.War.Assets.FingerIcon);


        public FingerWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            UseAmmo ();
//            var direction = (TurnData.XY - Entity.Position).Normalized;
//            var targets   = Entity.CastMelee (direction * Balance.MeleeReach);
//            foreach (var t in targets) {
////                t.TakeBlast (direction * Balance.PushExplosionSmall);
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