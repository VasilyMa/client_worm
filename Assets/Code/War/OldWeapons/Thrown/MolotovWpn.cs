using Core;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Бутылка, при ударе разбивающаяся и разбрызгивающая огоньки.
     */
    public class MolotovWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("коктейль Молотова", desc => new MolotovWpn (desc), () => _.War.Assets.MolotovIcon);


        public MolotovWpn (WeaponDescriptor desc) : base (desc) {}
        

        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new MolotovBottle (), wormPosition, v * Balance.ThrowSpeed);
        }

    }

}