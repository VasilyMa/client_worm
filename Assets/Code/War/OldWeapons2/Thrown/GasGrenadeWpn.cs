using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class GasGrenadeWpn : AbstractBazookaWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "газовая граната", desc => new GasGrenadeWpn (desc), () => _.War.Assets.GasGrenadeIcon
        );


        private GasGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.GasGrenadeWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new GasGrenade (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
