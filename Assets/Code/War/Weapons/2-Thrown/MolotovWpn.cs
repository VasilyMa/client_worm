using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class MolotovWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "коктейль Молотова",
            desc => new MolotovWpn (desc),
            () => _.War.Assets.MolotovIcon
        );


        private MolotovWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.MolotovWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (
                new MolotovBottle (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}