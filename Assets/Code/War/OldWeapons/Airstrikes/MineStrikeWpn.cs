using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасывается 5 мин, которые прилипают к земле.
     */
    public class MineStrikeWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("минный удар", desc => new MineStrikeWpn (desc), () => _.War.Assets.MineStrikeIcon);

        
        public MineStrikeWpn (WeaponDescriptor desc) : base (desc) {}

        
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