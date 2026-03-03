using System;
using System.Collections.Generic;
using Core;
using Geometry;
using UnityEngine;
using War.Components;
using Box = Math.Box;
using Collider = War.Components.Collider;
using Collision = War.Systems.Collisions.Collision;
using XY = Math.XY;


namespace War.Entities {

    public class MobileEntity : Entity, IMobile, IRayOrMobile {
        
        // класс объектов, совместимых с физическими взаимодействиями
        // мы обычно не двигаем такие объекты напрямую, а только через движок

        private XY _position;
        public XY Position {
            get { return _position; }
            set {
                PositionChanged?.Invoke (_position, value);
                PokeTiles (false);
                _position = value;
                foreach (var c in _colliders) c.UpdatePosition ();
            }
        }
        public event Action <XY, XY> PositionChanged;


        public XY Velocity; // todo - сделать свойство, чтобы менять угол поворота


        public  float MP;       // movement points, очки движения - от 0 до 1
        public  float Mass = 1;
        public  int   MassRank; // порядок массы - объект будет нельзя сдвинуть если у него больше порядок массы

        
        public  int   IdleAt;   // тик когда объект станет неподвижным
        public  bool  Idle => IdleAt <= _.War.Time;
        // если объект неподвижен то физика его не обрабатывает
        // также можно в апдейте что-то не делать в зависимости от этого
        
        private XY    _anchor;  // якорь - метка для определения того, надо ли сбрасывать неподвижность
        

        private HashSet <Collider> _colliders = new HashSet <Collider> ();
        private Rope _rope; // может быть null
        
        
        // сущности которые исключены из проверки препятствий
        // мы должны при спавне объекта заполнять это множество
        // protected только для рейкаста червяка
        protected HashSet <MobileEntity> Excluded = new HashSet <MobileEntity> ();


        // если да, объект e проходит через наш объект беспрепятственно
        // если нет, он сталкивается
        // взаимная проходимость допускается
        public virtual bool PassableFor (IRayOrMobile e) => true;


        // если да, мы можем сдвинуть наш объект когда в него влетит объект e
        // если нет, наш объект не двигается от столкновения объекта e
        public virtual bool PushableFor (MobileEntity e) => MassRank <= e.MassRank;


        public virtual bool MeleeHittable => true;


        public void OnSpawn (XY position, XY velocity) {
            Position =
            _anchor  = position;
            Velocity = velocity;
            Poke ();
            _.War.World.Add (this);
            OnSpawn ();
            ExcludeEntitiesMutual ();
        }


        protected override void OnDespawn () {
            PokeTiles (true);
            foreach (var c in _colliders) c.ClearTiles ();
        }


        // при добавлении в "рантайме" вызывать ExcludeEntitiesMutual
        public void AddCircleCollider (float r) {
            var collider = new CircleCollider (0, 0, r) {Entity = this};
            collider.OnAttach ();
            _colliders.Add (collider);
            _.War.CollisionSystem.Add (collider);
        }


        // при добавлении в "рантайме" вызывать ExcludeEntitiesMutual
        public void AddCollider (Collider collider) {
            collider.Entity = this;
            collider.OnAttach ();
            _colliders.Add (collider);
            _.War.CollisionSystem.Add (collider);
        }


        // public только для рейкаста червяка
        public void ExcludeEntities () {
            var system = _.War.CollisionSystem;
            foreach (var collider in _colliders) {
                system.AddOverlappingEntities (collider, Excluded);
            }
        }
        
        
        // вызывать при спавне объекта
        public void ExcludeEntitiesMutual () {
            var system = _.War.CollisionSystem;
            foreach (var collider in _colliders) {
                system.AddOverlappingEntities (collider, Excluded);
            }
            foreach (var entity in Excluded) {
                entity.Excluded.Add (this);
            }
        }


        public void PhysicsBegin () => MP = 1;


