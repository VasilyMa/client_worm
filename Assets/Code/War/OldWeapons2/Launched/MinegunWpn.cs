using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities;


namespace War.OldWeapons2.Launched {

    public class MinegunWpn : AbstractAimedWeapon {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "миномет", desc => new MinegunWpn (desc), () => _.War.Assets.MineGunIcon
        );

        private MinegunWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.MinegunWeapon;


        protected override void Shoot (TurnData td) {
            _.War.Spawn (new Mine (), Worm.Position, (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed));
        }

    }

}
