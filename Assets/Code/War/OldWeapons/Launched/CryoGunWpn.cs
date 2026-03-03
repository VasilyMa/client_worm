using Core;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Launched {

    /**
     * Выпускает снаряд, при попадании создающий ледяной шар. Шар становится частью ландшафта.
     */
    public class CryoGunWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("криопушка", desc => new CryoGunWpn (desc), () => _.War.Assets.CryoGunIcon);


        public CryoGunWpn (WeaponDescriptor desc) : base (desc) {}

        
        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new CryoBall (), wormPosition, v * Balance.ThrowSpeed);
        }

    }

}