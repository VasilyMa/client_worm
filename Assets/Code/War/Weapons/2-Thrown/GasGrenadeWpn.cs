using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class GasGrenadeWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "газовая граната",
            desc => new GasGrenadeWpn (desc),
            () => _.War.Assets.GasGrenadeIcon
        );


        private GasGrenadeWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.GasGrenadeWeapon, Worm.WeaponSlot, false);
            
            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (
                new GasGrenade (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}