using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Indication;
using Object = UnityEngine.Object;


namespace War.Weapons {

    /*
    **  оружие без механики хоминга
    **  состояния:
    **      исходное
    **      готовность
    **      регулировка
    **      стрельба - задержка перед атакой, выстрелы, задержка после атаки
    **      после стрельбы - такое же как исходное, разве что другая анимация
    **      конечное - анимация убирания оружия, пропускается если игрок тупо выбрал другое
    */
    public abstract class AimedWeapon1 : Weapon {


        private bool          _chargeable;
        private int           _charge;
        private const int     MaxCharge = 90;
        
        private WeaponState2  _state;
        private int           _stateChangedAt;

        private GameObject    _sprite;
        private LineCrosshair _crosshair;

        private int           _attackWarmup;
        private int           _shotCooldown;
        private int           _attackCooldown = 30;
        private int           _lastAttackCooldown;
        private int           _unequippingTime;
        private int           _attacks = 1;
        private int           _attacksDone;
        private int           _shots = 1;


        protected AimedWeapon1 (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            _state          = WeaponState2.JustEquipped;
            _stateChangedAt = _.War.Time;
            _crosshair      = Object.Instantiate (_.War.Assets.LineCrosshair, Worm.CrosshairSlot, false).GetComponent <LineCrosshair> ();
            _sprite         = Object.Instantiate (_.War.Assets.BazookaWeapon, Worm.WeaponSlot, false);
            Aim (td);
            UpdateSprite ();
        }


        public override void Update (TurnData td) {
            switch (_state) {
                case WeaponState2.JustEquipped:
                    // только что достали оружие
                    if (td.MB) {
                        // начало атаки
                        _.War.TurnPaused = true;
                        if (_chargeable) {
                            _state                  = WeaponState2.Charging;
                            _crosshair.RingVisible  = true;
                            _charge                 = 1;
                            _crosshair.RingPosition = 1f / MaxCharge;
                        }
                        else {
                            _state          = WeaponState2.Shooting;
                            _stateChangedAt = _.War.Time;
                            goto case WeaponState2.Shooting;
                        }
                        _stateChangedAt = _.War.Time;
                    }
                    break;
                
                case WeaponState2.Ready:
                    // включается после атаки
                    if (td.MB) {
                        // начало атаки
                        _.War.TurnPaused = true;
                        if (_chargeable) {
                            _state                  = WeaponState2.Charging;
                            _crosshair.RingVisible  = true;
                            _charge                 = 1;
                            _crosshair.RingPosition = 1f / MaxCharge;
                        }
                        else {
                            _state                  = WeaponState2.Shooting;
                            _stateChangedAt         = _.War.Time;
                            goto case WeaponState2.Shooting;
                        }
                        _stateChangedAt = _.War.Time;
                    }
                    break;
                
                case WeaponState2.Charging:
                    if (td.MB) {
                        _charge++;
                        _crosshair.RingPosition = (float) _charge / MaxCharge;
                        if (_charge < MaxCharge) break;
                    }
                    _state = WeaponState2.Shooting;
                    _stateChangedAt = _.War.Time;
                    _crosshair.RingPosition = (float) _charge / MaxCharge;
                    goto case WeaponState2.Shooting;
                    
                case WeaponState2.Shooting:
                    int t = _.War.Time - _stateChangedAt - _attackWarmup;
                    if (t < 0) break;
                    if (t % _shotCooldown == 0 && t < _shotCooldown * _shots) {
                        // ЛОГИРУЕМ ПЕРЕД ВЫСТРЕЛОМ
                        LogShoot(td);
                        
                        Shoot (td);
                        if (t >= _shotCooldown * (_shots - 1)) {
                            if (++_attacksDone >= _attacks) {
                                _crosshair.gameObject.SetActive (false);
                                _state = WeaponState2.Final;
                                _stateChangedAt = _.War.Time;
                                _.War.EndTurn (180);
                                goto case WeaponState2.Final;
                            }
                            _crosshair.RingVisible = false;
                        }
                    }
                    if (t >= _shotCooldown * (_shots - 1) + _attackCooldown) {
                        _state = WeaponState2.Ready;
                        _stateChangedAt = _.War.Time;
                        goto case WeaponState2.Ready;
                    }
                    break;
                
                case WeaponState2.Final:
                    if (_.War.Time - _stateChangedAt >= _lastAttackCooldown) {
                        Worm.UnequipWeaponSlowly ();
                        goto case WeaponState2.Unequipping;
                    }
                    break;
                
                case WeaponState2.Unequipping:
                    if (_.War.Time - _stateChangedAt >= _lastAttackCooldown) {
                        Worm.UnequipWeaponSlowly ();
                        goto case WeaponState2.Unequipping;
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }


        protected abstract void Shoot (TurnData td);
        
        /// <summary>
        /// Логирует выстрел перед вызовом Shoot
        /// </summary>
        protected void LogShoot(TurnData td)
        {
            /*float angle = XY.DirectionAngle(Worm.Position, td.XY) * Mathf.Rad2Deg;
            float power = (float)_charge;
            
            Core.Logger.LogWeaponFire(
                Worm.Team.TeamId,
                Worm.Name,
                this.GetType().Name,
                angle,
                power
            );*/
        }


        private void Aim (TurnData td) {
            float angle = XY.DirectionAngle (Worm.Position, td.XY);
            Worm.Sprite.HeadRotation = angle;
            bool flip = Mathf.RoundToInt (angle / Mathf.PI) % 2 != 0;
            _sprite.transform.localScale    = _crosshair.transform.localScale    = new Vector3 (1, flip ? -1 : 1, 1);
            _sprite.transform.localRotation = _crosshair.transform.localRotation = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
        }


        protected abstract void UpdateSprite ();

    }

}