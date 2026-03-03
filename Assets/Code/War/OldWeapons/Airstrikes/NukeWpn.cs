using Core;
using DataTransfer.Data;


namespace War.OldWeapons.Airstrikes {

    /**
     * Игрок указывает цель, и на следующий его ход туда прилетает ядерная ракета. Противники не видят метку.
     * Она наносит урон в огромном радиусе и облучает червяков радиацией в еще большем радиусе.
     */
    public class NukeWpn : Weapon {

        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("ядерная ракета", desc => new NukeWpn (desc), () => _.War.Assets.NukeIcon);


        public NukeWpn (WeaponDescriptor desc) : base (desc) {}

        
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