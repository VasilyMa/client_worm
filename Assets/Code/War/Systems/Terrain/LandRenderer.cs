using Math;
using UnityEngine;


namespace War.Systems.Terrain {

    public class LandRenderer : MonoBehaviour {

        [SerializeField] private Texture2D _template;

        private Texture2D _texture;
        private Color []  _tempArray;
        private int       _w, _h;
        private int       _blockX, _blockY, _blockW, _blockH;
        private const int _textureSize = 2048;


        public void Init (byte [,] array) {
            _w = array.GetLength (0);
            _h = array.GetLength (1);

            var template = _template.GetPixels ();

            var result = new Color [_w * _h];
            for (int x = 0; x < _w; x++)
            for (int y = 0; y < _h; y++) {
                result [x + y * _w] = array [x, y] == 0 ? Color.clear : template [x % _textureSize + y % _textureSize * _textureSize];
                // 256 это размер текстуры
            }

            _texture = new Texture2D (_w, _h, TextureFormat.RGBA32, false) {wrapMode = TextureWrapMode.Clamp};
            _texture.SetPixels (result);
            _texture.Apply ();

            gameObject.AddComponent <SpriteRenderer> ().
            sprite = Sprite.Create (_texture, new Rect (0, 0, _w, _h), Vector2.zero, 1, 0, SpriteMeshType.FullRect);

        }


        public Color GetPixel (int x, int y) {
            x -= _blockX;
            y -= _blockY;
            return _tempArray [x + y * _blockW];
        }


        public void SetPixel (int x, int y, Color color) {
            x -= _blockX;
            y -= _blockY;
            _tempArray [x + y * _blockW] = color;
        }


        public void ApplyBlock () {
//            if (_tempArray == null) return;
            _texture.SetPixels (_blockX, _blockY, _blockW, _blockH, _tempArray);
            _texture.Apply ();
            _tempArray = null;
        }


        public void PrepareBlock (IntBox box) {
            _blockW = box.X1 - (_blockX = box.X0);
            _blockH = box.Y1 - (_blockY = box.Y0);
            _tempArray = _texture.GetPixels (_blockX, _blockY, _blockW, _blockH);
        }


        public void ApplyWhole () {
            _texture.SetPixels (_tempArray);
            _texture.Apply ();
            _tempArray = null;
        }


        public void PrepareWhole () {
            _blockX = _blockY = 0;
            _blockW = _w;
            _blockH = _h;
            _tempArray = _texture.GetPixels ();
        }

    }

}