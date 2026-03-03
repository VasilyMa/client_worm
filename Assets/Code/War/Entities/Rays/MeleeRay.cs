using System;
using System.Collections.Generic;
using Core;
using Math;
using War.Components;


namespace War.Entities.Rays {

    // Это неправильно что он не наследуется от Ray, но сделать составной коллайдер у меня руки не дошли
    // а зачем нам вообще этот класс тогда?
    [Obsolete]
    public class MeleeRay : Entity, IRayOrMobile {
        
        private Collider [] _colliders;
        

        public MeleeRay (Worm worm) {
            _colliders = new Collider [] {worm.Head, worm.Body, worm.Tail};
        }


        public void Cast (XY v) {
            // да мы здесь не пытаемся исключить объекты с которыми перекрываемся а наоборот считаем их
            var entities = new HashSet <MobileEntity> ();
            _.War.CollisionSystem.FindObstaclesMeleeRay (_colliders, v, entities);
        }

    }

}
