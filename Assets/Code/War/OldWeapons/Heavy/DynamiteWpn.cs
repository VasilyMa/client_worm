using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Мощная устанавливаемая взрывчатка с таймером.
     */
    public class DynamiteWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("динамит", desc => new DynamiteWpn (desc), () => _.War.Assets.DynamiteIcon);


        public DynamiteWpn (WeaponDescriptor desc) : base (desc) {}

        
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