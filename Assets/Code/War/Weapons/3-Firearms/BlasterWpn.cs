using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class BlasterWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "бластер",
            desc => new BlasterWpn (desc),
            () => _.War.Assets.BlasterIcon
        );


        protected BlasterWpn (WeaponDescriptor desc) : base (desc) {
            Attacks = 2;
            Warmup = 30;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.BlasterWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (new BlasterBullet (Worm.Position, (td.XY - Worm.Position).WithLength (Balance.RayLength)));
        }

    }

}