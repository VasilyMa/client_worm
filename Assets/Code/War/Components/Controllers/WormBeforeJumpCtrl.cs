using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormBeforeJumpCtrl : Controller <Worm> {

        private          int _addedAt;
        private readonly XY  _jumpVelocity;


        public WormBeforeJumpCtrl (XY jumpVelocity) {
            _jumpVelocity = jumpVelocity;
        }


        public override void OnAttach () {
            _addedAt = _.War.Time;
//            Entity.Sprite.BeforeJump (Entity.FacesRight, 0, 0, 0);
            Entity.Sprite.Animation    = WormAnimation.BeforeJump;
            Entity.Sprite.FacesRight   = Entity.FacesRight;
            Entity.Sprite.HeadRotation = float.NaN;
            Entity.Sprite.Frame        = 0;
        }


        public override void Update (TurnData td) {
            _.War.Wait ();
            int age = _.War.Time - _addedAt;

            if (age < 16) {
//                Entity.Sprite.BeforeJump (Entity.FacesRight, 0, 0, age);
                Entity.Sprite.Animation    = WormAnimation.BeforeJump;
                Entity.Sprite.FacesRight   = Entity.FacesRight;
                Entity.Sprite.HeadRotation = float.NaN;
                Entity.Sprite.Frame        = age;
            }
            else {
                Entity.Velocity = _jumpVelocity;
                Entity.Jump ();
            }
        }

    }

}