using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Позволяет лечить червяка, стоя к нему вплотную. Нельзя лечить самого себя.
     */
    public class MedikitWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("аптечка", desc => new MedikitWpn (desc), () => _.War.Assets.MedikitIcon);


        public MedikitWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            UseAmmo ();
//            var direction = (TurnData.XY - Entity.Position).WithLength (Balance.MeleeReach);
//            foreach (var w in _.OldWar.World.Worms) {
////                if (w != Entity && Entity.CastMelee (direction, w)) {
////                    w.Heal (Balance.HealingMedikit);
////                }
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