using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class BazookaWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "базука",
            desc => new BazookaWpn (desc),
            () => _.War.Assets.BazookaIcon
        );


        protected BazookaWpn (WeaponDescriptor desc) : base (desc) {
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
            base.OnShot (td);
            _.War.Spawn (
                new BazookaShell (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}
