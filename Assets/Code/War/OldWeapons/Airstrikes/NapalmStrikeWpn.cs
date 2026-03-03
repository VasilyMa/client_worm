using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасывается горящий напалм. Он падает на землю и несколько ходов остается гореть.
     */
    public class NapalmStrikeWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "удар напалмом",
            desc => new NapalmStrikeWpn (desc),
            () => _.War.Assets.NapalmStrikeIcon
        );


        public NapalmStrikeWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            _.OldWar.World.LaunchAirstrike (() => new NapalmBomb (), _.OldWar.Camera.MouseXY, _.OldWar.Random.Bool (), 7);
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
