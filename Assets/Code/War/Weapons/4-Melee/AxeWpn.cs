using System.Collections.Generic;
using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities;
using War.Indication;
using War.Systems.Blasts;
using War.Systems.Damage;


namespace War.Weapons {

    public class AxeWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "боевой топор",
            desc => new AxeWpn (desc),
            () => _.War.Assets.AxeIcon
        );
        

        protected AxeWpn (WeaponDescriptor desc) : base (desc) {
            Warmup = 10;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.AxeWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();

            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            var v1 = (td.XY - Worm.Position).Normalized;
            
            var entities = new HashSet <MobileEntity> {Worm};
            _.War.CollisionSystem.FindObstaclesMeleeRay (Worm.Colliders, v1 * Balance.MeleeReach, entities);
            entities.Remove (Worm);

            foreach (var e in entities) {
                (e as IDamageable)?.TakeAxeDamage ();
                (e as IBlastable )?.TakeBlast  (Balance.PushMeleeWeak * v1);
            }
        }
    }

}