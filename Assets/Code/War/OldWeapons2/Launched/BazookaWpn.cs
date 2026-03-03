using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Launched {

    public class BazookaWpn : AbstractBazookaWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "базука", desc => new BazookaWpn (desc), () => _.War.Assets.BazookaIcon
        );


        private BazookaWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.BazookaWeapon;


        protected override void Shoot (TurnData td, float power) 
        { 
            _.War.Spawn (
                new BazookaShell (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
