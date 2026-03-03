using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Indication;
using Object = UnityEngine.Object;
using Time = Utils.Time;


namespace War.OldWeapons2 {

    public abstract class AbstractGrenadeWpn : Weapon {

        protected abstract GameObject    WeaponPrefab { get; }
        protected          GameObject    WeaponSprite;
        protected          LineCrosshair Crosshair;
        protected          int           AdjustingSince;
        protected          int           Timer = 5;


        protected AbstractGrenadeWpn (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            State        = WeaponState.CanAttack;
            WeaponSprite = Object.Instantiate (WeaponPrefab,               Worm.WeaponSlot,    false);
            var o        = Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false);
            Crosshair    = o.GetComponent <LineCrosshair> ();
            if (td.NumKey != 0) Timer = td.NumKey;
        }


        public override void OnUnequip () {
            base.OnUnequip ();
            Object.Destroy (WeaponSprite);
            Object.Destroy (Crosshair.gameObject);
        }


        public override void OnDeselect () {
            base.OnDeselect ();
            _.War.TurnPaused = false;
        }


        public override void Update (TurnData td) {
            if (td.NumKey != 0) Timer = td.NumKey;
            switch (State) {
                case WeaponState.CanAttack:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY));
                    if (td.MB) {
                        State = WeaponState.Adjusting;
                        Crosshair.RingVisible = true;
                        Crosshair.RingPosition = 1 / 90f;
                        AdjustingSince = _.War.Time;
                        Declare ();
                        _.War.TurnTime = Time.Seconds (3);
                        _.War.TurnPaused = true;
                    }
                    break;
                
                case WeaponState.Adjusting:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY));
                    int power = _.War.Time - AdjustingSince;
                    if (td.MB && power < 90) {
                        Crosshair.RingPosition = (power + 1) / 90f;
                    }
                    else {
                        _.War.TurnPaused = false;
                        Shoot (td, power / 90f);
                        Worm.UnequipWeapon ();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }


        protected abstract void Shoot (TurnData td, float power);


        private void Aim (float angle) {
            Worm.Sprite.HeadRotation = angle;
            Aim (angle, WeaponSprite.transform);
            Aim (angle, Crosshair   .transform);
        }


    }

}
