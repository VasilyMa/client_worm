using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;


namespace War.OldWeapons.Launched {

    /**
     * Стреляет снарядом, подверженным ветру.
     */
    public class BazookaWpn : VariablePowerWpn {
        
        public static readonly WeaponDescriptor Descriptor =
        new WeaponDescriptor ("базука", desc => new BazookaWpn (desc), () => _.War.Assets.BazookaIcon);
        
        
        private GameObject _sprite;


        private BazookaWpn (WeaponDescriptor desc) : base (desc) {}


//        public override void OnDraw () {
//            base.OnDraw ();
//            _sprite = Object.Instantiate (_.War.Assets.BazookaWeapon, _.War.ActiveWorm.GameObject.transform, false);
//        }


        protected override void Shoot (XY wormPosition, XY v) {
            Declare ();
//            _.War.Spawn (new BazookaShell (), wormPosition, v * Balance.ThrowVelocity);
        }


        public override void Update (TurnData td) {
            base.Update (td);
            // повернуть спрайт, прицел, вызвать метод у червяка чтобы он посмотрел куда надо
        }


//        public override void OnUnequip () {
//            base.OnUnequip ();
//            Object.Destroy (_sprite);
//        }

    }

}
