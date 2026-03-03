using System;
using System.Collections;
using Core;
using DataTransfer.Data;
using Utils;


namespace War.OldWeapons {

    [Obsolete]
    public class OldStdWeapon : OldWeapon {

        protected TurnData TurnData { get; private set; }

        protected int      Power    { get; private set; }
        protected float    Power01  => Power / (1.5f * Settings.FPS);
        
        protected int      Attacks        = 1;     // у бластера 2
        protected int      Shots          = 1;     // у автомата много
        
        protected int      AttackCooldown = Time.Seconds (0.5f);
        protected int      ShotCooldown   = Time.Seconds (0.1f);
        
        protected virtual bool ConstPower     => true;  // если тут false, то кнопку выстрела надо держать
        protected virtual bool Removable      => false; // если тут false, то арсенал блокируется при использовании
        protected virtual bool RequiresClick  => true;  // стреляет только по клику или если просто если держать кнопку

        private IEnumerator _mainRoutine;

        
        protected OldStdWeapon (WeaponDescriptor desc) : base (desc) {}


        private IEnumerator MainRoutine () {
            // ждать когда он нажмет кнопку
            while (!TurnData.MB) yield return null;

            // атака
            if (!Removable) _.War.WeaponSystem.WeaponsLocked = true;
            OnFirstAttack ();
            _.War.TurnPaused = true;
            foreach (var step in AttackRoutine ()) yield return step;

            for (int attack = 1; attack != Attacks; attack++) {
                // задержка, если эта атака не последняя
                for (int t = 0; t < AttackCooldown; t++) yield return null;
                _.War.TurnPaused = false;

                if (RequiresClick) {
                    // ждать когда он отпустит кнопку, потом нажмет
                    while (TurnData.MB) yield return null;
                }
                while (!TurnData.MB) yield return null;

                // атака
                _.War.TurnPaused = true;
                foreach (var step in AttackRoutine ()) yield return step;
            }
            
            _.War.TurnPaused = false;
            OnLastAttackEnd ();
        }


        private IEnumerable AttackRoutine () {
            OnAttack ();
            
            if (!ConstPower) {
                Power = 0;
                const int max = (int) (1.5f * Settings.FPS);
                // кнопка подразумевается что уже нажата
                while (TurnData.MB && ++Power < max) yield return null;
            }
            
            // стреляем очередь
            OnShot ();
            for (int shot = 1; shot < Shots; shot++) {
                for (int t = 0; t < ShotCooldown; t++) yield return null;
                OnShot ();
            }
            OnAttackEnd ();
        }


        public override void Update (TurnData td) {
            TurnData = td;
            if (td.NumKey > 0) OnNumberPress (td.NumKey);

            if (_mainRoutine.MoveNext ()) OnUpdate ();
            else {
                throw new NotImplementedException ();
//                Entity.Weapon = null;
            }
        }


        protected void EndTurn (int retreatTicks) => _.War.TurnTime = retreatTicks;


        public override void OnEquip         () => _mainRoutine = MainRoutine ();
        public virtual  void OnNumberPress   (int n) {}
        public virtual  void OnUpdate        () {}
        public virtual  void OnFirstAttack   () => Declare ();
        public virtual  void OnAttack        () {}
        public virtual  void OnShot          () {}
        public virtual  void OnAttackEnd     () {}
        public virtual  void OnLastAttackEnd () => EndTurn (OldTime.SecondsToTicks (2));
        // это что-то вроде блока finally
        public override void OnUnequip       () => _.War.TurnPaused = false;

    }

}