using System.Collections.Generic;
using Core;
using Math;
using UnityEngine;


namespace War.Systems.Terrain {

    public partial class Land {

        private byte [,]     _land;
        private LandRenderer _renderer;

        public int       Width         { get; private set; }
        public int       Height        { get; private set; }
        public int       WidthInTiles  { get; private set; }
        public int       HeightInTiles { get; private set; }
        public LandTiles Tiles         { get; private set; }


        public Land (byte [,] land, LandRenderer renderer)
        {
            UnityEngine.Debug.Log("Init land!");
            _land  = land;
            Width  = land.GetLength (0);
            Height = land.GetLength (1);
            InitTiles ();
            _renderer = renderer;
            renderer.Init (land);
        }


        public byte this [int x, int y] => x < 0 || y < 0 || x >= Width || y >= Height ? (byte) 0 : _land [x, y];


        private void InitTiles () {
            WidthInTiles = Width / LandTile.Size;
            if (Width % LandTile.Size != 0) ++WidthInTiles;

            HeightInTiles = Height / LandTile.Size;
            if (Height % LandTile.Size != 0) ++HeightInTiles;

            Tiles = new LandTiles ();
            for (int x = 0; x < WidthInTiles; x++)
            for (int y = 0; y < HeightInTiles; y++) {
                var tile = new LandTile (x, y);
                tile.Recalculate (this);
                Tiles [x, y] = tile;
            }
        }


        public void BeginRaw () => _renderer.PrepareWhole ();
        public void EndRaw   () => _renderer.ApplyWhole ();


        public void DestroyTerrainRaw (XY o, float r, HashSet <LandTile> result) {
            float r2 = r * r;

            // мы убираем пиксель, если в круг попадет его центр, а не угол, поэтому применяем смещение
            o -= new XY (0.5f, 0.5f);

            // отличие от обычной версии в отсутствии работы с блоками
            var boxf = new Box (o.X - r, o.X + r, o.Y - r, o.Y + r);

            var box = IntBox.Intersection (
                boxf.ToTiles (1),
                new IntBox (0, Width, 0, Height)
            );
            
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                if (XY.SqrDistance (o, new XY (x, y)) > r2) continue;
                _land [x, y] = 0;
                _renderer.SetPixel (x, y, Color.clear);
            }

            // здесь вроде бы начинается попытка обхода тайлов с сохраненными вершинами
            box = IntBox.Intersection (
                boxf.ToTiles (LandTile.Size),
                new IntBox (0, WidthInTiles, 0, HeightInTiles)
            );

            // обработка тайлов на изменение выпуклых вершин
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = Tiles [x, y];

                if (tile.Land == 0 && tile.Vertices.Count == 0) continue;

                // _.OldWar.World.Tiles [x, y].Poke ();
                _.War.CollisionSystem.PokeTile (x, y);

                // углы клетки проверяем, если угол не находится внутри круга то счетчик увеличивается
                byte temp = 0;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   (y + 1) * LandTile.Size)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, (y + 1) * LandTile.Size)) > r2) temp++;

                // неправильно - проверки углов недостаточно чтобы сказать что тайл не задет
