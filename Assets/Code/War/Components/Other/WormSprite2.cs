using System;
using UnityEngine;


namespace War.Components.Other {


    public enum WormAnimation {
        Stand,
        Walk,
        BeforeJump,
        Jump,
        AfterJump,
        Fly,
        Spin,
        Slide,
        Recover,
        Death,
    }

    /**
     * контроллер сложного спрайта червяка по типу аниматора юнити,
     * но не привязанный к встроенной временной шкале
     */
    public class WormSprite2 : MonoBehaviour {
        
        /* слои спрайтов червяка
         *
         *  -50  крылья (сзади)
         *  -40  хвост
         *  -30  крылья (спереди)
         *  -20  рога (сзади), шляпа (сзади)
         *  -10  волосы (сзади)
         * 
         *    0  голова
         *   10  глаза
         *   20  зрачки
         *   30  веки
         * 
         *   40  рога (та часть которая закрыта прической, хотя возможно надо ее выключать)
         *   50  волосы (спереди)
         *   60  рога (спереди), шляпа (спереди)
         *   70  брови (отключать при несовместимой шляпе)
         *   80  хвост (на анимации когда спереди)
         *
         *   90  взрывалка
         *
         *
         * анимации червяка
         *     стоит на месте - может моргать, поворот головы и хвоста может отличаться
         *     ползет         - хвост анимированный, поворот головы и хвоста может отличаться
         *     перед прыжком, после прыжка
         *                    - голова и хвост анимированные, положение хвоста может отличаться
         *     в прыжке, летит, крутится, скользит, встает, умирает
         *                    - цельная анимация, у вещей другие координаты
         *
         *
         * изменяемые штуки:
         *     root   .rotation
         *     root   .scale
         *     sprites.position
         *     head   .rotation
         *     head   .scale
         *     head   .sprite
         *     tail   .rotation
         *     tail   .scale
         *     tail   .sprite
         *     hat    .position
         *     hat    .rotation
         *     wings  .position
         *     wings  .rotation
         */


        [HideInInspector] public WormAnimation Animation;
        [HideInInspector] public bool          FacesRight;
        [HideInInspector] public float         HeadRotation; // NaN - червяк смотрит туда куда повернут
        [HideInInspector] public float         Rotation;     // используется когда червяк летит от взрыва или удара
        [HideInInspector] public int           Frame;


        [SerializeField] private Transform
            _root,
            _offset,
            _head,
            _tail,
            _hat,
            _wings;

        [SerializeField] private SpriteRenderer
            _headRenderer,
            _tailRenderer,
            _eyesRenderer,
            _browsRenderer,
            _pupilsRenderer,
            _tntRenderer;

        [Space]
        [SerializeField] private Sprite _headSprite;
        [SerializeField] private Sprite _tailSprite;
        [SerializeField] private Sprite _spinSprite;
        [SerializeField] private Sprite _eyesSprite;
        [SerializeField] private Sprite _browsSprite;
        [SerializeField] private Sprite _pupilsSprite;

        [SerializeField] private Sprite []
            _walkTailSprites,
            _walkEyesSprites,
            _walkBrowsSprites,
            _walkPupilsSprites,
            
            _jumpHeadSprites,
            _jumpEyesSprites,
            _jumpBrowsSprites,
            _jumpPupilsSprites,
            
            _beforeAfterJumpHeadSprites,
            _beforeAfterJumpEyesSprites,
            _beforeAfterJumpBrowsSprites,
            _beforeAfterJumpPupilsSprites,
            
            _flyHeadSprites,
            _flyEyesSprites,
            _flyBrowsSprites,
            _flyPupilsSprites,
            
            _slideHeadSprites,
            _slideTailSprites,
            _slideEyesSprites,
            _slideBrowsSprites,
            _slidePupilsSprites,
            
            _recoverHeadSprites,
            _recoverTailSprites,
            _recoverEyesSprites,
            _recoverBrowsSprites,
            _recoverPupilsSprites,
            
