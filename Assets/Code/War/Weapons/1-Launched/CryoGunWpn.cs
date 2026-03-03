using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class CryoGunWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "криопушка",
            desc => new CryoGunWpn (desc),
            () => _.War.Assets.CryoGunIcon
        );

        protected CryoGunWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.BazookaWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (
                new CryoBall (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}