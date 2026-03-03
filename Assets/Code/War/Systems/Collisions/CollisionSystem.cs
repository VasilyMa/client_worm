using System;
using System.Collections.Generic;
using System.Linq;
using Math;
using War.Components;
using War.Entities;
using War.Entities.Rays;


namespace War.Systems.Collisions {

    public class CollisionSystem {

        private const int TileSize = 20;

        private Dictionary <IntXY, HashSet <Collider>> _tiles = new Dictionary <IntXY, HashSet <Collider>> ();


        private HashSet <Collider> Tile (int x, int y) {
            var key = new IntXY (x, y);
            HashSet <Collider> value;
            if (_tiles.TryGetValue (key, out value)) return value;
            value = new HashSet <Collider> ();
            _tiles.Add (key, value);
            return value;
        }


        private HashSet <Collider> TileOrNull (int x, int y) {
            var key = new IntXY (x, y);
            HashSet <Collider> value;
            _tiles.TryGetValue (key, out value);
            return value;
        }
        

        public void Add (Collider collider) {
            var box = collider.Box.ToTiles (TileSize);
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                Add (collider, x, y);
            }
        }


        private void Add (Collider collider, int x, int y) {
            var tile = Tile (x, y);
            tile.Add (collider);
            collider.AddTile (tile);
        }


        public void AddOverlappingEntities (Collider collider, HashSet <MobileEntity> result) {
            var box = collider.Box.ToTiles (TileSize);

            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = TileOrNull (x, y);
                if (tile == null) continue;
                foreach (var c in tile) {
                    if (!result.Contains (c.Entity) && collider.Overlaps (c)) {
                        result.Add (c.Entity);
                    }
                }
            }
        }


        // мы не делаем разные фильтры для разных лучей хотя с огненным ударом и паяльной лампой возможно надо по хитрому
        public void FindObstaclesMeleeRay (IEnumerable <Collider> colliders, XY v, HashSet <MobileEntity> result) {
            float x0 = float.PositiveInfinity;
            float x1 = float.NegativeInfinity;
            float y0 = float.PositiveInfinity;
            float y1 = float.NegativeInfinity;
            foreach (var c in colliders) {
                var cbox = c.Box;
                x0 = System.Math.Min (x0, cbox.X0);
                x1 = System.Math.Max (x1, cbox.X1);
                y0 = System.Math.Min (y0, cbox.Y0);
                y1 = System.Math.Max (y1, cbox.Y1);
            }
            var box = new Box (x0, x1, y0, y1).Expanded (v).ToTiles (TileSize);
            
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = TileOrNull (x, y);
                if (tile == null) continue;
                foreach (var o in tile.
                    Where (o => o.Entity.MeleeHittable && !result.Contains (o.Entity)).
                    Where (o => colliders.Any (c => c.Overlaps(o) || c.FlyInto (o, v) != null))
                ) {
                    result.Add (o.Entity);
                }
            }
        }


        public HashSet <Collider> FindObstacles (Collider collider, XY v, Func <Collider, bool> filter) {
            var box    = collider.Box.Expanded (v).ToTiles (TileSize);
            var result = new HashSet <Collider> ();

            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = TileOrNull (x, y);
                if (tile == null) continue;
                foreach (var c in tile.Where (filter)) {
                    result.Add (c);
                }
            }
            
            return result;
        }


        // итак, документации нет поэтому я ее напишу
        // что делает метод?
        // тупо обходит все созданные тайлы и с каждого тайла кидает в хешсет коллайдеры через фильтр
        // вот и все
        // разница в том что этот метод для лучей у которых огромное расстояние проверки
        // обходить по координатам 100500 пустых тайлов не круто поэтому мы обходим все существующие
        public HashSet <Collider> FindObstaclesRay (Collider collider, XY v, Func <Collider, bool> filter) {
            return new HashSet <Collider> (_tiles.Values.SelectMany (tile => tile.Where (filter)));
            // todo: отсеивать тайлы                    ^-здесь
            // собственно почему здесь до сих пор остались коллайдер и скорость
        }


        public void PokeTile (int x, int y) {
            var tile = TileOrNull (x, y);
            if (tile == null) return;
            foreach (var c in tile) c.Entity.Poke ();
        }


        public void PokeTiles (Box box, MobileEntity entity, bool ignoreTimer) {
            var intBox = box.ToTiles (TileSize);
            
            for (int x = intBox.X0; x < intBox.X1; x++)
            for (int y = intBox.Y0; y < intBox.Y1; y++) {
                var tile = TileOrNull (x, y);
                if (tile == null) continue;
                foreach (var c in tile) {
                    c.Entity.Poke (entity, ignoreTimer);
                }
            }
        }


        private void Clean () {
            _tiles = _tiles.Where (kv => kv.Value.Any ()).ToDictionary (kv => kv.Key, kv => kv.Value);
        }

    }

}