        public void PhysicsUpdate (bool hardColliding) {
            if (Idle || Velocity.Length * MP <= Settings.PhysicsPrecision) {
                return;
            }
            
            // по идее мы тут должны ограничить скорость сущности веревкой

            var collision = NextCollision ();

            if (collision == null) {
                Position += (MP * Velocity).WithLengthReduced (Settings.PhysicsPrecision);
                MP = 0;
                goto checkSinking;
            }

            collision.ImprovePrecision ();

            if (collision.IsLandCollision) {
                collision.DoMove ();

                OnBeforeCollision (collision);
                var с = collision.Collider1;
                Velocity = Geom.Bounce (
                    Velocity,
                    collision.Normal,
                    Mathf.Sqrt (с.TangentialBounce * _.War.Land.TangentialBounce),
                    Mathf.Sqrt (с.NormalBounce     * _.War.Land.NormalBounce)
                );
                OnCollision (collision);
            }
            else if (hardColliding) {
                collision.DoMove ();
                collision.DoHardBounce ();
            }
            else {
                collision.DoMove ();
                collision.DoBounce ();
            }

            checkSinking:
            if (HasSunk ()) Despawn ();
        }


        protected virtual bool HasSunk () => false;


        public virtual void OnBeforeCollision (Collision collision) {}
        public virtual void OnCollision       (Collision collision) {}


        public void PhysicsEnd () {
            // уменьшить скорость для застрявших объектов
            Velocity *= 1 - MP;
            
            // логика для механики неподвижности
            if (Idle) {
                _anchor = Position;
            }
            else {
                _anchor = XY.Lerp (_anchor, Position, Settings.EntityAnchorLerpCoefficient);
                if (XY.SqrDistance (_anchor, Position) > Settings.EntityAnchorSqrDistance) {
                    IdleAt = _.War.Time + Settings.EntityIdleTime;
                }
            }

            // пересчет перекрываний
            Excluded.Clear ();
            Excluded.Add (this);
            ExcludeEntities ();
        }


        private Collision NextCollision () {
            var v = Velocity * MP;

            var cObj = CollideWithObjects (v);
            if (cObj != null) v = cObj.Offset;

            var cLand = CollideWithLand (v);
            return cLand ?? cObj;
        }


        private Collision CollideWithLand (XY v) {
            Collision min = null;
            foreach (var c in _colliders) {
                var temp = c.FlyInto (_.War.Land, v);
                if (temp < min) min = temp;
            }
            return min;
        }


        private Collision CollideWithObjects (XY v) {
            Collision min = null;
            foreach (var c in _colliders)
            foreach (var o in _.War.CollisionSystem.FindObstacles (c, v, ObstacleFilter)) {
                var temp = c.FlyInto (o, v);
                if (temp < min) min = temp;
            }
            return min;
        }


        protected bool ObstacleFilter (Collider obstacle) {
            return !obstacle.Entity.PassableFor (this) && !Excluded.Contains (obstacle.Entity);
        }


        private void PokeTiles (bool ignoreTimer) {
            var box = new Box (
                float.PositiveInfinity,
                float.NegativeInfinity,
                float.PositiveInfinity,
                float.NegativeInfinity
            );
            foreach (var collider in _colliders) {
                var cbox = collider.Box;
                if (cbox.X0 < box.X0) box.X0 = cbox.X0;
                if (cbox.X1 > box.X1) box.X1 = cbox.X1;
                if (cbox.Y0 < box.Y0) box.Y0 = cbox.Y0;
                if (cbox.Y1 > box.Y1) box.Y1 = cbox.Y1;
            }
            _.War.CollisionSystem.PokeTiles (box, this, ignoreTimer);
        }


        // public только для системы коллизий
        public void Poke (MobileEntity sender, bool ignoreTimer) {
            if (sender.PassableFor (this)) return;
            if (ignoreTimer) {
                Poke ();
                return;
            }
            if (IdleAt < sender.IdleAt) {
                if (sender.IdleAt - _.War.Time > Settings.EntityIdleTime) {
                    throw new InvalidOperationException ("Ошибка в MobileEntity.Poke (MobileEntity, bool).");
                }
                IdleAt = sender.IdleAt;
            }
        }


        public void Poke () {
            IdleAt = _.War.Time + Settings.EntityIdleTime;
        }

    }

}