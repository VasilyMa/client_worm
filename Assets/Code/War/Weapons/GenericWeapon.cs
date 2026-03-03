using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;


namespace War.Weapons {


    public abstract class GenericWeapon : Weapon {

        protected       bool        Targetable;
        protected       XY          Target;
        
        protected       bool        Chargeable;
        private         int         _charge;
        protected const int         MaxCharge = 90;
        protected       float       ChargeFactor => (float) _charge / MaxCharge;
        protected       float       ChargeFactorPlusOne => (1f + _charge) / MaxCharge;
        
        private         WeaponState1 _state;
        private         int         _stateChangedAt;
        
        protected       int         AttackWarmup;
        protected       int         AttackCooldown;
        protected       int         Attacks = 1;
        
        protected       int         ShotWarmup;
        protected       int         ShotCooldown;
        protected       int         Shots = 1;


        protected GenericWeapon (WeaponDescriptor desc) : base (desc) {}


        public WeaponState1 State {
            get { return _state; }
            set {
                _state = value;
                _stateChangedAt = _.War.Time;
            }
        }


        public override void OnEquip (TurnData td) {
            base.OnEquip (td);
            State = Targetable ? WeaponState1.Targeting2 : WeaponState1.Ready2;
        }


        public override void Update (TurnData td) {

            Debug.Log($"[GenericWeapon.Update] State={State}, Time={_.War.Time}, Attacks={Attacks}, Charge={_charge}");

            switch (State) {
                
                case WeaponState1.Targeting1:
                    // фаза указания цели, ждем отпускания мыши
                    Target = td.XY;
                    if (!td.MB) State = WeaponState1.Targeting2;
                    break;
                
                case WeaponState1.Targeting2:
                    // фаза указания цели, ждем нажатия мыши
                    Target = td.XY;
                    if (td.MB) State = WeaponState1.Ready1;
                    break;
                
                case WeaponState1.Ready1:
                    // фаза прицеливания, ждем отпускания мыши
                    if (!td.MB) State = WeaponState1.Ready2;
                    break;
                
                case WeaponState1.Ready2:
                    // огонь по нажатию мыши
                    if (td.MB) {
                        // на этом месте тратится количество атак и объявляется оружие
                        --Attacks;
                        _.War.TurnPaused = true;
                        BeginAttack ();
                        
                        if (Chargeable) {
                            State = WeaponState1.Charging;
                        }
                        else {
                            State = WeaponState1.Shooting;
                            goto case WeaponState1.Shooting;
                        }
                    }
                    break;
                
                case WeaponState1.Charging:
                    // фаза регулировки мощности
                    _charge = _.War.Time - _stateChangedAt;
                    if (!td.MB || _charge >= MaxCharge) {
                        State = WeaponState1.Shooting;
                        goto case WeaponState1.Shooting;
                    }
                    break;
                
                case WeaponState1.Shooting:
                    int t = _.War.Time - _stateChangedAt;
                    if (t < AttackWarmup) break;
                    t -= AttackWarmup;
                    int loop = ShotWarmup + ShotCooldown;
                    if (t < Shots * loop) {
                        if (t % loop == ShotWarmup) {
                            // Вычисляем угол и мощность ДО выстрела
                            var direction = (Target - Worm.Position).Normalized;
                            float angle = direction.Angle * Mathf.Rad2Deg;
                            float power = ChargeFactor * 100f;
                            
                            // ЛОГИРУЕМ ВЫСТРЕЛ с отладкой
                            Debug.Log($"[GenericWeapon] Попытка логирования выстрела: " +
                                $"Оружие={this.GetType().Name}, " +
                                $"Червяк={Worm?.Name}, " +
                                $"Команда={Worm?.Team?.TeamId}, " +
                                $"Угол={angle:F2}°, " +
                                $"Мощность={power:F2}%");
                            
                            LogFireWeapon(angle, power);
                            
                            // Затем выполняем выстрел
                            Shoot(td);
                        }
                        break;
                    }
                    t -= Shots * loop;
                    if (t >= AttackCooldown) {
                        _.War.TurnPaused = false;
                        _charge          = 0;
                        if (Attacks > 0) State = Targetable ? WeaponState1.Targeting1 : WeaponState1.Ready2;
                        else             _.War.EndTurn (Balance.RetreatTime);
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }


        private void BeginAttack () {
            Declare ();
        }


        protected abstract void Shoot(TurnData td);
        
        /// <summary>
        /// Логирует выстрел оружия с углом и мощностью
        /// </summary>
        protected void LogFireWeapon(float angle, float power)
        {
            try
            {
                if (Worm == null)
                {
                    Debug.LogError("[GenericWeapon] Worm is null!");
                    return;
                }

                if (Worm.Team == null)
                {
                    Debug.LogError("[GenericWeapon] Worm.Team is null!");
                    return;
                }

                // Проверяем инициализацию Loggerю
                Debug.Log($"[GenericWeapon] Вызов Logger.LogWeaponFire - " +
                    $"TeamId={Worm.Team.TeamId}, " +
                    $"WormName={Worm.Name}, " +
                    $"WeaponName={this.GetType().Name}, " +
                    $"Angle={angle:F2}°, " +
                    $"Power={power:F2}%");

                Core.Logger.LogWeaponFire(
                    Worm.Team.TeamId,
                    Worm.Name,
                    this.GetType().Name,
                    angle,
                    power
                );

                Debug.Log("[GenericWeapon] Логирование выстрела успешно!");
                }
            catch (Exception ex)
            {
                Debug.LogError($"[GenericWeapon] Ошибка при логировании выстрела: {ex.Message}\n{ex.StackTrace}");
            }
        }

    }

}