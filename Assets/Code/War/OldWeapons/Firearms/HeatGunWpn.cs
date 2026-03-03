using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Стреляет 2 раза за ход. В точке попадания создаются огоньки.
     */
    public class HeatGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("тепловой пистолет", desc => new HeatGunWpn (desc), () => _.War.Assets.HeatGunIcon);


        public HeatGunWpn (WeaponDescriptor desc) : base (desc) {}

        
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