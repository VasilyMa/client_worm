using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Components.Other;
using Worm = War.Entities.Worm;


namespace War.Components.Controllers {

    public class WormWalkCtrl : Controller <Worm> {

        private int _walksSince;
        

        public override void OnAttach () => Stand ();


        private void Stand () {
            _walksSince = _.War.Time + 1;
//            Entity.Sprite.Stand (Entity.FacesRight, Entity.LookAngle, 0);
            Entity.Sprite.Animation = WormAnimation.Stand;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.HeadRotation = float.NaN;
        }


        private void Walk () {
//            Entity.Sprite.Walk (Entity.FacesRight, Entity.LookAngle, 0, _.War.Time - _walksSince);
            Entity.Sprite.Animation = WormAnimation.Walk;
            Entity.Sprite.FacesRight = Entity.FacesRight;
            Entity.Sprite.HeadRotation = float.NaN;
            Entity.Sprite.Frame = _.War.Time - _walksSince;
        }


        public override void Update (TurnData td) {
            // проверить есть ли земля под ногами
            Entity.ExcludeEntities ();
            var collision = Entity.Cast (Entity.Tail, new XY (0, -Worm.MaxDescend));
            
            // если нет то упасть
            if (collision == null) {
                Entity.Jump ();
                return;
            }

            // проверить можем мы двигаться или нет (наш ход или нет)
            if (td == null || Entity != _.War.ActiveWorm) {
                // не анимировать движение
                Stand ();
            }
            else {
                if (td.A ^ td.D) Entity.FacesRight = td.D;

                // проверить прыгаем мы или нет
                // если да то прыгнуть
                if (td.S) {
                    var vj = new XY (0f, Worm.HighJumpSpeed);
                    if (td.A ^ td.D) {
                        vj.Rotate (td.A ? Worm.HighJumpAngle : -Worm.HighJumpAngle);
                    }
                    Entity.BeforeJump (vj);
                    return;
                }
                if (td.W) {
                    var vj = new XY (0f, Worm.JumpSpeed).Rotated (Entity.FacesRight ? -Worm.JumpAngle : Worm.JumpAngle);
                    Entity.BeforeJump (vj);
                    return;
                }
                
                // здесь логика ползания
                // в общем кастим бля я даже не знаю что
                // кастим круглый коллайдер с середины червяка строго в сторону
                // потом кастим его вверх и вниз и смотрим что дальше было
                if (td.A ^ td.D) {
                    Walk ();
                    float vx = td.D ? Worm.WalkSpeed: -Worm.WalkSpeed;
                    var test = Entity.Cast (
                        new CircleCollider (Entity.Position, Worm.HeadRadius),
                        new XY (vx, 0)
                    );
                    if (test == null) {
                        vx = Mathf.MoveTowards (vx, 0, Settings.PhysicsPrecision);

                        var c = new CircleCollider (Entity.Position + new XY (vx, 0), Worm.HeadRadius);

                        const float testDown = Worm.HalfBodyHeight + Worm.MaxDescend;
                        const float testUp   = Worm.HalfBodyHeight + Worm.MaxClimb;

                        var collisionDown = Entity.Cast (c, new XY (0, -testDown));
                        var collisionUp   = Entity.Cast (c, new XY (0, testUp + 3 * Settings.PhysicsPrecision));
                        // 3: учет того что каст вниз и вверх с корректировкой,
                        // плюс возможная ошибка округления при сложении
                        
                        if (collisionUp == null) {
                            // быстрая проверка отсутствия потолка которая почти всегда будет проходить
                            if (collisionDown == null) {
                                Entity.Position += new XY (vx, 0);
                                Entity.Jump ();
                                return;
                            }
                            float offset =
                            Worm.HalfBodyHeight + Mathf.Clamp (collisionDown.Offset.Y + Settings.PhysicsPrecision, -testDown, 0);
                            
                            if (offset <= Worm.MaxClimb) {
                                Entity.Poke ();
                                Entity.Position += new XY (vx, offset);
                                return;
                            }
                        }
                        else {
                            float spaceAbove = Mathf.Clamp (collisionUp  .Offset.Y - Settings.PhysicsPrecision, 0, testUp);
                            float spaceBelow = collisionDown == null ? -testDown :
                            Mathf.Clamp (collisionDown.Offset.Y + Settings.PhysicsPrecision, -testDown, 0);
                            // spaceBelow < 0 помним
                            if (spaceAbove - spaceBelow > Worm.BodyHeight) {
                                float offset = Worm.HalfBodyHeight + spaceBelow;
                                if (offset <= Worm.MaxClimb) {
                                    Entity.Poke ();
                                    Entity.Position += new XY (vx, offset);
                                    return;
                                }
                            }
                        }
                    }
                    // else а если не null то мы усложнять не будем пока просто не двигаемся
                }
                else {
                    Stand ();
                }
            }
            
            // логика стояния на месте, Offset.X = 0, Offset.Y < 0 и я надеюсь не NaN
            if (collision.Offset.Y >= -4 * Settings.PhysicsPrecision) {
                return;
            }
            collision.Offset.Y += Settings.PhysicsPrecision;
            Entity.Poke ();
            Entity.Position += collision.Offset;
        }

    }

}
