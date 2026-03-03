using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Лягушка перемещается только прыжками и умеет плавать. Можно взорвать в любой момент.
     */
    public class FrogWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("лягушка", desc => new FrogWpn (desc), () => _.War.Assets.FrogIcon);


        public FrogWpn (WeaponDescriptor desc) : base (desc) {}

        
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