using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасывается 7 бомб.
     */
    public class AirstrikeWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("авиаудар", desc => new AirstrikeWpn (desc), () => _.War.Assets.AirstrikeIcon);


        public AirstrikeWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            _.OldWar.World.LaunchAirstrike (() => new Bomb (), _.OldWar.Camera.MouseXY, _.OldWar.Random.Bool (), 7);
//        }

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