            _deathHeadSprites,
            _deathEyesSprites,
            _deathBrowsSprites,
            _deathPupilsSprites,
            _deathTntSprites;

        private static readonly Vector3
            _standSpritesPosition   = new Vector2 ( 3,    -8),
            _jumpSpritesPosition    = new Vector2 ( 3,    -6),
            _flySpritesPosition     = new Vector2 (-1.5f, -1),
            _spinSpritesPosition    = new Vector2 (-1.5f, -7),
            _slideSpritesPosition   = new Vector2 ( 1.5f, -7),
            _recoverSpritesPosition = new Vector2 ( 1.5f, -7),
            _deathSpritesPosition   = new Vector2 ( 2,    -6);

        private static readonly Vector3 []
            _jumpThingsPositions    = new Vector3 [3] {
                new Vector3 (-0.71f, -0.5f , 0),
                new Vector3 (-0.89f, -0.6f , 0),
                new Vector3 (-1.08f, -0.71f, 0),
            },
            _flyThingsPositions     = new Vector3 [3] {
                new Vector3 (10.1f,  1.7f , 0),
                new Vector3 ( 9.3f,  0.35f, 0),
                new Vector3 ( 8.5f, -1    , 0),
            },
//            _spinThingsPositions    = {},
            _slideThingsPositions   = new Vector3 [3] {
                new Vector3 (0.88f,  0.21f, 0),
                new Vector3 (0.75f, -0.1f , 0),
                new Vector3 (0.58f, -0.39f, 0),
            },
            _recoverThingsPositions = new Vector3 [30] {
                new Vector3 ( 0.4f ,  0    , 0),
                new Vector3 (-0.28f, -0.4f , 0),
                new Vector3 (-0.9f , -0.78f, 0),
                new Vector3 (-1.57f, -1.21f, 0),
                new Vector3 (-2.19f, -1.63f, 0),
                new Vector3 (-2.89f, -2.02f, 0),
                new Vector3 (-3.54f, -2.4f , 0),
                new Vector3 (-4.21f, -2.77f, 0),
                new Vector3 (-4.88f, -3.15f, 0),
                new Vector3 (-5.65f, -2.48f, 0),

                new Vector3 (-3.66f,  0.91f, 0),
                new Vector3 (-1.69f,  4.26f, 0),
                new Vector3 ( 0.17f,  7.66f, 0),
                new Vector3 (-0.13f, 10.41f, 0),
                new Vector3 (-0.24f, 11.3f , 0),
                new Vector3 (-0.33f, 12.13f, 0),
                new Vector3 (-0.23f, 12.52f, 0),
                new Vector3 (-0.63f, 12.24f, 0),
                new Vector3 (-1.05f, 11.89f, 0),
                new Vector3 (-1.58f, 11.33f, 0),

                new Vector3 (-2.1f , 10.8f , 0),
                new Vector3 (-3.06f, 10    , 0),
                new Vector3 (-3.37f,  9.77f, 0),
                new Vector3 (-3.53f,  9.47f, 0),
                new Vector3 (-3.25f,  9    , 0),
                new Vector3 (-2.95f,  8.56f, 0),
                new Vector3 (-3.01f,  7.9f , 0),
                new Vector3 (-2.79f,  6.48f, 0),
                new Vector3 (-2.54f,  4.88f, 0),
                new Vector3 (-2.39f,  2.85f, 0),
            },
            _deathThingsPositions = new Vector3 [33] {
                new Vector3 (-0.42f,  2.6f , 0),
                new Vector3 (-0.83f,  8.74f, 0),
                new Vector3 (-0.76f, 14.22f, 0),
                new Vector3 (-1.03f, 15.95f, 0),
                new Vector3 (-1.52f, 13.29f, 0),
                new Vector3 (-1.36f,  9.68f, 0),
                new Vector3 (-2.8f ,  7.27f, 0),
                new Vector3 (-3.19f,  4.22f, 0),
                new Vector3 (-2.55f,  6.05f, 0),
                new Vector3 (-1.96f,  7.91f, 0),
                
                new Vector3 (-1.4f ,  9.76f, 0),
                new Vector3 (-1.4f ,  9.75f, 0),
                new Vector3 (-1.38f,  9.75f, 0),
                new Vector3 (-1.38f,  9.75f, 0),
                new Vector3 (-1.39f,  9.75f, 0),
                new Vector3 (-1.4f , 10    , 0),
                new Vector3 (-2.35f, 10.3f , 0),
                new Vector3 (-3.19f, 10.52f, 0),
                new Vector3 (-3.58f, 10.77f, 0),
                new Vector3 (-3.5f , 11.07f, 0),
                
                new Vector3 (-2.81f, 10.16f, 0),
                new Vector3 (-2.2f ,  8.94f, 0),
                new Vector3 (-1.71f,  7.26f, 0),
                new Vector3 (-1.72f,  5.42f, 0),
                new Vector3 (-1.89f,  4.65f, 0),
                new Vector3 (-2.35f,  6.09f, 0),
                new Vector3 (-2.43f,  7.74f, 0),
                new Vector3 (-2.51f,  9.43f, 0),
                new Vector3 (-2.55f, 11.12f, 0),
                new Vector3 (-2.6f , 12.74f, 0),
                
                new Vector3 (-2.69f, 14.37f, 0),
                new Vector3 (-2.72f, 16.01f, 0),
                new Vector3 (-2.78f, 17.62f, 0),
            };

