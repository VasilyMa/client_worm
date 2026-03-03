using Core;
using DataTransfer.Data;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormFlyCtrl : Controller <Worm> {

        private int _addedAt;


        public override void OnAttach () {
            _addedAt = _.War.Time;
            if      (Entity.Velocity.X > 0) Entity.FacesRight = true;
            else if (Entity.Velocity.X < 0) Entity.FacesRight = false;
//            Entity.Sprite.Fly (Entity.FacesRight, Entity.Velocity.Angle, 0);
            Entity.Sprite.Animation  = WormAnimation.Fly;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Rotation   = Entity.Velocity.Angle;
            Entity.Sprite.Frame      = 0;
        }


        public override void Update (TurnData td) {
            _.War.Wait ();
            Entity.Velocity.Y += WarScene.Gravity;

            if (Entity.Idle) { // маловероятно конечно
                Entity.Recover ();
                return;
            }
            
            if      (Entity.Velocity.X > 0) Entity.FacesRight = true;
            else if (Entity.Velocity.X < 0) Entity.FacesRight = false;
//            Entity.Sprite.Fly (Entity.FacesRight, Entity.Velocity.Angle, _.War.Time - _addedAt);
            Entity.Sprite.Animation  = WormAnimation.Fly;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Rotation   = Entity.Velocity.Angle;
            Entity.Sprite.Frame      = _.War.Time - _addedAt;
        }

    }

}
