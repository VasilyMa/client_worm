using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Мина, при установке прилипающая к земле, если ее отбросит взрывом, она теряет это свойство.
     * Таймер активируется при появлении червяка поблизости.
     */
    public class MineWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("мина", desc => new MineWpn (desc), () => _.War.Assets.MineIcon);


        public MineWpn (WeaponDescriptor desc) : base (desc) {}

        
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