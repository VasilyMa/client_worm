using System;
using Geometry;
using UnityEngine;
using Box = Math.Box;
using Circle = Math.Circle;
using Collision = War.Systems.Collisions.Collision;
using XY = Math.XY;


namespace War.Systems.Terrain {

    public partial class Land {

        public float TangentialBounce { get; } = 0.9f;
        public float NormalBounce     { get; } = 0.5f;


        public Collision ApproxCollision (Circle circle, XY v) {
            var primitive = Primitive.None;

            var   o = circle.O;
            float r = circle.R;

            bool collided =
            (v.X > 0 && RayToRight    (o, ref v, r, ref primitive)) |
            (v.X < 0 && RayToLeft     (o, ref v, r, ref primitive)) |
            (v.Y > 0 && RayToTop      (o, ref v, r, ref primitive)) |
            (v.Y < 0 && RayToBottom   (o, ref v, r, ref primitive)) |
            (r > 0   && RayToVertices (o, ref v, r, ref primitive));

            return collided ? new Collision (v, Primitive.Circle (circle), primitive) : null;
        }


        public Collision ApproxCollision (Box b, XY v) {
            var  boxPrimitive  = Primitive.None;
            var  landPrimitive = Primitive.None;
            bool collided      = false;

            // вершины
            if (v.X > 0 || v.Y > 0) {
                var corner = new XY (b.X1, b.Y1);
                if (
                    RayToRight (corner, ref v, 0, ref landPrimitive, true) |
                    RayToTop   (corner, ref v, 0, ref landPrimitive, true)
                ) {
                    collided     = true;
                    boxPrimitive = Primitive.Circle (corner);
                }
            }
            if (v.X > 0 || v.Y < 0) {
                var corner = new XY (b.X1, b.Y0);
                if (
                    RayToRight  (corner, ref v, 0, ref landPrimitive) |
                    RayToBottom (corner, ref v, 0, ref landPrimitive, true)
                ) {
                    collided     = true;
                    boxPrimitive = Primitive.Circle (corner);
                }
            }
            if (v.X < 0 || v.Y < 0) {
                var corner = new XY (b.X0, b.Y0);
                if (
                    RayToLeft   (corner, ref v, 0, ref landPrimitive) |
                    RayToBottom (corner, ref v, 0, ref landPrimitive)
                ) {
                    collided     = true;
                    boxPrimitive = Primitive.Circle (corner);
                }
            }
            if (v.X < 0 || v.Y > 0) {
                var corner = new XY (b.X0, b.Y1);
                if (
                    RayToLeft (corner, ref v, 0, ref landPrimitive, true) |
                    RayToTop  (corner, ref v, 0, ref landPrimitive)
                ) {
                    collided     = true;
                    boxPrimitive = Primitive.Circle (corner);
                }
            }

            // потом с сторонами прямоугольника и вершинами земли
            if (v.X > 0 && RightSideToVertices (b, ref v, ref landPrimitive)) {
                collided     = true;
                boxPrimitive = Primitive.Right (b.X1);
            }
            if (v.X < 0 && LeftSideToVertices (b, ref v, ref landPrimitive)) {
                collided     = true;
                boxPrimitive = Primitive.Left (b.X0);
            }
            if (v.Y > 0 && TopSideToVertices (b, ref v, ref landPrimitive)) {
                collided     = true;
                boxPrimitive = Primitive.Top (b.Y1);
            }
            if (v.Y < 0 && BottomSideToVertices (b, ref v, ref landPrimitive)) {
                collided     = true;
                boxPrimitive = Primitive.Bottom (b.Y0);
            }
            return collided ? new Collision (v, boxPrimitive, landPrimitive) : null;
        }


        private bool RayToRight (XY o, ref XY v, float w, ref Primitive primitive, bool ceil = false) {
            var startXY = new XY (o.X + w, o.Y);
            var endXY   = startXY + v;

            int startX = System.Math.Max (Mathf.CeilToInt (startXY.X), 0);
            int endX   = System.Math.Min (Mathf.CeilToInt (endXY.X), Width) - 1;
            for (int x = startX; x <= endX; x++) {
                float d = Geom.RayTo1D (startXY.X, v.X, x);
                
                if (d < 0 || d > 1) throw new Exception ($"RayToRight, d = {d:R}");
                    
                int   y = ceil ? Mathf.CeilToInt (startXY.Y + d * v.Y) - 1 : Mathf.FloorToInt (startXY.Y + d * v.Y);
                if (this [x, y] == 0) continue;
                v *= d;
                primitive = Primitive.Left (x);
                return true;
            }
            return false;
        }


