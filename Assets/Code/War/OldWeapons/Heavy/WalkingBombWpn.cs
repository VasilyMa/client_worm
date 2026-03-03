using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Heavy {

    /**
     * Шагающая бомба, которая умеет лазить по любой поверхности и падать строго вертикально вниз.
     * Можно взорвать в любой момент.
     */
    public class WalkingBombWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("шагающая бомба", desc => new WalkingBombWpn (desc), () => _.War.Assets.WalkingBombIcon);


        public WalkingBombWpn (WeaponDescriptor desc) : base (desc) {}

        
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
