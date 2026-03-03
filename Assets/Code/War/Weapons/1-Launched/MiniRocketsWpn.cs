using Core;
using DataTransfer.Data;
using UnityEngine;
using Utils;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class MiniRocketsWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "мини-ракеты",
            desc => new MiniRocketsWpn (desc),
            () => _.War.Assets.MiniRocketsIcon
        );


        protected MiniRocketsWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
            Shots = 5;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.MiniRocketsWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (
                new MiniRocket (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90).Rotated (0.3f * _.War.Random.SignedFloat ())
            );
        }

    }

}