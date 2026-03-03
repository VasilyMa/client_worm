using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Роет землю в указанном направлении. Наносит урон объектам и толкает их.
     */
    public class DrillWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("сверло", desc => new DrillWpn (desc), () => _.War.Assets.DrillIcon);


        public DrillWpn (WeaponDescriptor desc) : base (desc) {}

        
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