//                if (temp == 4 && holeLargeEnough) continue;

                // полностью в круге взрыва
                if (temp == 0) {
                    tile.Erase ();
                    result.Remove (tile);
                    continue;
                }

                // частично задет
                // tile.Recalculate (this);
                result.Add (tile);
            }
        }
        

        public void DestroyTerrain (XY o, float r) {
            float r2 = r * r;

            // мы убираем пиксель, если в круг попадет его центр, а не угол, поэтому применяем смещение
            o -= new XY (0.5f, 0.5f);

            var boxf = new Box (o.X - r, o.X + r, o.Y - r, o.Y + r);

            var box = IntBox.Intersection (
                boxf.ToTiles (1),
                new IntBox (0, Width, 0, Height)
            );

            if (box.X0 < box.X1 && box.Y0 < box.Y1) {
                _renderer.PrepareBlock (box);
                for (int x = box.X0; x < box.X1; x++)
                for (int y = box.Y0; y < box.Y1; y++) {
                    if (XY.SqrDistance (o, new XY (x, y)) > r2) continue;
                    _land [x, y] = 0;
                    _renderer.SetPixel (x, y, Color.clear);
                }
                _renderer.ApplyBlock (); // todo - при обработке огоньков тормоза тут
            }

            box = IntBox.Intersection (
                boxf.ToTiles (LandTile.Size),
                new IntBox (0, WidthInTiles, 0, HeightInTiles)
            );

            // обработка тайлов на изменение выпуклых вершин
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = Tiles [x, y];

                if (tile.Land == 0 && tile.Vertices.Count == 0) continue;

                // _.OldWar.World.Tiles [x, y].Poke ();
                _.War.CollisionSystem.PokeTile (x, y);

                // углы клетки проверяем, если угол не находится внутри круга то счетчик увеличивается
                byte temp = 0;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   (y + 1) * LandTile.Size)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, (y + 1) * LandTile.Size)) > r2) temp++;

                // неправильно - проверки углов недостаточно чтобы сказать что тайл не задет
//                if (temp == 4 && holeLargeEnough) continue;

                // полностью в круге взрыва
                if (temp == 0) {
                    tile.Erase ();
                    continue;
                }

                // частично задет
                tile.Recalculate (this);
            }
        }


        public void CreateTerrain (XY o, int r) {
            float r2 = r * r;

            // мы убираем пиксель, если в круг попадет его центр, а не угол, поэтому применяем смещение
            o -= new XY (0.5f, 0.5f);

            var boxf = new Box (o.X - r, o.X + r, o.Y - r, o.Y + r);

            var box = IntBox.Intersection (
                boxf.ToTiles (1),
                new IntBox (0, Width, 0, Height)
            );

            if (box.X0 < box.X1 && box.Y0 < box.Y1) {
                _renderer.PrepareBlock (box);
                for (int x = box.X0; x < box.X1; x++)
                for (int y = box.Y0; y < box.Y1; y++) {
                    if (XY.SqrDistance (o, new XY (x, y)) > r2) continue;
                    _land [x, y] = 1;
                    var color = _renderer.GetPixel (x, y);
                    color = color.a > 0 ? Color.LerpUnclamped (color, Color.cyan, 0.6f) : new Color (0, 1, 1, 0.6f);
                    _renderer.SetPixel (x, y, color);
                }
                _renderer.ApplyBlock (); // todo - при обработке огоньков тормоза тут
            }

            box = IntBox.Intersection (
                boxf.ToTiles (LandTile.Size),
                new IntBox (0, WidthInTiles, 0, HeightInTiles)
            );

            // обработка тайлов на изменение выпуклых вершин
            for (int x = box.X0; x < box.X1; x++)
            for (int y = box.Y0; y < box.Y1; y++) {
                var tile = Tiles [x, y];

                if (tile.Land == 0 && tile.Vertices.Count == 0) continue;

                // _.OldWar.World.Tiles [x, y].Poke ();
                // _.War.CollisionSystem.PokeTile (x, y); - нет смысла делать потому что земля не стирается
                
                // углы клетки проверяем, если угол не находится внутри круга то счетчик увеличивается
                byte temp = 0;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, y * LandTile.Size - 1)) > r2) temp++;
                if (XY.SqrDistance (o, new XY (x * LandTile.Size - 1,   (y + 1) * LandTile.Size)) > r2) temp++;
                if (XY.SqrDistance (o, new XY ((x + 1) * LandTile.Size, (y + 1) * LandTile.Size)) > r2) temp++;

                // неправильно - проверки углов недостаточно чтобы сказать что тайл не задет
//                if (temp == 4 && holeLargeEnough) continue;

                // полностью в круге взрыва
                if (temp == 0) {
                    tile.Fill ();
                    continue;
                }

                // частично задет
                tile.Recalculate (this);
            }
        }

    }

}
