using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class GrenadeWpn : AbstractGrenadeWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "граната", desc => new GrenadeWpn (desc), () => _.War.Assets.GrenadeIcon
        );


        private GrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.GrenadeWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new Grenade (Timer),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
