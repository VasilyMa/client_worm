using System.Collections.Generic;
using Core;
using Math;
using War.Components;
using War.Systems.Collisions;


// Лучи это костыльные сущности на самом деле.
// Их мы указываем в методах каста лучей чтобы работали коллайдеры и считалась проходимость.
// Луч врезается в объект непроходимый для него и воздействует.
// У луча также может быть спрайт и т.д.
// Луч таки может быть заспавнен в мире.
// Но коллайдер луча мы в мир не добавляем.


namespace War.Entities.Rays {

    public class Ray : Entity, IRayOrMobile {

        protected Collider Collider { get; set; }


        protected Collision Cast (XY v) {
            var cObj = CastToObjects (v);
            if (cObj != null) v = cObj.Offset;

            var cLand = Collider.FlyInto (_.War.Land, v);
            return cLand ?? cObj;
        }


        private Collision CastToObjects (XY v) {
            var excluded = new HashSet <MobileEntity> ();
            _.War.CollisionSystem.AddOverlappingEntities (Collider, excluded);
            
            Collision min = null;

            foreach (var o in _.War.CollisionSystem.FindObstaclesRay (
                Collider,
                v,
                c => !c.Entity.PassableFor (this) && !excluded.Contains (c.Entity)
            )) {
                var temp = Collider.FlyInto (o, v);
                if (temp < min) min = temp;
            }
            return min;
        }

    }

}
