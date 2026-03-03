using Core;
using DataTransfer.Data;
using UnityEngine;
using Utils;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class AssaultRifleWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "автомат",
            desc => new AssaultRifleWpn (desc),
            () => _.War.Assets.AssaultRifleIcon
        );


        protected AssaultRifleWpn (WeaponDescriptor desc) : base (desc) {
            Shots        = 30;
            ShotCooldown = 6;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.AssaultRifleWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            // todo добавить разброс
            _.War.Spawn (new Bullet (
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.RayLength).Rotated (0.2f * _.War.Random.SignedFloat ())
            ));
        }

    }

}
