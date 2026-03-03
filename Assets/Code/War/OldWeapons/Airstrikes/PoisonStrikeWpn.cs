using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасываются бомбы, при ударе выпускающие яд. Возможно, что при полете тоже.
     */
    public class PoisonStrikeWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "отравляющий удар",
            desc => new PoisonStrikeWpn (desc),
            () => _.War.Assets.PoisonStrikeIcon
        );


        public PoisonStrikeWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            _.OldWar.World.LaunchAirstrike (() => new PoisonBomb (), _.OldWar.Camera.MouseXY, _.OldWar.Random.Bool (), 7);
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
