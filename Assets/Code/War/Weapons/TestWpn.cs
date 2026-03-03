using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;


namespace War.Weapons {

    public class TestWpn : GenericWeapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "тестовое оружие",
            desc => new TestWpn (desc),
            () => _.War.Assets.BazookaIcon
        );
        
        
        protected GameObject    _sprite;
        protected LineCrosshair _crosshair;


        protected TestWpn (WeaponDescriptor desc) : base (desc) {
            Attacks        = 3;
            Shots          = 5;
            ShotCooldown   = 10;
            AttackCooldown = 30;
            Chargeable     = true;
        }


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            _sprite =
                Object.Instantiate (_.War.Assets.BazookaWeapon, Worm.WeaponSlot, false);
            _crosshair =
                Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).
                GetComponent <LineCrosshair> ();
            Aim (td);
        }


        public override void OnUnequip () {
            base.OnUnequip ();
            Object.Destroy (_sprite);
            Object.Destroy (_crosshair.gameObject);
        }


        public override void Update (TurnData td) {
            base.Update (td);
            Aim (td);
            UpdateCrosshair ();
        }


        private void Aim (TurnData td) {
            float angle = XY.DirectionAngle (Worm.Position, td.XY);
            Worm.Sprite.HeadRotation = angle;
            bool flip = Mathf.RoundToInt (angle / Mathf.PI) % 2 != 0;
            _sprite   .transform.localScale    =
            _crosshair.transform.localScale    = new Vector3 (1, flip ? -1 : 1, 1);
            _sprite   .transform.localRotation =
            _crosshair.transform.localRotation = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
        }


        private void UpdateCrosshair () {
            if (State == WeaponState1.Charging) {
                _crosshair.RingVisible  = true;
                _crosshair.RingPosition = ChargeFactorPlusOne;
            }
            else if (Chargeable && State == WeaponState1.Shooting) {
                _crosshair.RingVisible  = true;
                _crosshair.RingPosition = ChargeFactor;
            }
            else {
                _crosshair.RingVisible = false;
            }
        }


        protected override void Shoot (TurnData td) {
            _.War.Spawn (
                new BazookaShell (),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (Balance.ThrowSpeed * ChargeFactor)
            );
        }

    }

}