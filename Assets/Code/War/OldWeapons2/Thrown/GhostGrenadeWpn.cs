using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class GhostGrenadeWpn : AbstractGrenadeWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "граната", desc => new GhostGrenadeWpn (desc), () => _.War.Assets.GhostGrenadeIcon
        );


        private GhostGrenadeWpn (WeaponDescriptor desc) : base (desc) {
            Timer = 1;
        }


        protected override GameObject WeaponPrefab => _.War.Assets.GhostGrenadeWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new GhostGrenade (Timer),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
