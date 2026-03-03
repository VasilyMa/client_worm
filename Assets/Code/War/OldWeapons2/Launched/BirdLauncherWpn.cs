using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;


namespace War.OldWeapons2.Launched {

    public class BirdLauncherWpn : AbstractAimedWeapon {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "птичкопушка", desc => new BirdLauncherWpn (desc), () => _.War.Assets.BirdLauncherIcon
        );


        private BirdLauncherWpn (WeaponDescriptor desc) : base (desc) {}


        protected override GameObject WeaponPrefab => _.War.Assets.BirdLauncherWeapon;


        protected override void Shoot (TurnData td) {
            _.War.Spawn (new AngryBird (), Worm.Position, (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed));
        }

    }

}
