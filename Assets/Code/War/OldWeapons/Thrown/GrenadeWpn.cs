using Core;
using DataTransfer.Data;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Обычная граната с таймером.
     */
    public class GrenadeWpn : VariablePowerWpn {
        
        private int _timer = 5;
        

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("граната", desc => new GrenadeWpn (desc), () => _.War.Assets.GrenadeIcon);
        

        public GrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new Grenade (_timer), wormPosition, v * Balance.ThrowSpeed);
        }


        public override void Update (TurnData td) {
            if (td.NumKey != 0) _timer = td.NumKey;
            base.Update (td);
        }

    }

}
