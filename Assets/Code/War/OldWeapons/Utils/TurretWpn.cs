using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Устанавливаемая турель, реагирующая на движение противника в определенном радиусе.
     */
    public class TurretWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("турель", desc => new TurretWpn (desc), () => _.War.Assets.TurretIcon);


        public TurretWpn (WeaponDescriptor desc) : base (desc) {}

        
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