        private bool RayToLeft (XY o, ref XY v, float w, ref Primitive primitive, bool ceil = false) {
            var startXY = new XY (o.X - w, o.Y);
            var endXY   = startXY + v;

            int startX = System.Math.Min (Mathf.FloorToInt (startXY.X), Width) - 1;
            int endX   = System.Math.Max (Mathf.FloorToInt (endXY.X), 0);
            for (int x = startX; x >= endX; x--) {
                float d = Geom.RayTo1D (startXY.X, v.X, x + 1);
                
                if (d < 0 || d > 1) throw new Exception ($"RayToLeft, d = {d:R}");

                int   y = ceil ? Mathf.CeilToInt (startXY.Y + d * v.Y) - 1 : Mathf.FloorToInt (startXY.Y + d * v.Y);
                if (this [x, y] == 0) continue;
                v *= d;
                primitive = Primitive.Right (x + 1);
                return true;
            }
            return false;
        }


        private bool RayToTop (XY o, ref XY v, float w, ref Primitive primitive, bool ceil = false) {
            var startXY = new XY (o.X, o.Y + w);
            var endXY   = startXY + v;

            int startY = System.Math.Max (Mathf.CeilToInt (startXY.Y), 0);
            int endY   = System.Math.Min (Mathf.CeilToInt (endXY.Y), Height) - 1;
            for (int y = startY; y <= endY; y++) {
                float d = Geom.RayTo1D (startXY.Y, v.Y, y);
                
                if (d < 0 || d > 1) throw new Exception ($"RayToTop, d = {d:R}");

                int   x = ceil ? Mathf.CeilToInt (startXY.X + d * v.X) - 1 : Mathf.FloorToInt (startXY.X + d * v.X);
                if (this [x, y] == 0) continue;
                v *= d;
                primitive = Primitive.Bottom (y);
                return true;
            }
            return false;
        }


        private bool RayToBottom (XY o, ref XY v, float w, ref Primitive primitive, bool ceil = false) {
            var startXY = new XY (o.X, o.Y - w);
            var endXY   = startXY + v;

            int startY = System.Math.Min (Mathf.FloorToInt (startXY.Y), Height) - 1;
            int endY   = System.Math.Max (Mathf.FloorToInt (endXY.Y), 0);
            for (int y = startY; y >= endY; y--) {
                float d = Geom.RayTo1D (startXY.Y, v.Y, y + 1);
                
                if (d < 0 || d > 1) throw new Exception ($"RayToBottom, d = {d:R}");

                int   x = ceil ? Mathf.CeilToInt (startXY.X + d * v.X) - 1 : Mathf.FloorToInt (startXY.X + d * v.X);
                if (this [x, y] == 0) continue;
                v *= d;
                primitive = Primitive.Top (y + 1);
                return true;
            }
            return false;
        }


