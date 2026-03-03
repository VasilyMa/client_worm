using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Entities.Projectiles;
using War.Indication;
using static Utils.Time;
using static War.Weapons.WeaponState;
using Object = UnityEngine.Object;


namespace War.Weapons {

    /*
    **  Анимации:
    **      доставание оружия (канселится в стрельбу)
    **      регулировка силы
    **      начало атаки
    **      задержки между выстрелами (на последнем выстреле индикатор силы убрать)
    **      если выстрел последний и атака последняя, завершение стрельбы и убирание
    **      если выстрел последний но атака нет, задержка между атаками, снятие с паузы и ожидание новой атаки
    **      если оружие убрали не в момент последнего выстрела
    */


    public class AimedWeapon : Weapon {

        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "тестовое оружие",
            desc => new AimedWeapon (desc),
            () => _.War.Assets.BazookaIcon
        );

        protected bool Targetable;
        protected XY   Target;

        protected bool Chargeable;
        protected int  Charge;

        protected int  Attacks            = 1;
        protected int  AttacksDone;
        protected int  Shots              = 1;
        protected int  ShotsDone;

        protected int  Warmup;
        protected int  ShotCooldown       = 10;
        protected int  Cooldown           = 10;
        protected int  CancelableCooldown = 20;
        protected int  UnequipTime        = 20;

        protected WeaponState   State;
        protected int           StateChangedAt;

        protected GameObject    Sprite;
        protected LineCrosshair LineCrosshair;
        

        protected AimedWeapon (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            // Sprite        = ...
            // LineCrosshair = ...
            // if (Worm.CanUseWeapon) OnReequip ();
        }


        public override void OnReequip () {
            State = Ready;
            StateChangedAt = _.War.Time;
            Sprite.SetActive (true);
            LineCrosshair.gameObject.SetActive (true);
            LineCrosshair.RingVisible = false;
        }


        public override void OnSlowUnequip () {
            switch (State) {
                case Initial:
                case NotEquipped:
                    Worm.UnequipWeapon ();
                    break;
                case WeaponState.Cooldown:
                    _.War.TurnPaused = false;
                    State            = LastCooldown;
                    // не сбрасываем время состояния
                    break;
                case Unequipping:
                case LastCooldown:
                case Final:
                    break;
                default:
                    OnUnequipBegin ();
                    _.War.TurnPaused = false;
                    State            = Unequipping;
                    StateChangedAt   = _.War.Time;
                    break;
            }
        }


        public override void OnUnequip () {
            if (State == Final) return;
            State = Final;
            Object.Destroy (Sprite);
            Object.Destroy (LineCrosshair.gameObject);
        }


        public override void Update (TurnData td) {
            BeforeUpdateState (td);
            UpdateState       (td);
            AfterUpdateState  (td);
        }


        protected virtual void BeforeUpdateState (TurnData td) {}


