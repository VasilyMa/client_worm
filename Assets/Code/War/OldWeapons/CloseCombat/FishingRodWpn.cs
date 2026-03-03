using Core;
using DataTransfer.Data;


namespace War.OldWeapons.CloseCombat {

    /**
     * Позволяет подтянуть объект к себе. Работает как веревка, на которой нельзя качаться влево-вправо.
     */
    public class FishingRodWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("удочка", desc => new FishingRodWpn (desc), () => _.War.Assets.FishingRodIcon);


        public FishingRodWpn (WeaponDescriptor desc) : base (desc) {}

        
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