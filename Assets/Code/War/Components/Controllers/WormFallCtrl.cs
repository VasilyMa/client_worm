using Core;
using DataTransfer.Data;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormFallCtrl : Controller <Worm> {

        private int   _changedAt;
        private bool  _spinning; // задать false при столкновении и true при большой скорости
        private float _angularVelocity;
        private float _xBorder;


        /**
         * В общем, тут у нас 3 состояния - червяк летит прямо, крутится и скользит.
         *
         * Как включать?
         *
         * Если червяка сносит взрывом, включаем прямой полет.
         * Если его сносит слабым взрывом, включаем скольжение.
         * При ударе обо что-то смотрим скорость, и включаем кручение либо скольжение.
         * Если включено скольжение, но скорость стала велика, включаем кручение.
         *
         * Параметры анимаций
         *
         * У всех анимаций есть направление и кадр, зависящий от времени.
         */


        public override void OnAttach () {
            _changedAt = _.War.Time;
            if (Entity.Velocity.SqrLength > Settings.WormSpinSqrVelocity) {
                SlideInternal ();
            }
            else {
                Spin ();
            }
        }


        public void Slide () {
            if (!_spinning) return;
            if (Entity.Velocity.SqrLength > Settings.WormSpinSqrVelocity) {
                Spin ();
            }
            else {
                SlideInternal ();
            }
        }


        private void SlideInternal () {
            _spinning  = false;
            _changedAt = _.War.Time;
            _xBorder   = Entity.Position.X;
//            Entity.Sprite.Slide (Entity.FacesRight, 0);
            Entity.Sprite.Animation  = WormAnimation.Slide;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Frame      = 0;
        }


        public void Spin (float angularVelocity = 0.5f) {
            _spinning        = true;
            _changedAt       = _.War.Time;
            _angularVelocity = angularVelocity;
            if      (Entity.Velocity.X > 0) Entity.FacesRight = true;
            else if (Entity.Velocity.X < 0) Entity.FacesRight = false;
//            Entity.Sprite.Spin (Entity.FacesRight, 0);
            Entity.Sprite.Animation  = WormAnimation.Spin;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.Rotation   = 0;
        }


        public override void Update (TurnData td) {
            _.War.Wait ();
            Entity.Velocity.Y += WarScene.Gravity;

            if (Entity.Idle) {
                Entity.Recover ();
                return;
            }

            if (_spinning) {
                if      (Entity.Velocity.X > 0) Entity.FacesRight = true;
                else if (Entity.Velocity.X < 0) Entity.FacesRight = false;
//                Entity.Sprite.Spin (Entity.FacesRight, (_.War.Time - _changedAt) * _angularVelocity);
                Entity.Sprite.Animation  = WormAnimation.Slide;
                Entity.Sprite.FacesRight = Entity.FacesRight;
                Entity.Sprite.Rotation   = (_.War.Time - _changedAt) * _angularVelocity;
            }
            else {
                // сделать переход если скорость велика
                if (Entity.Velocity.SqrLength > Settings.WormSpinSqrVelocity) {
                    Spin ();
                    return;
                }
                
                // сделать гистерезис по координате х
                float x = Entity.Position.X;
                if (Entity.FacesRight) {
                    if (x > _xBorder) {
                        _xBorder = x;
                    }
                    else if (x < _xBorder - Settings.WormSlideXHysteresis) {
                        Entity.FacesRight = false;
                        _xBorder = x;
                    }
                }
                else {
                    if (x < _xBorder) {
                        _xBorder = x;
                    }
                    else if (x > _xBorder + Settings.WormSlideXHysteresis) {
                        Entity.FacesRight = true;
                        _xBorder = x;
                    }
                }
                
//                Entity.Sprite.Slide (Entity.FacesRight, _.War.Time - _changedAt);
                Entity.Sprite.Animation  = WormAnimation.Slide;
                Entity.Sprite.FacesRight = Entity.FacesRight;
                Entity.Sprite.Frame      = _.War.Time - _changedAt;
            }
        }

    }

}