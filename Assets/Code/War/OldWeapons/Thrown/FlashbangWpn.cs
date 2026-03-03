using Core;
using DataTransfer.Data;
using Math;
using War.Entities.Projectiles;


namespace War.OldWeapons.Thrown {

    /**
     * Наносит урон тем, кто посмотрел на взрыв, и оглушает их. Оглушение меняет местами кнопки управления.
     * Эффект пропадает, если сделать ход этим червяком, или со временем.
     */
    public class FlashbangWpn : VariablePowerWpn {

        private int _timer = 5;

        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "светошумовая граната",
            desc => new FlashbangWpn (desc),
            () => _.War.Assets.FlashbangIcon
        );


        public FlashbangWpn (WeaponDescriptor desc) : base (desc) {}


        protected override void Shoot (XY wormPosition, XY v) {
            _.War.Spawn (new Flashbang (_timer), wormPosition, v * Balance.ThrowSpeed);
        }


        public override void Update (TurnData td) {
            if (td.NumKey != 0) _timer = td.NumKey;
            base.Update (td);
        }

    }

}
