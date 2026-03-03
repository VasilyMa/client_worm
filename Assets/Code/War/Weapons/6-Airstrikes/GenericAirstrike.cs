using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Indication;
using static Utils.Time;
using static War.Weapons.WeaponState;
using Object = UnityEngine.Object;


namespace War.Weapons {

    /*
    **  просто указываем место для удара и рассчитываем как должен полететь самолет
    **  то есть нам все же нужен объект самолета и настройки как он сбрасывает бомбы
    **  да и еще мы нажимаем 1 или 2 чтобы задать направление удара
    ** 
    **  Анимации:
    **    доставание оружия (канселится в атаку)
    **    начало атаки (когда червяк начинает говорить по рации, цель в этот момент фиксируется)
    **    здесь происходит спавн самолета и расход боеприпасов
    **    убирание оружия
    **
    **  И надо разобраться с канселами по аналогии с AimedWeapon
    */
    
    
    public class GenericAirstrike : Weapon {
        
        public static readonly WeaponDescriptor Descriptor = new WeaponDescriptor (
            "тестовый авиаудар",
            desc => new GenericAirstrike (desc),
            () => _.War.Assets.AirstrikeIcon
        );

        protected int            Warmup      = 20;
        protected int            Cooldown    = 20;
        protected int            UnequipTime = 20;

        protected WeaponState    State;
        protected int            StateChangedAt;
        protected XY             Target;
        protected bool           LeftToRight;

        protected GameObject     Sprite;
        protected GameObject     CrosshairObject;
        protected PointCrosshair Crosshair;
        

        protected GenericAirstrike (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip (TurnData td) {
            Sprite          = Object.Instantiate (_.War.Assets.Radio, Worm.WeaponSlot, false);
            CrosshairObject = Object.Instantiate (_.War.Assets.PointCrosshair, (Vector3) td.XY, Quaternion.identity);
            Crosshair       = CrosshairObject.GetComponent <PointCrosshair> ();
            LeftToRight     = true;
            Crosshair.SetLeftToRight ();
        }


        public override void OnReequip () {
            State          = Ready;
            StateChangedAt = _.War.Time;
            Sprite.SetActive (true);
        }


        public override void OnUnequip () {
            if (State == Final) return;
            State = Final;
            Object.Destroy (Sprite);
            Crosshair.References--;
            // Object.Destroy (Crosshair.gameObject);
        }


        public override void OnSlowUnequip () {
            switch (State) {
                case Initial:
                case NotEquipped:
                    Worm.UnequipWeapon ();
                    break;
                case Unequipping:
                case LastCooldown:
                case Final:
                    break;
                default:
                    // OnUnequipBegin ();
                    _.War.TurnPaused = false;
                    State            = Unequipping;
                    StateChangedAt   = _.War.Time;
                    break;
            }
        }


        public override void Update (TurnData td) {
            UpdateState (td);
        }


        private void UpdateState (TurnData td) {
            switch (State) {
                case Initial:
                case NotEquipped:
                    if (!Worm.CanUseWeapon) break;
                    OnReequip ();
                    goto case Ready;
                case Ready:
                    // двигаем курсор, меняем направление
                    CrosshairObject.transform.localPosition = (Vector3) (Target = td.XY);
                    if      (td.NumKey == 1) { LeftToRight = false; Crosshair.SetRightToLeft (); }
                    else if (td.NumKey == 2) { LeftToRight = true;  Crosshair.SetLeftToRight (); }
                    // как только нажимаем курсор фиксируется
                    if (td.MB) {
                        _.War.TurnPaused = true;
                        State            = WeaponState.Warmup;
                        StateChangedAt   = _.War.Time;
                        Crosshair.PlayLockOnAnimation ();
                        Crosshair.SetNonDirectional ();
                        goto case WeaponState.Warmup;
                    }
                    break;
                case WeaponState.Warmup:
                    // просто анимация, когда заканчивается спавним самолет
                    if (_.War.Time - StateChangedAt < Warmup) break;
                    Declare ();
                    OnAttack ();
                    _.War.EndTurn (Seconds (3));
                    _.War.TurnPaused = false;
                    State = LastCooldown;
                    StateChangedAt = _.War.Time;
                    goto case LastCooldown;
                case LastCooldown:
                    // опять же анимация
                    if (_.War.Time - StateChangedAt < Cooldown) break;
                    OnUnequipBegin ();
                    State          = Unequipping;
                    StateChangedAt = _.War.Time;
                    break;
                case Unequipping:
                    // тоже анимация
                    if (_.War.Time - StateChangedAt < UnequipTime) break;
                    Worm.UnequipWeapon ();
                    break;
                case Final:
                    break;
                default: throw new ArgumentOutOfRangeException ();
            }
        }


        protected virtual void OnUnequipBegin () {}


        protected virtual void OnAttack () {}

    }

}