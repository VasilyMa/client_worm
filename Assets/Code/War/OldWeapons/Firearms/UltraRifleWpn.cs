using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Выстреливает неточную очередь, проходящую сквозь землю и наносящую урон червякам. Не сталкивает их.
     */
    public class UltraRifleWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("ультравинтовка", desc => new UltraRifleWpn (desc), () => _.War.Assets.UltraRifleIcon);


        public UltraRifleWpn (WeaponDescriptor desc) : base (desc) {}
//            Shots = 20;
//        }
//
//
//        public override void OnShot () {
//            var origin = Entity.Position + Worm.HeadOffset;
//            var offset =
//            (TurnData.XY - origin).WithLength (10000).Rotated ((_.OldWar.Random.Float () - _.OldWar.Random.Float ()) * 0.1f);
//            var targets = _.OldWar.World.CastUltraRay (origin, offset);
//            foreach (var target in targets) target.TakeDamage (1);
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
