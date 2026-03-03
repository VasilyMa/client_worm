using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class PistolWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "пистолет",
            desc => new PistolWpn (desc),
            () => _.War.Assets.PistolIcon
        );


        protected PistolWpn (WeaponDescriptor desc) : base (desc) {
            Shots        = 5;
            ShotCooldown = 30;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.PistolWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (new PistolBullet (Worm.Position, (td.XY - Worm.Position).WithLength (Balance.RayLength)));
        }

    }

}