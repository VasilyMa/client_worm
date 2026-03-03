using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * С самолета сбрасывается бомба, которая при ударе притягивает к себе объекты, затем взрывается.
     */
    public class VacuumBombWpn : GenericAirstrikeWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("вакуумная бомба", desc => new VacuumBombWpn (desc), () => _.War.Assets.VacuumBombIcon);


        public VacuumBombWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnShot () {
//            _.OldWar.World.LaunchAirstrike (() => new VacuumBomb (), _.OldWar.Camera.MouseXY, _.OldWar.Random.Bool ()); // 2
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