        private static readonly Quaternion []
            _jumpThingsRotations    = new Quaternion [3] {
                Quaternion.Euler (0, 0,  4.2f), 
                Quaternion.Euler (0, 0,  1.8f), 
                Quaternion.Euler (0, 0, -0.6f), 
            },
            _flyThingsRotations     = new Quaternion [3] {
                Quaternion.Euler (0, 0, 54), 
                Quaternion.Euler (0, 0, 42.2f), 
                Quaternion.Euler (0, 0, 30.1f), 
            },
            _spinThingsRotations    = {},
            _slideThingsRotations   = new Quaternion [3] {
                Quaternion.Euler (0, 0, 10.4f), 
                Quaternion.Euler (0, 0,  7.7f), 
                Quaternion.Euler (0, 0,  4.9f), 
            },
            _recoverThingsRotations = new Quaternion [30] {
                Quaternion.Euler (0, 0,   8.4f), 
                Quaternion.Euler (0, 0,   3.4f), 
                Quaternion.Euler (0, 0,  -1.4f), 
                Quaternion.Euler (0, 0,  -6.4f), 
                Quaternion.Euler (0, 0, -11.1f), 
                Quaternion.Euler (0, 0, -16.3f), 
                Quaternion.Euler (0, 0, -21.2f), 
                Quaternion.Euler (0, 0, -26.1f), 
                Quaternion.Euler (0, 0, -30.9f), 
                Quaternion.Euler (0, 0, -36.2f),
                
                Quaternion.Euler (0, 0, -21.4f), 
                Quaternion.Euler (0, 0,  -7   ), 
                Quaternion.Euler (0, 0,   7.5f), 
                Quaternion.Euler (0, 0,  12.5f), 
                Quaternion.Euler (0, 0,  13.2f), 
                Quaternion.Euler (0, 0,  14   ), 
                Quaternion.Euler (0, 0,  14.6f), 
                Quaternion.Euler (0, 0,  11.5f), 
                Quaternion.Euler (0, 0,   3.2f), 
                Quaternion.Euler (0, 0,  -7.1f),
                
                Quaternion.Euler (0, 0, -17.2f), 
                Quaternion.Euler (0, 0, -31.6f), 
                Quaternion.Euler (0, 0, -39.6f), 
                Quaternion.Euler (0, 0, -41.3f), 
                Quaternion.Euler (0, 0, -38.8f), 
                Quaternion.Euler (0, 0, -36.3f), 
                Quaternion.Euler (0, 0, -31.2f), 
                Quaternion.Euler (0, 0, -25.8f), 
                Quaternion.Euler (0, 0, -23.1f), 
                Quaternion.Euler (0, 0, -23   ),
            },
            _deathThingsRotations = new Quaternion[33] {
                Quaternion.Euler (0, 0, -19.3f),
                Quaternion.Euler (0, 0, -12.2f),
                Quaternion.Euler (0, 0,  -5.5f),
                Quaternion.Euler (0, 0,  -1.3f),
                Quaternion.Euler (0, 0,  -0.4f),
                Quaternion.Euler (0, 0,   0.1f),
                Quaternion.Euler (0, 0, -10.3f),
                Quaternion.Euler (0, 0, -16   ),
                Quaternion.Euler (0, 0, -10.7f),
                Quaternion.Euler (0, 0,  -5.3f),
                
                Quaternion.identity,
                Quaternion.identity,
                Quaternion.identity,
                Quaternion.identity,
                Quaternion.identity,
                Quaternion.Euler (0, 0,   2.5f),
                Quaternion.Euler (0, 0,   5   ),
                Quaternion.Euler (0, 0,   7.5f),
                Quaternion.Euler (0, 0,  10.2f),
                Quaternion.Euler (0, 0,  12   ),
                
                Quaternion.Euler (0, 0,   1.6f),
                Quaternion.Euler (0, 0,  -9   ),
                Quaternion.Euler (0, 0, -20.5f),
                Quaternion.Euler (0, 0, -32.3f),
                Quaternion.Euler (0, 0, -44   ),
                Quaternion.Euler (0, 0, -46.9f),
                Quaternion.Euler (0, 0, -49.7f),
                Quaternion.Euler (0, 0, -52.6f),
                Quaternion.Euler (0, 0, -55.5f),
                Quaternion.Euler (0, 0, -58.2f),
                
                Quaternion.Euler (0, 0, -61.2f),
                Quaternion.Euler (0, 0, -64.1f),
                Quaternion.Euler (0, 0, -66.9f),
            };


