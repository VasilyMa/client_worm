using Core;
using DataTransfer.Data;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Граната, пролетающая сквозь любые препятствия.
     */
    public class GhostGrenadeWpn : VariablePowerWpn {

        private int _timer = 1;
        

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "призрачная граната",
            desc => new GhostGrenadeWpn (desc),
            () => _.War.Assets.GhostGrenadeIcon
        );


        public GhostGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new GhostGrenade (_timer), wormPosition, v * Balance.ThrowSpeed);
        }


        public override void Update (TurnData td) {
            if (td.NumKey != 0) _timer = td.NumKey;
            base.Update (td);
        }

    }

}
