using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;


namespace War.OldWeapons {

    // базовый класс для всяких базук, гранат, поддерживает регулировку силы и ровно 1 выстрел
    public abstract class VariablePowerWpn : Weapon {

        protected int StateChangedAt;
        protected int Power;

        protected WeaponState State;

//        protected Action <TurnData> UpdateState;


        protected VariablePowerWpn (WeaponDescriptor desc) : base (desc) {}


        public override void OnEquip () {
            StateChangedAt = _.War.Time;
            State          = WeaponState.Idle2;
        }


        public override void OnDraw () {
            StateChangedAt = _.War.Time;
            State          = WeaponState.Idle2;
        }


        public override void Update (TurnData td) {
            switch (State) {
                case WeaponState.Idle2:
                    if (td.MB) {
                        State            = WeaponState.AdjustingPower;
                        StateChangedAt   = _.War.Time;
                        _.War.TurnPaused = true;
                    }
                    break;
                case WeaponState.AdjustingPower:
                    Power = _.War.Time - StateChangedAt;
                    if (!td.MB || Power >= 90) {
                        Shoot (td, Power / 90f);
                        State            = WeaponState.Shooting;
                        StateChangedAt   = _.War.Time;
                        _.War.TurnPaused = false;
                    }
                    break;
            }
        }


        public override void OnInterrupt () {
            if (State == WeaponState.AdjustingPower) {
                Shoot (Power / 90f);
//                _.War.WeaponSystem.Unequip ();
            }
        }


        public override void OnUnequip () {
        }


        protected void Shoot (float power) {
            var wormPosition = _.War.ActiveWorm.Position;
            var v            = new XY (_.War.Random.Angle ()) * power;
            Shoot (wormPosition, v);
        }


        protected void Shoot (TurnData td, float normalizedPower) 
        {
            // Вычисляем угол выстрела из TurnData
            float angle = XY.DirectionAngle(_.War.ActiveWorm.Position, td.XY) * Mathf.Rad2Deg;
            float power = normalizedPower * 100f; // переводим в проценты (0-100%)
             

            // Создаем вектор скорости на основе направления и мощности
            var wormPosition = _.War.ActiveWorm.Position;
            var v = (td.XY - wormPosition).WithLength(normalizedPower * 25f); // масштабируем скорость
            Shoot(wormPosition, v);
        }


        protected abstract void Shoot (XY wormPosition, XY v);

    }

}
