using Core;
using DataTransfer.Data;
using UnityEngine;
using Utils;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class UltraRifleWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "ультравинтовка",
            desc => new UltraRifleWpn (desc),
            () => _.War.Assets.UltraRifleIcon
        );


        protected UltraRifleWpn (WeaponDescriptor desc) : base (desc) {
            Shots        = 15;
            ShotCooldown = 10;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.UltraRifleWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent<LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (new UltraWave (
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.RayLength).Rotated (0.1f * _.War.Random.SignedFloat ())
            ));
        }

    }

}