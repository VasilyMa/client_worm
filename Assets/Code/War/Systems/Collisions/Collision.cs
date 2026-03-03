using System;
using Core;
using Geometry;
using UnityEngine;
using War.Components;
using War.Entities;
using BoxCollider = War.Components.BoxCollider;
using Collider = War.Components.Collider;
using XY = Math.XY;


namespace War.Systems.Collisions {

    public class Collision : IComparable <Collision> {

        public XY        Offset;
        public XY        Normal;
        public Collider  Collider1,  Collider2;
        public Primitive Primitive1, Primitive2;


        public Collision (XY offset, Primitive p1, Primitive p2) {
            Offset     = offset;
            Primitive1 = p1;
            Primitive2 = p2;
        }


        public Collision (XY offset, XY normal, Collider c1, Collider c2, Primitive p1, Primitive p2) {
            Offset     = offset;
            Normal     = normal;
            Collider1  = c1;
            Collider2  = c2;
            Primitive1 = p1;
            Primitive2 = p2;
        }


        public int CompareTo (Collision other) =>
        other == null ? -1 : Offset.SqrLength.CompareTo (other.Offset.SqrLength);


        public bool IsLandCollision => Collider2 == null;


        public void ImprovePrecision () {
            PrimitiveUtils.GetOffsetNormal (Primitive1, Primitive2, ref Offset, out Normal);
        }


        public static Collision operator - (Collision c) =>
        c == null ? null : new Collision (
            -c.Offset,
            -c.Normal,
            c.Collider2,
            c.Collider1,
            c.Primitive2,
            c.Primitive1
        );


        public void Invert () {
            // возможно буду использовать этот метод чтобы не нагружать GC
            Offset = -Offset;
            Normal = -Normal;
            var c = Collider1;  Collider1  = Collider2;  Collider2  = c;
            var p = Primitive1; Primitive1 = Primitive2; Primitive2 = p;
        }


        public static bool operator < (Collision a, Collision b) {
            if (a == null) return false;
            if (b == null) return true;
            return a.Offset.SqrLength < b.Offset.SqrLength;
        }


        public static bool operator > (Collision a, Collision b) {
            if (b == null) return false;
            if (a == null) return true;
            return a.Offset.SqrLength > b.Offset.SqrLength;
        }


        public static bool operator <= (Collision a, Collision b) {
            if (b == null) return true;
            if (a == null) return false;
            return a.Offset.SqrLength <= b.Offset.SqrLength;
        }


        public static bool operator >= (Collision a, Collision b) {
            if (a == null) return true;
            if (b == null) return false;
            return a.Offset.SqrLength >= b.Offset.SqrLength;
        }


        public void DoMove () {
            var e = Collider1.Entity;
            e.Position += Offset.WithLengthReduced (Settings.PhysicsPrecision);
            e.MP       -= Mathf.Sqrt (Offset.SqrLength / e.Velocity.SqrLength);
        }


        public void DoBounce () {
            var e1 = Collider1.Entity;
            var e2 = Collider2.Entity;
            
            if (e1.Mass == 0 || e2.Mass == 0) {
                throw new InvalidOperationException ("Объект не имеет вообще никакой массы!");
            }

            e1.OnBeforeCollision (this);
            e2.OnBeforeCollision (-this);
            if (
                XY.Dot (Normal, e2.Velocity) > 0 ||
                e2.MP * e2.Velocity.Length <= Settings.PhysicsPrecision
                // объект мешается
            ) {
                if (e1.PushableFor (e2)) {
                    if (e2.PushableFor (e1)) {
                        // оба объекта изменят скорости
                        var   velocity   = (e1.Mass * e1.Velocity + e2.Mass * e2.Velocity) / (e1.Mass + e2.Mass);
                        var   v1         = e1.Velocity - velocity;
                        var   v2         = e2.Velocity - velocity;
                        float tangBounce = Mathf.Sqrt (Collider1.TangentialBounce * Collider2.TangentialBounce);
                        float normBounce = Mathf.Sqrt (Collider1.NormalBounce     * Collider2.NormalBounce);
                        e1.Velocity      = velocity + Geom.Bounce (v1, Normal, tangBounce, normBounce);
                        e2.Velocity      = velocity + Geom.Bounce (v2, Normal, tangBounce, normBounce);
                        e1.IdleAt        =
                        e2.IdleAt        = System.Math.Max (e1.IdleAt, e2.IdleAt);
                    }
                    else {
                        // e2 неподвижен
                        float tangBounce = Mathf.Sqrt (Collider1.TangentialBounce * Collider2.TangentialBounce);
                        float normBounce = Mathf.Sqrt (Collider1.NormalBounce     * Collider2.NormalBounce);
                        e1.Velocity = e2.Velocity + Geom.Bounce (
                            e1.Velocity - e2.Velocity,
                            Normal,
                            tangBounce,
                            normBounce
                        );
                        if (e1.IdleAt < e2.IdleAt) e1.IdleAt = e2.IdleAt;
                    }
                }
                else {
                    if (e2.PushableFor (e1)) {
                        // e1 неподвижен
                        float tangBounce = Mathf.Sqrt (Collider1.TangentialBounce * Collider2.TangentialBounce);
                        float normBounce = Mathf.Sqrt (Collider1.NormalBounce     * Collider2.NormalBounce);
                        e2.Velocity = e1.Velocity + Geom.Bounce (
                            e2.Velocity - e1.Velocity,
                            Normal,
                            tangBounce,
                            normBounce
                        );
                        if (e2.IdleAt < e1.IdleAt) e2.IdleAt = e1.IdleAt;
                    }
                    else {
                        // вот хз на самом деле что делать
                        throw new InvalidOperationException ($"Столкнулись взаимно нетолкаемые объекты: {e1}, {e2}");
                    }
                }
            }
            
            UnityEngine.Debug.Log ($"after colllision: v1={e1.Velocity}, v2={e2.Velocity}");

            e1.OnCollision (this);
            e2.OnCollision (-this);
        }


        public void DoHardBounce () {
            var e1 = Collider1.Entity;
            var e2 = Collider2.Entity;
            e1.OnBeforeCollision (this);
            e2.OnBeforeCollision (-this);
            e1.Velocity = Geom.Bounce (
                e1.Velocity,
                Normal,
                Mathf.Sqrt (Collider1.TangentialBounce * Collider2.TangentialBounce),
                Mathf.Sqrt (Collider1.NormalBounce     * Collider2.NormalBounce)
            );
            if (e1.IdleAt < e2.IdleAt) e1.IdleAt = e2.IdleAt;
            e1.OnCollision (this);
            e2.OnCollision (-this);
        }

    }

}
