using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет передать ход любому своему червяку.
     */
    public class WormSelectWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("выбор червяка", desc => new WormSelectWpn (desc), () => _.War.Assets.WormSelectIcon);


        public WormSelectWpn (WeaponDescriptor desc) : base (desc) {}

        
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