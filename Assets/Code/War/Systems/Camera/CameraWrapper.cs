using Math;
using UnityEngine;


namespace War.Systems.Camera {

    [RequireComponent (typeof (UnityEngine.Camera))]
    public class CameraWrapper : MonoBehaviour { // пока наверно пусть будет компонентом юнити

        public  XY    Target;
        public  float Zoom;
        private XY    _target;
        private float _zoom;
        private float _z;

        private UnityEngine.Camera _camera;
        private CameraController   _controller;


        public CameraController Controller {
            get {
                return _controller;
            }
            set {
                _controller        = value;
                _controller.Camera = this;
                _controller.OnAttach ();
            }
        }


        private void Awake () {
            _camera = GetComponent <UnityEngine.Camera> ();
            var p = transform.localPosition;
            _z     = p.z;
            Target = _target = (XY) p;
            Zoom   = _zoom   = 1;
        }


        private void Update () {
            Controller?.Update ();
            
            _target                         = XY.Lerp (_target, Target, 0.25f);
            _zoom                           = Mathf.Lerp (_zoom, Zoom, 0.25f);
            _camera.transform.localPosition = new Vector3 (_target.X, _target.Y, _z);
            _camera.orthographicSize        = Screen.height * _zoom * 0.5f;
        }


        public XY MouseXY => (XY) _camera.ScreenToWorldPoint (UnityEngine.Input.mousePosition);


        public void HandleWheel (float delta) {
//            Debug.Log (delta);
            Zoom = Mathf.Clamp (Zoom * Mathf.Pow (2, delta / -5), 0.25f, 2);
        }

    }

}
