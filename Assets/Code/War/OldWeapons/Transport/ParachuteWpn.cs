using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет безопасно спускаться с высоты. Можно настроить автоматическое срабатывание. Не тратит ход.
     */
    public class ParachuteWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("парашют", desc => new ParachuteWpn (desc), () => _.War.Assets.ParachuteIcon);


        public ParachuteWpn (WeaponDescriptor desc) : base (desc) {}

        
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