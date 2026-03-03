using System.Collections.Generic;
using Math;


namespace War.Systems.Terrain {

    public class LandTiles {

        private readonly Dictionary <IntXY, LandTile> _tiles;


        public LandTiles () {
            _tiles = new Dictionary <IntXY, LandTile> (65536);
        }


        public LandTile this [int x, int y] {
            get {
                LandTile tile;
                if (_tiles.TryGetValue (new IntXY (x, y), out tile)) {
                    return tile;
                }
                return _tiles [new IntXY (x, y)] = new LandTile (x, y);
            }
            set { _tiles [new IntXY (x, y)] = value; }
        }

    }

}
