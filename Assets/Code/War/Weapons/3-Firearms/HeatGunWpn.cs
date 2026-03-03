using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Rays;
using War.Indication;


namespace War.Weapons {

    public class HeatGunWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "тепловой пистолет",
            desc => new HeatGunWpn (desc),
            () => _.War.Assets.HeatGunIcon
        );


        protected HeatGunWpn (WeaponDescriptor desc) : base (desc) {
            Attacks = 2;
            Warmup  = 30;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.HeatGunWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (new HeatRay (Worm.Position, (td.XY - Worm.Position).WithLength (Balance.RayLength)));
        }

    }

}