        public void DoUpdate () {
            switch (Animation) {
                case WormAnimation.Stand     : UpdateStand      (); break;
                case WormAnimation.Walk      : UpdateWalk       (); break;
                case WormAnimation.BeforeJump: UpdateBeforeJump (); break;
                case WormAnimation.Jump      : UpdateJump       (); break;
                case WormAnimation.AfterJump : UpdateAfterJump  (); break;
                case WormAnimation.Fly       : UpdateFly        (); break;
                case WormAnimation.Spin      : UpdateSpin       (); break;
                case WormAnimation.Slide     : UpdateSlide      (); break;
                case WormAnimation.Recover   : UpdateRecover    (); break;
                case WormAnimation.Death     : UpdateDeath      (); break;
                default: throw new ArgumentOutOfRangeException ();
            }
        }
        
        
        private void UpdateStand () {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _standSpritesPosition;
            if (float.IsNaN (HeadRotation)) {
                _head.localRotation = Quaternion.identity;
                _head.localScale    = Vector3.one;
            }
            else {
                float sin  = Mathf.Sin (HeadRotation);
                bool  flip = Mathf.RoundToInt (HeadRotation / Mathf.PI) % 2 == 0 ^ FacesRight;
                _head.localRotation = Quaternion.Euler (0, 0, (flip ? -40 : 40) * sin);
                _head.localScale    = new Vector3 (flip ? -1 : 1, 1, 1);
            }
            _headRenderer  .sprite        = _headSprite;
            _tail          .localRotation = Quaternion.Euler (0, 0, 0); //tailRotation);
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _tailSprite;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        private void UpdateWalk () {
            int frame = (Frame + 16) % 22;
            if (frame > 11) frame = 22 - frame;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _standSpritesPosition;
            if (float.IsNaN (HeadRotation)) {
                _head.localRotation = Quaternion.identity;
                _head.localScale    = Vector3.one;
            }
            else {
                float sin  = Mathf.Sin (HeadRotation);
                bool  flip = Mathf.RoundToInt (HeadRotation / Mathf.PI) % 2 == 0 ^ FacesRight;
                _head.localRotation = Quaternion.Euler (0, 0, (flip ? -40 : 40) * sin);
                _head.localScale    = new Vector3 (flip ? -1 : 1, 1, 1);
            }
            _headRenderer  .sprite        = _headSprite;
            _tail          .localRotation = Quaternion.Euler (0, 0, 0); //tailrotation
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _walkTailSprites [frame];
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        private void UpdateBeforeJump () {
            int frame = Frame / 2;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _standSpritesPosition;
            _head          .localRotation = Quaternion.Euler (0, 0, -25f / 7 * frame);
            _head          .localScale    =
                new Vector3 (Mathf.LerpUnclamped (1, 1.3f, frame / 7f), Mathf.LerpUnclamped (1, 0.7f, frame / 7f), 1);
            _headRenderer  .sprite        = _beforeAfterJumpHeadSprites [frame];
            _tail          .localRotation = Quaternion.Euler (0, 0, 0); //...
            _tail          .localScale    = new Vector3(Mathf.LerpUnclamped (1, 1.2f, frame / 7f), 1, 1);
            _tailRenderer  .sprite        = _tailSprite;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        private void UpdateJump () {
            int frame = Frame / 3 % 4;
//          if (frame < 0) frame += 4;
            if (frame > 2) frame = 4 - frame;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _jumpSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _jumpHeadSprites     [frame];
//          _tail          .localRotation = ...;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _jumpEyesSprites     [frame];
            _browsRenderer .sprite        = _jumpBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _jumpPupilsSprites   [frame];
            _hat           .localPosition = _jumpThingsPositions [frame];
            _hat           .localRotation = _jumpThingsRotations [frame];
            _wings         .localPosition = _jumpThingsPositions [frame];
            _wings         .localRotation = _jumpThingsRotations [frame];
        }


        private void UpdateAfterJump () {
            // копия UpdateBeforeJump, кроме формулы для кадра
            int frame = 7 - Frame / 2;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _standSpritesPosition;
            _head          .localRotation = Quaternion.Euler (0, 0, -25f / 7 * frame);
            _head          .localScale =
            new Vector3 (Mathf.LerpUnclamped (1, 1.3f, frame / 7f), Mathf.LerpUnclamped (1, 0.7f, frame / 7f), 1);
            _headRenderer  .sprite        = _beforeAfterJumpHeadSprites [frame];
            _tail          .localRotation = Quaternion.Euler (0, 0, 0); //...
            _tail          .localScale    = new Vector3(Mathf.LerpUnclamped (1, 1.2f, frame / 7f), 1, 1);
            _tailRenderer  .sprite        = _tailSprite;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        private void UpdateFly () {
            int frame = Frame / 2 % 4;
            if (frame > 2) frame = 4 - frame;

            _root          .localRotation = Quaternion.Euler (0, 0, Rotation * Mathf.Rad2Deg - 90);
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _flySpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _flyHeadSprites     [frame];
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _flyEyesSprites     [frame];
            _browsRenderer .sprite        = _flyBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _flyPupilsSprites   [frame];
            _hat           .localPosition = _flyThingsPositions [frame];
            _hat           .localRotation = _flyThingsRotations [frame];
            _wings         .localPosition = _flyThingsPositions [frame];
            _wings         .localRotation = _flyThingsRotations [frame];
        }


        private void UpdateSpin () {
            _root          .localRotation =
                Quaternion.Euler (0, 0, Rotation * (FacesRight ? -Mathf.Rad2Deg : Mathf.Rad2Deg));
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _spinSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _spinSprite;
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        private void UpdateSlide () {
            int frame = Frame / 4 % 4;
            if (frame > 2) frame = 4 - frame;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _slideSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _slideHeadSprites     [frame];
            _tail          .localRotation = Quaternion.identity;
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _slideTailSprites     [frame];
            _eyesRenderer  .sprite        = _slideEyesSprites     [frame];
            _browsRenderer .sprite        = _slideBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _slidePupilsSprites   [frame];
            _tailRenderer  .sortingOrder  = 80;
            _hat           .localPosition = _slideThingsPositions [frame];
            _hat           .localRotation = _slideThingsRotations [frame];
            _wings         .localPosition = _slideThingsPositions [frame];
            _wings         .localRotation = _slideThingsRotations [frame];
        }


        private void UpdateRecover () {
            int frame = Frame / 2;
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _recoverSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _recoverHeadSprites     [frame];
            _tail          .localRotation = Quaternion.identity;
            _tail          .localScale    = Vector3.one;
            _tailRenderer.sprite          = null; //_recoverTailSprites     [frame];
            _eyesRenderer  .sprite        = _recoverEyesSprites     [frame];
            _browsRenderer .sprite        = _recoverBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _recoverPupilsSprites   [frame];
            _tailRenderer  .sortingOrder  = 80;
            _hat           .localPosition = _recoverThingsPositions [frame];
            _hat           .localRotation = _recoverThingsRotations [frame];
            _wings         .localPosition = _recoverThingsPositions [frame];
            _wings         .localRotation = _recoverThingsRotations [frame];
        }


        private void UpdateDeath () {
            int frame;
            if      (Frame < 28) frame = Frame / 2;
            else if (Frame < 88) frame = 14;
            else                 frame = (Frame - 58) / 2;
            
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (FacesRight ? 1 : -1, 1, 1);
            _offset       .localPosition = _deathSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _deathHeadSprites     [frame];
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _deathEyesSprites     [frame];
            _browsRenderer .sprite        = _deathBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _deathPupilsSprites   [frame];
            _tntRenderer   .sprite        = _deathTntSprites      [frame];
            _hat           .localPosition = _deathThingsPositions [frame];
            _hat           .localRotation = _deathThingsRotations [frame];
            _wings         .localPosition = _deathThingsPositions [frame];
            _wings         .localRotation = _deathThingsRotations [frame];
        }
        

/*
        public void Stand (bool facesRight, float headRotation, float tailRotation) {
            StandRaw (facesRight, headRotation, tailRotation);
        }


        public void StandRaw (
            bool  facesRight,   // направление червяка
            float headRotation, // поворот головы
            float tailRotation  // поворот хвоста
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _standSpritesPosition;
            float sin  = Mathf.Sin (headRotation);
            bool  flip = Mathf.RoundToInt (headRotation / Mathf.PI) % 2 == 0 ^ facesRight;
            _head          .localRotation = Quaternion.Euler (0, 0, (flip ? -40 : 40) * sin);
            _head          .localScale    = new Vector3 (flip ? -1 : 1, 1, 1);
            _headRenderer  .sprite        = _headSprite;
            _tail          .localRotation = Quaternion.Euler (0, 0, tailRotation);
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _tailSprite;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        public void Walk (bool facesRight, float headRotation, float tailRotation, int frame) {
            frame += 16;
            frame %= 22;
            if (frame > 11) frame = 22 - frame;
            WalkRaw (facesRight, headRotation, tailRotation, frame);
        }


        public void WalkRaw (
            bool  facesRight,   // направление червяка
            float headRotation, // поворот головы
            float tailRotation, // поворот хвоста
            int   frame         // кадр анимации хвоста
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _standSpritesPosition;
            float sin  = Mathf.Sin (headRotation);
            bool  flip = Mathf.RoundToInt (headRotation / Mathf.PI) % 2 == 0 ^ facesRight;
            _head          .localRotation = Quaternion.Euler (0, 0, (flip ? -40 : 40) * sin);
            _head          .localScale    = new Vector3 (flip ? -1 : 1, 1, 1);
            _headRenderer  .sprite        = _headSprite;
            _tail          .localRotation = Quaternion.Euler (0, 0, tailRotation);
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _walkTailSprites [frame];
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        public void BeforeJump (bool facesRight, float headRotation, float tailRotation, int frame) {
            BeforeJumpRaw (facesRight, headRotation, tailRotation, frame / 2);
        }


        public void BeforeJumpRaw (
            bool  facesRight,   // направление червяка
            float headRotation, // поворот головы
            float tailRotation, // поворот хвоста
            int   frame         // кадр анимации 0 - 7
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _standSpritesPosition;
            _head          .localRotation = Quaternion.Euler (0, 0, -25f / 7 * frame);
            _head          .localScale    =
            new Vector3 (Mathf.LerpUnclamped (1, 1.3f, frame / 7f), Mathf.LerpUnclamped (1, 0.7f, frame / 7f), 1);
            _headRenderer  .sprite        = _beforeAfterJumpHeadSprites [frame];
            _tail          .localRotation = Quaternion.Euler (0, 0, tailRotation);
            _tail          .localScale    = new Vector3(Mathf.LerpUnclamped (1, 1.2f, frame / 7f), 1, 1);
            _tailRenderer  .sprite        = _tailSprite;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _tailRenderer  .sortingOrder  = -40;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }
        
        
        public void AfterJump (bool facesRight, float headRotation, float tailRotation, int frame) {
            AfterJumpRaw (facesRight, headRotation, tailRotation, frame / 2);
        }


        public void AfterJumpRaw (
            bool  facesRight,   // направление червяка
            float headRotation, // поворот головы
            float tailRotation, // поворот хвоста
            int   frame         // кадр анимации: 0 - 7
        ) {
            BeforeJump (facesRight, headRotation, tailRotation, 7 - frame);
        }


        public void Jump (bool facesRight, int frame) {
            frame /= 3;
            frame %= 4;
//            if (frame < 0) frame += 4;
            if (frame > 2) frame = 4 - frame;
            JumpRaw (facesRight, frame);
        }


        public void JumpRaw (
            bool facesRight, // направление червяка
            int  frame       // кадр анимации
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _jumpSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _jumpHeadSprites     [frame];
//          _tail          .localRotation = ...;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _jumpEyesSprites     [frame];
            _browsRenderer .sprite        = _jumpBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _jumpPupilsSprites   [frame];
            _hat           .localPosition = _jumpThingsPositions [frame];
            _hat           .localRotation = _jumpThingsRotations [frame];
            _wings         .localPosition = _jumpThingsPositions [frame];
            _wings         .localRotation = _jumpThingsRotations [frame];
        }


        public void Fly (bool facesRight, float rotation, int frame) {
            frame /= 2;
            frame %= 4;
            if (frame > 2) frame = 4 - frame;
            float degrees = rotation * Mathf.Rad2Deg - 90;
            FlyRaw (facesRight, degrees, frame);
        }


        public void FlyRaw (
            bool  facesRight, // направление червяка
            float rotation,   // поворот
            int   frame       // кадр анимации
        ) {
            _root          .localRotation = Quaternion.Euler (0, 0, rotation);
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _flySpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _flyHeadSprites [frame];
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _flyEyesSprites     [frame];
            _browsRenderer .sprite        = _flyBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _flyPupilsSprites   [frame];
            _hat           .localPosition = _flyThingsPositions [frame];
            _hat           .localRotation = _flyThingsRotations [frame];
            _wings         .localPosition = _flyThingsPositions [frame];
            _wings         .localRotation = _flyThingsRotations [frame];
        }


        public void Spin (bool facesRight, float rotation) {
            SpinRaw (facesRight, rotation * (facesRight ? -Mathf.Rad2Deg : Mathf.Rad2Deg));
        }


        public void SpinRaw (
            bool  facesRight, // направление червяка
            float rotation    // поворот
        ) {
            _root          .localRotation = Quaternion.Euler (0, 0, rotation);
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _spinSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _spinSprite;
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _eyesSprite;
            _browsRenderer .sprite        = _browsSprite;
            _pupilsRenderer.sprite        = _pupilsSprite;
            _hat           .localPosition = Vector3.zero;
            _hat           .localRotation = Quaternion.identity;
            _wings         .localPosition = Vector3.zero;
            _wings         .localRotation = Quaternion.identity;
        }


        public void Slide (bool facesRight, int frame) {
            frame /= 4;
            frame %= 4;
            if (frame > 2) frame = 4 - frame;
            SlideRaw (facesRight, frame);
        }


        public void SlideRaw (
            bool  facesRight, // направление червяка
            int   frame       // кадр анимации
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _slideSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _slideHeadSprites     [frame];
            _tail          .localRotation = Quaternion.identity;
            _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = _slideTailSprites     [frame];
            _eyesRenderer  .sprite        = _slideEyesSprites     [frame];
            _browsRenderer .sprite        = _slideBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _slidePupilsSprites   [frame];
            _tailRenderer  .sortingOrder  = 80;
            _hat           .localPosition = _slideThingsPositions [frame];
            _hat           .localRotation = _slideThingsRotations [frame];
            _wings         .localPosition = _slideThingsPositions [frame];
            _wings         .localRotation = _slideThingsRotations [frame];
        }


        public void Recover (bool facesRight, int frame) => RecoverRaw (facesRight, frame / 2);


        public void RecoverRaw (
            bool  facesRight, // направление червяка
            int   frame       // кадр анимации 0 - 29
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _recoverSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _recoverHeadSprites     [frame];
            _tail          .localRotation = Quaternion.identity;
            _tail          .localScale    = Vector3.one;
            _tailRenderer.sprite = null;//_recoverTailSprites     [frame];
            _eyesRenderer  .sprite        = _recoverEyesSprites     [frame];
            _browsRenderer .sprite        = _recoverBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _recoverPupilsSprites   [frame];
            _tailRenderer  .sortingOrder  = 80;
            _hat           .localPosition = _recoverThingsPositions [frame];
            _hat           .localRotation = _recoverThingsRotations [frame];
            _wings         .localPosition = _recoverThingsPositions [frame];
            _wings         .localRotation = _recoverThingsRotations [frame];
        }


        public void Death (bool facesRight, int frame) {
            if      (frame < 28) frame /= 2;
            else if (frame < 88) frame = 14;
            else                 frame = (frame - 58) / 2;
            
            DeathRaw (facesRight, frame);
        }


        public void DeathRaw (
            bool  facesRight, // направление червяка
            int   frame       // кадр анимации 0 - 32
        ) {
            _root          .localRotation = Quaternion.identity;
            _root          .localScale    = new Vector3 (facesRight ? 1 : -1, 1, 1);
            _sprites       .localPosition = _deathSpritesPosition;
            _head          .localRotation = Quaternion.identity;
            _head          .localScale    = Vector3.one;
            _headRenderer  .sprite        = _deathHeadSprites     [frame];
//          _tail          .localRotation = ...;
//          _tail          .localScale    = Vector3.one;
            _tailRenderer  .sprite        = null;
            _eyesRenderer  .sprite        = _deathEyesSprites     [frame];
            _browsRenderer .sprite        = _deathBrowsSprites    [frame];
            _pupilsRenderer.sprite        = _deathPupilsSprites   [frame];
            _tntRenderer   .sprite        = _deathTntSprites      [frame];
            _hat           .localPosition = _deathThingsPositions [frame];
            _hat           .localRotation = _deathThingsRotations [frame];
            _wings         .localPosition = _deathThingsPositions [frame];
            _wings         .localRotation = _deathThingsRotations [frame];
        }
*/

    }

}
