using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class FlashbangWpn : AbstractGrenadeWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "светошумовая граната", desc => new FlashbangWpn (desc), () => _.War.Assets.FlashbangIcon
        );


        private FlashbangWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.FlashbangWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new Flashbang (Timer),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
