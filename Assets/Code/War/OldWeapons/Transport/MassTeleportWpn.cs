using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Телепортирует всех червей на карте в случайные точки.
     */
    public class MassTeleportWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "общий телепорт",
            desc => new MassTeleportWpn (desc),
            () => _.War.Assets.MassTeleportIcon
        );


        public MassTeleportWpn (WeaponDescriptor desc) : base (desc) {}

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
