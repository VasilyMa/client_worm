using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;
using Object = UnityEngine.Object;
using Time = Utils.Time;


namespace War.OldWeapons2.Launched {

    // игрок может регулировать силу и изменять количество ракет которые хочет запустить
    // но изменение количества ракет нужно отключить когда он начнет стрелять
    public class MiniRocketsWpn : Weapon {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "мини-ракеты", desc => new MiniRocketsWpn (desc), () => _.War.Assets.MiniRocketsIcon
        );

        private GameObject    _weaponSprite;
        private LineCrosshair _crosshair;
        private int           _adjustingSince;
        private int           _shootingSince;
        private int           _power;


        private MiniRocketsWpn (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            State         = WeaponState.CanAttack;
            _weaponSprite = Object.Instantiate (_.War.Assets.MiniRocketsWeapon, Worm.WeaponSlot,    false);
            var o         = Object.Instantiate (_.War.Assets.LineCrosshair,     Worm.CrosshairSlot, false);
            _crosshair    = o.GetComponent <LineCrosshair> ();
        }


        public override void OnUnequip () {
            base.OnUnequip ();
            Object.Destroy (_weaponSprite);
            Object.Destroy (_crosshair.gameObject);
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
                        _crosshair.RingVisible = true;
                        _crosshair.RingPosition = 1 / 90f;
                        _adjustingSince = _.War.Time;
                        Declare ();
                        _.War.TurnTime = Time.Seconds (3);
                        _.War.TurnPaused = true;
                    }
                    break;
                
                case WeaponState.Adjusting:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY));
                    _power = _.War.Time - _adjustingSince;
                    if (td.MB && _power < 90) {
                        _crosshair.RingPosition = (_power + 1) / 90f;
                    }
                    else {
                        _crosshair.RingPosition = _power / 90f;
                        State = WeaponState.Shooting;
                        _shootingSince = _.War.Time;
                        goto case WeaponState.Shooting;
                    }
                    break;
                
                case WeaponState.Shooting:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY));
                    int t = _.War.Time - _shootingSince;
                    if (t % 15 == 0) {
                        _.War.Spawn (
                            new MiniRocket (),
                            Worm.Position,
                            (td.XY - Worm.Position).WithLength (_power / 90f * Balance.ThrowSpeed)
                        );
                    }                    
                    if (t >= 60) {
                        _.War.TurnPaused = false;
                        Worm.UnequipWeapon ();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }


        private void Aim (float angle) {
            Worm.Sprite.HeadRotation = angle;
            Aim (angle, _weaponSprite.transform);
            Aim (angle, _crosshair   .transform);
        }

    }

}
