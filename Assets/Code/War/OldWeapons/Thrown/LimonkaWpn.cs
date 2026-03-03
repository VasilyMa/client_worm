using Core;
using DataTransfer.Data;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Граната, при взрыве выпускающая осколки. Осколки взрываются при ударе.
     */
    public class LimonkaWpn : VariablePowerWpn {

        private int _timer = 5;

        
        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("лимонка", desc => new LimonkaWpn (desc), () => _.War.Assets.LimonkaIcon);

        
        public LimonkaWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new Limonka (_timer), wormPosition, v * Balance.ThrowSpeed);
        }


        public override void Update (TurnData td) {
            if (td.NumKey != 0) _timer = td.NumKey;
            base.Update (td);
        }

    }

}