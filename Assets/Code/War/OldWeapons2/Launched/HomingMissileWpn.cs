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

    public class HomingMissileWpn : Weapon {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "самонаводящаяся ракета", desc => new HomingMissileWpn (desc), () => _.War.Assets.HomingMissileIcon
        );
        

        private GameObject     _weaponSprite;
        private LineCrosshair  _lineCrosshair;
        private PointCrosshair _pointCrosshair;
        private int            _adjustingSince;
        private XY             _target = XY.NaN;


        private HomingMissileWpn (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            State         = WeaponState.CanLockOn;
            _weaponSprite = Object.Instantiate (_.War.Assets.HomingMissileWeapon, Worm.WeaponSlot, false);
            var o = Object.Instantiate (_.War.Assets.PointCrosshair, (Vector3) td.XY, Quaternion.identity);
            _pointCrosshair = o.GetComponent <PointCrosshair> ();
        }


        public override void OnUnequip () {
            base.OnUnequip ();
            Object.Destroy (_weaponSprite);
            _pointCrosshair.References--;
        }


        public override void OnDeselect () {
            base.OnDeselect ();
            _.War.TurnPaused = false;
        }


        public override void Update (TurnData td) {
            switch (State) {
                case WeaponState.CanLockOn:
                    _pointCrosshair.transform.localPosition = (Vector3) td.XY;
                    _weaponSprite.transform.localRotation = Quaternion.identity;
                    _weaponSprite.transform.localScale = new Vector3 (Worm.FacesRight ? 1 : -1, 1, 1);
                    if (td.MB) {
                        State = WeaponState.BeforeCanAttack;
                        _target = td.XY;
                        
                        _lineCrosshair = Object.Instantiate (
                            _.War.Assets.LineCrosshair,
                            _weaponSprite.transform,
                            false
                        ).GetComponent<LineCrosshair> ();
                        Aim (XY.DirectionAngle (Worm.Position, td.XY), _weaponSprite.transform);
                    }
                    break;
                
                case WeaponState.BeforeCanAttack:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY), _weaponSprite.transform);
                    if (!td.MB) State = WeaponState.CanAttack;
                    break;
                
                case WeaponState.CanAttack:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY), _weaponSprite.transform);
                    if (td.MB) {
                        State = WeaponState.Adjusting;
                        _lineCrosshair.RingVisible = true;
                        _lineCrosshair.RingPosition = 1 / 90f;
                        _adjustingSince = _.War.Time;
                        Declare ();
                        _.War.TurnTime = Time.Seconds (3);
                        _.War.TurnPaused = true;
                    }
                    break;
                
                case WeaponState.Adjusting:
                    Aim (XY.DirectionAngle (Worm.Position, td.XY), _weaponSprite.transform);
                    int power = _.War.Time - _adjustingSince;
                    if (td.MB && power < 90) {
                        _lineCrosshair.RingPosition = (power + 1) / 90f;
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


        private void Shoot (TurnData td, float power) {
            _.War.Spawn (
                new HomingMissile (_target, _pointCrosshair),
                Worm.Position,
                (td.XY - Worm.Position).WithLength (power * Balance.ThrowSpeed)
            );
        }

    }

}
