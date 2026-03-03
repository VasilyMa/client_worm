using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Thrown {

    public class MolotovWpn : AbstractBazookaWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "коктейль Молотова", desc => new MolotovWpn (desc), () => _.War.Assets.MolotovIcon
        );


        private MolotovWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.MolotovWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new MolotovBottle (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

    
    public class HolyHandGrenadeWpn : AbstractBazookaWpn {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "святая граната", desc => new HolyHandGrenadeWpn (desc), () => _.War.Assets.HolyHandGrenadeIcon
        );


        private HolyHandGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.HolyHandGrenadeWeapon;


        protected override void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new HolyHandGrenade (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
