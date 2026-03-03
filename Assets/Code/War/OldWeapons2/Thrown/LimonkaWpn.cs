using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class LimonkaWpn : AbstractGrenadeWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "лимонка", desc => new LimonkaWpn (desc), () => _.War.Assets.LimonkaIcon
        );


        private LimonkaWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.LimonkaWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new Limonka (Timer),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
