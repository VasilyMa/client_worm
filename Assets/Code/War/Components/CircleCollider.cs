using Geometry;
using Math;
using War.Systems.Collisions;
using War.Systems.Terrain;


namespace War.Components {

    public class CircleCollider : Collider {

        private readonly XY    _o;
        private readonly float _r;


        private Circle _circle;
        public Circle Circle => _circle;


        public CircleCollider (
            Circle circle,
            float tangentialBounce = 0.9f, float normalBounce = 0.5f
        ) : base (tangentialBounce, normalBounce) {
            _o      = circle.O;
            _r      = circle.R;
            _circle = circle;
        }


        public CircleCollider (
            float x, float y, float r,
            float tangentialBounce = 0.9f, float normalBounce = 0.5f
        ) : base (tangentialBounce, normalBounce) {
            _o      = new XY (x, y);
            _circle = new Circle (x, y, _r = r);
        }


        public CircleCollider (
            XY o, float r,
            float tangentialBounce = 0.9f, float normalBounce = 0.5f
        ) : base (tangentialBounce, normalBounce) {
            _circle = new Circle (_o = o, _r = r);
        }


        public override Box Box =>
        new Box (Circle.O.X - Circle.R, Circle.O.X + Circle.R, Circle.O.Y - Circle.R, Circle.O.Y + Circle.R);


        protected override void DoUpdatePosition () {
            _circle.O = _o + Entity.Position;
        }


        public override bool Overlaps (Collider       other) => other .Overlaps (this);
        public override bool Overlaps (CircleCollider other) => Circle.Overlap  (Circle, other.Circle);
        public override bool Overlaps (BoxCollider    other) => Geom  .Overlap  (Circle, other.Box);
        public override bool Overlaps (Land           land ) { throw new System.NotImplementedException (); }


        public override Collision FlyInto (Collider other, XY v) => -other.FlyInto (this, -v);


        public override Collision FlyInto (CircleCollider other, XY v) {
            float dist = Geom.RayToCircle (Circle.O, v, other.Circle.O, Circle.R + other.Circle.R);

            if (float.IsNaN (dist) || dist < 0 || dist * dist >= v.SqrLength) return null;
            return new Collision (
                v.WithLength (dist),
                XY.NaN,
                this,
                other,
                Primitive.Circle (Circle),
                Primitive.Circle (other.Circle)
            );
        }


        private static bool CollideWithPoint (Circle c, ref XY v, XY point, ref Primitive primitive) {
            float dist = Geom.RayToCircle (c.O, v, point, c.R);
            if (float.IsNaN (dist) || dist < 0 || dist * dist >= v.SqrLength) return false;
            v.Length  = dist;
            primitive = Primitive.Circle (point);
            return true;
        }
        
        
        public override Collision FlyInto (BoxCollider other, XY v) {
            var circle = Circle;
            var box    = other.Box;

            bool collided  = false;
            var  primitive = Primitive.None;

            var
            point = new XY (circle.X + circle.R, circle.Y);
            if (point.X <= box.X0 && point.X + v.X > box.X0) {
                float dist = Geom.RayTo1D (point.X, v.X, box.X0);
                float y    = point.Y + dist * v.Y;
                if (box.Y0 <= y && y <= box.Y1) {
                    collided  =  true;
                    v         *= dist;
                    primitive =  Primitive.Left (box.X0);
                }
            }

            point = new XY (circle.X - circle.R, circle.Y);
            if (point.X >= box.X1 && point.X + v.X < box.X1) {
                float dist = Geom.RayTo1D (point.X, v.X, box.X1);
                float y    = point.Y + dist * v.Y;
                if (box.Y0 <= y && y <= box.Y1) {
                    collided  =  true;
                    v         *= dist;
                    primitive =  Primitive.Right (box.X1);
                }
            }

            point = new XY (circle.X, circle.Y - circle.R);
            if (point.Y >= box.Y1 && point.Y + v.Y < box.Y1) {
                float dist = Geom.RayTo1D (point.Y, v.Y, box.Y1);
                float x    = point.X + dist * v.X;
                if (box.X0 <= x && x <= box.X1) {
                    collided  =  true;
                    v         *= dist;
                    primitive =  Primitive.Top (box.Y1);
                }
            }

            point = new XY (circle.X, circle.Y + circle.R);
            if (point.Y <= box.Y0 && point.Y + v.Y > box.Y0) {
                float dist = Geom.RayTo1D (point.Y, v.Y, box.Y0);
                float x    = point.X + dist * v.X;
                if (box.X0 <= x && x <= box.X1) {
                    collided  =  true;
                    v         *= dist;
                    primitive =  Primitive.Bottom (box.Y0);
                }
            }

            if (v.X > 0 || v.Y > 0) collided |= CollideWithPoint (circle, ref v, new XY (box.X0, box.Y0), ref primitive);
            if (v.X > 0 || v.Y < 0) collided |= CollideWithPoint (circle, ref v, new XY (box.X0, box.Y1), ref primitive);
            if (v.X < 0 || v.Y < 0) collided |= CollideWithPoint (circle, ref v, new XY (box.X1, box.Y1), ref primitive);
            if (v.X < 0 || v.Y > 0) collided |= CollideWithPoint (circle, ref v, new XY (box.X1, box.Y0), ref primitive);

            return collided ? new Collision (v, XY.NaN, this, other, Primitive.Circle (circle), primitive) : null;
        }


        public override Collision FlyInto (Land land, XY v) {
            var collision = land.ApproxCollision (Circle, v); // bug здесь один и тот же круг
            if (collision != null) collision.Collider1 = this;
            return collision;
        }

    }

}