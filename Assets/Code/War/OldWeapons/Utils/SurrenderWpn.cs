using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Исключает из игры того, кто это использовал, но позволяет досмотреть ее.
     */
    public class SurrenderWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("белый флаг", desc => new SurrenderWpn (desc), () => _.War.Assets.SurrenderIcon);


        public SurrenderWpn (WeaponDescriptor desc) : base (desc) {}

        
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
