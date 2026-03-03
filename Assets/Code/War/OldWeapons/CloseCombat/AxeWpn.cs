using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Удар отнимает половину жизней червяка, но не менее 10 и не более 60. Округление в случайную сторону.
     */
    public class AxeWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("боевой топор", desc => new AxeWpn (desc), () => _.War.Assets.AxeIcon);


        public AxeWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            UseAmmo ();
//            var direction = (TurnData.XY - Entity.Position).Normalized;
//            var targets   = Entity.CastMelee (direction * Balance.MeleeReach);
//            foreach (var t in targets) {
////                t.TakeAxeDamage (0.5f, 10, 60);
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