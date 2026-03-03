using System.Collections.Generic;
using Core;
using Math;
using War.Entities;
using War.Systems.Collisions;
using War.Systems.Terrain;


namespace War.Components {

    public abstract class Collider : Component <MobileEntity> {

        private List <HashSet <Collider>> _tiles = new List <HashSet <Collider>> ();


        public abstract Box   Box { get; }
        
        public readonly float TangentialBounce, NormalBounce;


        public Collider (float tangentialBounce = 0.9f, float normalBounce = 0.5f) {
            TangentialBounce = tangentialBounce;
            NormalBounce     = normalBounce;
        }


        public void AddTile (HashSet <Collider> tile) {
            _tiles.Add (tile);
        }


        public void ClearTiles () {
            foreach (var tile in _tiles) tile.Remove (this);
            _tiles.Clear ();
        }


        public void UpdatePosition () {
            DoUpdatePosition ();
            ClearTiles ();
            _.War.CollisionSystem.Add (this);
        }


        public override void OnAttach () => DoUpdatePosition ();
        
        protected abstract void DoUpdatePosition ();

        
        public abstract bool Overlaps (Collider       other);
        public abstract bool Overlaps (CircleCollider other);
        public abstract bool Overlaps (BoxCollider    other);
        public abstract bool Overlaps (Land           land );

        public abstract Collision FlyInto (Collider       other, XY v);
        public abstract Collision FlyInto (CircleCollider other, XY v);
        public abstract Collision FlyInto (BoxCollider    other, XY v);
        public abstract Collision FlyInto (Land           land , XY v);

    }

}