        // todo optimize
        private bool RayToVertices (XY o, ref XY v, float r, ref Primitive primitive) {
            float d2      = v.SqrLength;
            var   nearest = XY.NaN;

            var box = new Box (
                Mathf.Min (o.X, o.X + v.X) - r,
                Mathf.Max (o.X, o.X + v.X) + r,
                Mathf.Min (o.Y, o.Y + v.Y) - r,
                Mathf.Max (o.Y, o.Y + v.Y) + r
            ).ToTiles (LandTile.Size);

            for     (int x = box.X0; x < box.X1; x++)
            for     (int y = box.Y0; y < box.Y1; y++)
            foreach (var pt in Tiles [x, y].Vertices) {
                float dist = Geom.RayToCircle (o, v, pt, r);
                if (float.IsNaN (dist) || dist < 0 || dist * dist >= d2) continue;
                d2      = dist * dist;
                nearest = pt;
            }
            
            if (nearest.IsNaN) return false;

            primitive = Primitive.Circle (nearest);
            v.Length  = Mathf.Sqrt (d2);

            return true;
        }

        
        private bool RightSideToVertices (Box b, ref XY v, ref Primitive primitive) {
            var box = new Box (
                b.X1,
                b.X1 + v.X,
                Mathf.Min (0, v.Y) + b.Y0,
                Mathf.Max (0, v.Y) + b.Y1
            ).ToTiles (LandTile.Size);
            
            float d = 1;
            var nearest = XY.NaN;
            
            for     (int ix = box.X0; ix < box.X1; ix++)
            for     (int iy = box.Y0; iy < box.Y1; iy++)
            foreach (var pt in Tiles [ix, iy].Vertices) {
                if (pt.X < b.X1/* || pt.X >= b.X1 + v.X*/) continue; // исключить (-inf, 0)
                float dist = Geom.RayTo1D (b.X1, v.X, pt.X);

                if (dist >= d) continue;
                
                float y = pt.Y - v.Y * dist;
                if (y < b.Y0 || y > b.Y1) continue;

                d = dist;
                nearest = pt;
            }
            if (nearest.IsNaN) return false;

            primitive = Primitive.Circle (nearest);
            v *= d;
            
            return true;
        }
        
        
        private bool LeftSideToVertices (Box b, ref XY v, ref Primitive primitive) {
            var box = new Box (
                b.X0 + v.X,
                b.X0,
                Mathf.Min (0, v.Y) + b.Y0,
                Mathf.Max (0, v.Y) + b.Y1
            ).ToTiles (LandTile.Size);

            float d = 1;
            var nearest = XY.NaN;
            
            for     (int ix = box.X0; ix < box.X1; ix++)
            for     (int iy = box.Y0; iy < box.Y1; iy++)
            foreach (var pt in Tiles [ix, iy].Vertices) {
                if (/*pt.X <= b.X0 + v.X || */pt.X > b.X0) continue;
                float dist = Geom.RayTo1D (b.X0, v.X, pt.X);

                if (dist >= d) continue;

                float y = pt.Y - v.Y * dist;
                if (y < b.Y0 || y > b.Y1) continue;

                d = dist;
                nearest = pt;
            }
            if (nearest.IsNaN) return false;

            primitive = Primitive.Circle (nearest);
            v *= d;
            
            return true;
        }


        private bool TopSideToVertices (Box b, ref XY v, ref Primitive primitive) {
            var box = new Box (
                Mathf.Min (0, v.X) + b.X0,
                Mathf.Max (0, v.X) + b.X1,
                b.Y0,
                b.Y0 + v.Y
            ).ToTiles (LandTile.Size);

            float d = 1;
            var nearest = XY.NaN;

            for     (int ix = box.X0; ix < box.X1; ix++)
            for     (int iy = box.Y0; iy < box.Y1; iy++)
            foreach (var pt in Tiles [ix, iy].Vertices) {
                if (pt.Y < b.Y1/* || pt.Y >= b.Y1 + v.Y*/) continue;
                float dist = Geom.RayTo1D (b.Y1, v.Y, pt.Y);

                if (dist >= d) continue;

                float x = pt.X - v.X * dist;
                if (x <= b.X0 || x >= b.X1) continue;

                d = dist;
                nearest = pt;
            }
            if (nearest.IsNaN) return false;

            primitive = Primitive.Circle (nearest);
            v *= d;

            return true;
        }


        private bool BottomSideToVertices (Box b, ref XY v, ref Primitive primitive) {
            var box = new Box (
                Mathf.Min (0, v.X) + b.X0,
                Mathf.Max (0, v.X) + b.X1,
                b.Y0 + v.Y,
                b.Y0
            ).ToTiles (LandTile.Size);

            float d = 1;
            var nearest = XY.NaN;
            
            for     (int ix = box.X0; ix < box.X1; ix++)
            for     (int iy = box.Y0; iy < box.Y1; iy++)
            foreach (var pt in Tiles [ix, iy].Vertices) {
                if (/*pt.Y <= b.Y0 + v.Y || */pt.Y > b.Y0) continue;
                float dist = Geom.RayTo1D (b.Y0, v.Y, pt.Y);

                if (dist >= d) continue;

                float x = pt.X - v.X * dist;
                if (x <= b.X0 || x >= b.X1) continue;

                d = dist;
                nearest = pt;
            }
            if (nearest.IsNaN) return false;

            primitive = Primitive.Circle(nearest);
            v *= d;

            return true;
        }

    }

}
