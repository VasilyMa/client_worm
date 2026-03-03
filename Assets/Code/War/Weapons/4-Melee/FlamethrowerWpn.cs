using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Liquids;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class FlamethrowerWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "огнемет",
            desc => new FlamethrowerWpn (desc),
            () => _.War.Assets.FlamethrowerIcon
        );


        protected FlamethrowerWpn (WeaponDescriptor desc) : base (desc) {
            Shots        = 200;
            ShotCooldown = 2;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.FlamethrowerWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            // todo добавить разброс
            _.War.Spawn (new Flame (), Worm.Position, (td.XY - Worm.Position).WithLength (Balance.LiquidSpeed));
        }

    }

}