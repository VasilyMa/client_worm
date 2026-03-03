using Core;
using DataTransfer.Data;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormJumpCtrl : Controller <Worm> {

        private int _addedAt;

        
        public override void OnAttach () {
            _addedAt = _.War.Time;
//            Entity.Sprite.Jump (Entity.FacesRight, 0);
            Entity.Sprite.Animation  = WormAnimation.Jump;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Frame      = 0;
        }


        public override void Update (TurnData td) {
            _.War.Wait ();
            Entity.Velocity.Y += WarScene.Gravity;
//            Entity.Sprite.Jump (Entity.FacesRight, _.War.Time - _addedAt);
            Entity.Sprite.Animation  = WormAnimation.Jump;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Frame      = _.War.Time - _addedAt;
        }

    }

}