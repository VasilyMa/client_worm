using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет летать, пока есть топливо. Не тратит ход.
     */
    public class JetpackWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("реактивный ранец", desc => new JetpackWpn (desc), () => _.War.Assets.JetpackIcon);


        public JetpackWpn (WeaponDescriptor desc) : base (desc) {}

        
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