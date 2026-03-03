using Core;
using DataTransfer.Data;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormRecoverCtrl : Controller <Worm> {

        private int _addedAt;


        public override void OnAttach () {
            _addedAt = _.War.Time;
//            Entity.Sprite.Recover (Entity.FacesRight, 0);
            Entity.Sprite.Animation  = WormAnimation.Recover;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Frame      = 0;
        }


        public override void Update (TurnData td) {
            _.War.Wait ();
            int age = _.War.Time - _addedAt;

            if (age < 60) {
                Entity.Sprite.Animation  = WormAnimation.Recover;
                Entity.Sprite.FacesRight = Entity.FacesRight;
                Entity.Sprite.Frame      = age;
            }
            else {
                Entity.AfterJump ();
            }
        }

    }

}