        private void UpdateState (TurnData td) {
            switch (State) {
                case Initial:
                case NotEquipped:
                    if (!Worm.CanUseWeapon) break;
                    OnReequip ();
                    goto case Ready;
                case Ready:
                    if (!td.MB) break; 
                    Debug.Log($"[AimedWeapon.UpdateState Ready] ДО OnAttack: Charge={Charge}");

                    if (AttacksDone == 0) OnFirstAttack(td);
                    else OnAttack(td);

                    Debug.Log($"[AimedWeapon.UpdateState Ready] ПОСЛЕ OnAttack: Charge={Charge}");

                    _.War.TurnPaused = true;
                    if (!Chargeable) goto attack;
                    LineCrosshair.RingVisible  = true;
                    LineCrosshair.RingPosition = 1 / 90f;
                    State                  = Charging;
                    StateChangedAt         = _.War.Time;
                    break;
                case Charging:
                    if (td.MB) {
                        Charge++;
                        Debug.Log($"[AimedWeapon.Update] Charging... Charge={Charge}");
                        LineCrosshair.RingPosition = Charge / 90f;
                        if (Charge < 90) break;
                    }
                    OnChargingDone ();
                    attack:
                    ShotsDone      = 0;
                    State          = WeaponState.Warmup;
                    StateChangedAt = _.War.Time;
                    goto case WeaponState.Warmup;
                case WeaponState.Warmup:
                    if (_.War.Time - StateChangedAt < Warmup) break;
                    goto shot;
                case WeaponState.ShotCooldown:
                    if (_.War.Time - StateChangedAt < ShotCooldown) break;
                    shot:
                    OnShot (td);
                    if (++ShotsDone < Shots) {
                        State          = WeaponState.ShotCooldown;
                        StateChangedAt = _.War.Time;
                        goto case WeaponState.ShotCooldown;
                    }
                    LineCrosshair.RingVisible = false;
                    if (++AttacksDone < Attacks) {
                        OnAttackEnd ();
                        State          = WeaponState.Cooldown;
                        StateChangedAt = _.War.Time;
                        goto case WeaponState.Cooldown;
                    }
                    LineCrosshair.gameObject.SetActive (false);
                    _.War.TurnPaused = false;
                    State            = LastCooldown;
                    StateChangedAt   = _.War.Time;
                    OnLastAttackEnd ();
                    goto case LastCooldown;
                case WeaponState.Cooldown:
                    if (_.War.Time - StateChangedAt < Cooldown) break;
                    State          = WeaponState.CancelableCooldown;
                    StateChangedAt = _.War.Time;
                    goto case WeaponState.CancelableCooldown;
                case WeaponState.CancelableCooldown:
                    // зачем я сделал это состояние вообще
                    // возможно это задержка после выстрела для того чтобы игрок не стрелял прям сразу же
                    // и отмена задержки будет происходить только если игрок уберет оружие совсем
                    if (_.War.Time - StateChangedAt < CancelableCooldown) break;
                    _.War.TurnPaused = false;
                    State            = Ready;
                    StateChangedAt   = _.War.Time;
                    goto case Ready;
                case LastCooldown:
                    if (_.War.Time - StateChangedAt < Cooldown) break;
                    OnUnequipBegin ();
                    State          = Unequipping;
                    StateChangedAt = _.War.Time;
                    break;
                case Unequipping:
                    if (_.War.Time - StateChangedAt < UnequipTime) break;
                    Worm.UnequipWeapon ();
                    break;
                case Final:
                    break;
                default:
                    throw new ArgumentOutOfRangeException ();
            }
            // Worm.DebugSetName (State.ToString ());
        }


        protected void AfterUpdateState (TurnData td) {
            if (State == Initial || State == NotEquipped || State == Final || td == null) {
                return;
            }
            float angle = XY.DirectionAngle (Worm.Position, td.XY);
            Worm.Sprite.HeadRotation = angle;
            bool flip = Mathf.RoundToInt (angle / Mathf.PI) % 2 != 0;
            Sprite       .transform.localScale    =
            LineCrosshair.transform.localScale    = new Vector3 (1, flip ? -1 : 1, 1);
            Sprite       .transform.localRotation =
            LineCrosshair.transform.localRotation = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
        }


        protected void OnUnequipBegin () {}


        protected void OnLastAttackEnd () {
            _.War.EndTurn (Seconds (3));
        }


        protected void OnAttackEnd () {}


        protected virtual void OnShot (TurnData td) {
            // Вычисляем угол и мощность для логирования
            float angle = XY.DirectionAngle(Worm.Position, td.XY) * Mathf.Rad2Deg;
            float power = (Chargeable ? (Charge / 90f) : 1f) * 100f;
            
            Debug.Log($"[AimedWeapon.OnShot] Логирование выстрела: Charge={Charge}, Power={power:F2}%");
            
            // ЛОГИРУЕМ ВЫСТРЕЛ
            Core.Logger.LogWeaponFire(
                Worm.Team.TeamId,
                Worm.Name,
                this.GetType().Name,
                angle,
                power
            );
             
            // _.War.Spawn (...);
        }


        protected void OnChargingDone () {
            Debug.Log($"[AimedWeapon.OnChargingDone] Зарядка завершена. Charge={Charge}");
        }


        protected void OnAttack (TurnData td) { 
            UseAmmo (); 
        }

        protected void OnFirstAttack(TurnData td)
        {
            Debug.Log($"[AimedWeapon.OnFirstAttack] ВХОД. Charge={Charge}");
            Declare();
            Debug.Log($"[AimedWeapon.OnFirstAttack] ПОСЛЕ Declare(). Charge={Charge}");
            OnAttack(td);
            Debug.Log($"[AimedWeapon.OnFirstAttack] ВЫХОД. Charge={Charge}");
        }
    }

}