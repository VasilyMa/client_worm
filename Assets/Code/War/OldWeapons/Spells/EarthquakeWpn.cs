using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Spells {

    /**
     * Сотрясает землю, заставляя объекты падать и кататься.
     */
    public class EarthquakeWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("землетрясение", desc => new EarthquakeWpn (desc), () => _.War.Assets.EarthquakeIcon);


        public EarthquakeWpn (WeaponDescriptor desc) : base (desc) {}

        
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