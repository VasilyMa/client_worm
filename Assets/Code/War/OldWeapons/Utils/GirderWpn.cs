using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Устанавливает балку, которая становится частью ландшафта.
     */
    public class GirderWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("балка", desc => new GirderWpn (desc), () => _.War.Assets.GirderIcon);


        public GirderWpn (WeaponDescriptor desc) : base (desc) {}

        
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