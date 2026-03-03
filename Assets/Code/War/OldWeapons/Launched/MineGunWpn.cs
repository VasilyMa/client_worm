using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Launched {

    /**
     * Стреляет миной-прилипалкой на большое расстояние.
     */
    public class MineGunWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("миномет", desc => new MineGunWpn (desc), () => _.War.Assets.MineGunIcon);


        public MineGunWpn (WeaponDescriptor desc) : base (desc) {}

        
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