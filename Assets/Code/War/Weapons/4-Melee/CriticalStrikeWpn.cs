using Core;
using DataTransfer.Data;
using static Utils.Time;


namespace War.Weapons {

    
    // активируется в прыжке, урон зависит от того с какой высоты ты спрыгнул
    public class CriticalStrikeWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "критический удар",
            desc => new CriticalStrikeWpn (desc),
            () => _.War.Assets.SkipTurnIcon
        );


        protected CriticalStrikeWpn (WeaponDescriptor desc) : base (desc) {}


        // очень просто: если мы нажали кнопку и мы в прыжке, активируем на червяке контроллер
        // если нет, ничего не делаем
        public override void Update (TurnData td) {
            if (td.MB && Worm.UseCriticalStrike ()) {
                UseAmmo();
                _.War.EndTurn (Seconds (3));
            }
        }

    }

}