using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class CrossbowWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "арбалет",
            desc => new CrossbowWpn (desc),
            () => _.War.Assets.CrossbowIcon
        );


        protected CrossbowWpn (WeaponDescriptor desc) : base (desc) {
            Attacks            = 2;
            CancelableCooldown = 50;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.CrossbowWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (new PoisonArrow (Worm.Position, (td.XY - Worm.Position).WithLength (Balance.RayLength)));
        }

    }

}