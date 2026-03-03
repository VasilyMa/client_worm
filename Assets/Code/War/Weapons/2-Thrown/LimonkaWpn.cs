using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;
using Time = Utils.Time;


namespace War.Weapons {

    public class LimonkaWpn : AimedWeapon {

        public new static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "лимонка",
            desc => new LimonkaWpn (desc),
            () => _.War.Assets.LimonkaIcon
        );


        private int _seconds = 5;


        protected LimonkaWpn (WeaponDescriptor desc) : base (desc) {
            Chargeable = true;
        }


        public override void OnEquip (TurnData td) {
            Sprite =
            Object.Instantiate (_.War.Assets.LimonkaWeapon, Worm.WeaponSlot, false);
            
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
                new Limonka (Time.Seconds (_seconds)),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * Charge / 90)
            );
        }

    }

}