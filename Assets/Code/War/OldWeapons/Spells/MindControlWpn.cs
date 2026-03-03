using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Получает контроль над червяком на несколько секунд. Оружие им выбирать нельзя, только ползать.
     */
    public class MindControlWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("гипноз", desc => new MindControlWpn (desc), () => _.War.Assets.MindControlIcon);


        public MindControlWpn (WeaponDescriptor desc) : base (desc) {}

        
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