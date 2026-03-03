using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Utils {

    /**
     * Пропускает ход. Если игрок взял удвоенный урон, то анимация будет быстрее.
     */
    public class SkipTurnWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("пропуск хода", desc => new SkipTurnWpn (desc), () => _.War.Assets.SkipTurnIcon);


        public SkipTurnWpn (WeaponDescriptor desc) : base (desc) {}

        
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