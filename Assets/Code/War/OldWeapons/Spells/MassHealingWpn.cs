using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Лечит всех червяков на карте.
     */
    public class MassHealingWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "общее исцеление",
            desc => new MassHealingWpn (desc),
            () => _.War.Assets.MassHealingIcon
        );


        public MassHealingWpn (WeaponDescriptor desc) : base (desc) {}
        
        
//        public override void OnShot () {
//            for (int i = 0; i < _.OldWar.World.Worms.Count; i++) {
//                var worm = _.OldWar.World.Worms [i];
//                if (worm.Despawned) continue;
//                worm.Heal (30);
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
