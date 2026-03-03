using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Launched {

    public class CryogunWpn : AbstractBazookaWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "криопушка", desc => new CryogunWpn (desc), () => _.War.Assets.CryoGunIcon
        );


        private CryogunWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.CryogunWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new CryoBall (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
