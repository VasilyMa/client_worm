using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Будучи установленным, притягивает металлические вещи к себе.
     */
    public class MagnetWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("магнит", desc => new MagnetWpn (desc), () => _.War.Assets.MagnetIcon);


        public MagnetWpn (WeaponDescriptor desc) : base (desc) {}

        
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