using Core;
using Math;


namespace War.OldWeapons.Transport {

    /**
     * Позволяет сделать усиленный прыжок. Не тратит ход.
     */
    public class JumperWpn : VariablePowerWpn {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("прыгалка", desc => new JumperWpn (desc), () => _.War.Assets.JumperIcon);


        public JumperWpn (WeaponDescriptor desc) : base (desc) {}

        
        protected override void Shoot (XY wormPosition, XY v) {
            throw new System.NotImplementedException ();
        }

    }

}