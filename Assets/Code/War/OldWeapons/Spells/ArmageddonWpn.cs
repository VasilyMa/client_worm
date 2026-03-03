using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Призывает множество метеоритов, которые бомбардируют карту.
     */
    public class ArmageddonWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("армагеддон", desc => new ArmageddonWpn (desc), () => _.War.Assets.ArmageddonIcon);


        public ArmageddonWpn (WeaponDescriptor desc) : base (desc) {}

        
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