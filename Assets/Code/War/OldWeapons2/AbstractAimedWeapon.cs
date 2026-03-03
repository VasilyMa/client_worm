using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Indication;
using Object = UnityEngine.Object;
using Time = Utils.Time;


namespace War.OldWeapons2 {
    
    // Базовый класс для оружия, которым ты целишься и стреляешь без регулировки силы, 1 раз.
    public abstract class AbstractAimedWeapon : Weapon {

        protected abstract GameObject    WeaponPrefab { get; }
        protected          GameObject    WeaponSprite;
        protected          LineCrosshair Crosshair;


        protected AbstractAimedWeapon (WeaponDescriptor desc) : base (desc) {}


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
                        Declare ();
                        Shoot (td);
                        _.War.TurnTime = Time.Seconds (3);
                        Worm.UnequipWeapon ();
                    }
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException ();
            }
        }


        protected abstract void Shoot (TurnData td);


        private void Aim (float angle) {
            Worm.Sprite.HeadRotation = angle;
            Aim (angle, WeaponSprite.transform);
            Aim (angle, Crosshair   .transform);
        }

    }

}
