using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Firearms {

    /**
     * Выстреливает длинной очередью пар пыщ-лучей, один из которых прицельный, другой случайный.
     * Пыщ-лучи наносят 9999 урона, но если у цели больше хп или оно бесконечное, то она все равно умирает.
     * Не наносит урон своим, а лечит их и снимает дебаффы.
     * Отравляет всех врагов на карте, приклеивает их и оглушает.
     * Если в настройках указана неразрушаемая земля, лучи ее пробивают.
     * Объекты, взорванные пыщ-лучом, взрываются таким же эффектом.
     * Взорванные ящики собираются в арсенал игрока.
     * Ящик, в котором находится пыщ-лучемет, нельзя уничтожить.
     */
    public class GsomRaycasterWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пыщ-лучемет",
            desc => new GsomRaycasterWpn (desc),
            () => _.War.Assets.GsomRaycasterIcon
        );


        public GsomRaycasterWpn (WeaponDescriptor desc) : base (desc) {}

        
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
