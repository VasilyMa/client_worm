using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Удаляет землю в указанном месте. Не наносит урон.
     */
    public class ErosionWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("эрозия", desc => new ErosionWpn (desc), () => _.War.Assets.ErosionIcon);


        public ErosionWpn (WeaponDescriptor desc) : base (desc) {}


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