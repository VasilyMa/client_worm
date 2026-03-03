using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасываются 5 кротов. Кроты роют землю пока не встретят препятствие.
     * Игрок не может управлять ими.
     */
    public class MoleStrikeWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("кротовый удар", desc => new MoleStrikeWpn (desc), () => _.War.Assets.MoleStrikeIcon);


        public MoleStrikeWpn (WeaponDescriptor desc) : base (desc) {}

        
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
