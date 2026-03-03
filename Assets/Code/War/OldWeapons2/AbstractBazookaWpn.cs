using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Indication;
using Object = UnityEngine.Object;
using Time = Utils.Time;


namespace War.OldWeapons2 {

    // Базовый класс для оружий с регулировкой силы.
    public abstract class AbstractBazookaWpn : Weapon {

        protected abstract GameObject    WeaponPrefab { get; }
        protected          GameObject    WeaponSprite;
        protected          LineCrosshair Crosshair;
        protected          int           AdjustingSince;


        protected AbstractBazookaWpn (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            State        = WeaponState.CanAttack;
            WeaponSprite = Object.Instantiate (WeaponPrefab,               Worm.WeaponSlot,    false);
            var o        = Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false);
            Crosshair    = o.GetComponent <LineCrosshair> ();
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
                        
                        // Вычисляем угол выстрела
                        float angle = XY.DirectionAngle(Worm.Position, td.XY) * Mathf.Rad2Deg;
                        float normalizedPower = power / 90f;
                        
                        
                        Shoot (td, normalizedPower);
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
