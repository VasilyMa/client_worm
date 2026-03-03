using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;
using Time = Utils.Time;


namespace War.Weapons {

    public class GhostGrenadeWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "призрачная граната",
            desc => new GhostGrenadeWpn (desc),
            () => _.War.Assets.GhostGrenadeIcon
        );


        private int _seconds = 1;


        protected GhostGrenadeWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.GhostGrenadeWeapon, Worm.WeaponSlot, false);

            LineCrosshair =
            Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();

            if (td.NumKey > 0) _seconds = td.NumKey;
            if (Worm.CanUseWeapon) OnReequip ();
        }


        protected override void BeforeUpdateState (TurnData td) {
            if (td.NumKey > 0) _seconds = td.NumKey;
        }


        protected override void OnShot (TurnData td) {
            _.War.Spawn (
                new GhostGrenade (Time.Seconds (_seconds)),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}