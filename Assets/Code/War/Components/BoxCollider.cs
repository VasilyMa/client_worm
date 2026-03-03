using Geometry;
using Math;
using War.Systems.Collisions;
using War.Systems.Terrain;


namespace War.Components {

    public class BoxCollider : Collider {

        private readonly float _x0, _x1, _y0, _y1;

        private Box _box;
        public override Box Box => _box;


        public BoxCollider (
            Box box,
            float tangentialBounce = 0.9f, float normalBounce = 0.5f
        ) : base (tangentialBounce, normalBounce) {
            _x0  = box.X0;
            _x1  = box.X1;
            _y0  = box.Y0;
            _y1  = box.Y1;
            _box = box;
        }


        public BoxCollider (
            float x0, float x1, float y0, float y1,
            float tangentialBounce = 0.9f, float normalBounce = 0.5f
        ) : base (tangentialBounce, normalBounce) {
            _box = new Box (_x0 = x0, _x1 = x1, _y0 = y0, _y1 = y1);
        }


        protected override void DoUpdatePosition () {
            var p   = Entity.Position;
            _box.X0 = _x0 + p.X;
            _box.X1 = _x1 + p.X;
            _box.Y0 = _y0 + p.Y;
            _box.Y1 = _y1 + p.Y;
        }


        public override bool Overlaps (Collider       other) => other.Overlaps (this);
        public override bool Overlaps (CircleCollider other) => Geom .Overlap  (other.Circle, Box);
        public override bool Overlaps (BoxCollider    other) => Box  .Overlap  (Box, other.Box);
        public override bool Overlaps (Land           land ) { throw new System.NotImplementedException (); }


        public override Collision FlyInto (Collider       other, XY v) => -other.FlyInto (this, -v);
        public override Collision FlyInto (CircleCollider other, XY v) => -other.FlyInto (this, -v);


        public override Collision FlyInto (BoxCollider other, XY v) {
            bool collided = false;
            var  box      = Box;
            var  cBox     = other.Box;
            var  p1       = Primitive.None;
            var  p2       = Primitive.None;

            if (box.X1 <= cBox.X0 && box.X1 + v.X > cBox.X0) {
                // vx > 0
                float dist = Geom.RayTo1D (box.X1, v.X, cBox.X0);
                float dy   = v.Y * dist;
                if (box.Y0 + dy < cBox.Y1 && box.Y1 + dy > cBox.Y0) {
                    collided =  true;
                    v        *= dist;
                    p1       =  Primitive.Right (box.X1);
                    p2       =  Primitive.Left (cBox.X0);
                }
            }
            if (box.X0 >= cBox.X1 && box.X0 + v.X < cBox.X1) {
                // vx < 0
                float dist = Geom.RayTo1D (box.X0, v.X, cBox.X1);
                float dy   = v.Y * dist;
                if (box.Y0 + dy < cBox.Y1 && box.Y1 + dy > cBox.Y0) {
                    collided =  true;
                    v        *= dist;
                    p1       =  Primitive.Left (box.X0);
                    p2       =  Primitive.Right (cBox.X1);
                }
            }
            if (box.Y1 <= cBox.Y0 && box.Y1 + v.Y > cBox.Y0) {
                // vy > 0
                float dist = Geom.RayTo1D (box.Y1, v.Y, cBox.Y0);
                float dx   = v.X * dist;
                if (box.X0 + dx < cBox.X1 && box.X1 + dx > cBox.X0) {
                    collided =  true;
                    v        *= dist;
                    p1       =  Primitive.Top (box.Y1);
                    p2       =  Primitive.Bottom (cBox.Y0);
                }
            }
            if (box.Y0 >= cBox.Y1 && box.Y0 + v.Y < cBox.Y1) {
                // vy < 0
                float dist = Geom.RayTo1D (box.Y0, v.Y, cBox.Y1);
                float dx   = v.X * dist;
                if (box.X0 + dx < cBox.X1 && box.X1 + dx > cBox.X0) {
                    collided =  true;
                    v        *= dist;
                    p1       =  Primitive.Bottom (box.Y0);
                    p2       =  Primitive.Top (cBox.Y1);
                }
            }
            return collided ? new Collision (v, XY.NaN, this, other, p1, p2) : null;
        }


        public override Collision FlyInto (Land land, XY v) {
            var collision = land.ApproxCollision (Box, v);
            if (collision != null) {
                collision.Collider1 = this;
            }
            return collision;
        }

    }

}