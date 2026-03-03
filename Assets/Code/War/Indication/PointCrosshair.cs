using System.Collections.Generic;
using Core;
using UnityEngine;


namespace War.Indication {

    public class PointCrosshair : MonoBehaviour {

        [SerializeField] private GameObject _cornerPrefab;
        [SerializeField] private float      _distance            = 3;
        [SerializeField] private float      _distanceWhenClicked = -3;
        [SerializeField] private float      _distanceWhenSet     = 0;
        [SerializeField] private float      _lockOnDuration      = 0.15f;

        GameObject c1_, c2_,
                   c4_, c3_; // уголки

        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private int        _arrowCount;
        [SerializeField] private float      _width;
        [SerializeField] private float      _speed = 10f;

        private List <GameObject>     _arrows    = new List <GameObject>     ();
        private List <SpriteRenderer> _renderers = new List <SpriteRenderer> ();
        private float                 _x;
        private float                 _currentSpeed;

        private float _lockedOnAt = float.NaN;
        private int   _references = 1;


        private void Awake () {
            c1_ = Instantiate (_cornerPrefab, transform, false);
            c2_ = Instantiate (_cornerPrefab, transform, false);
            c3_ = Instantiate (_cornerPrefab, transform, false);
            c4_ = Instantiate (_cornerPrefab, transform, false);
            
            c1_.transform.localPosition = new Vector3 (-_distance,  _distance);
            c2_.transform.localPosition = new Vector3 ( _distance,  _distance);
            c3_.transform.localPosition = new Vector3 ( _distance, -_distance);
            c4_.transform.localPosition = new Vector3 (-_distance, -_distance);
            
            //c1_.transform.localScale = new Vector3 (1, 1, 1);
            c2_.transform.localScale = new Vector3 (-1,  1, 1);
            c3_.transform.localScale = new Vector3 (-1, -1, 1);
            c4_.transform.localScale = new Vector3 ( 1, -1, 1);

            for (int i = 0; i < _arrowCount; i++) {
                var arrow = Instantiate (_arrowPrefab, transform, false);
                var renderer = arrow.GetComponent <SpriteRenderer> ();
                _arrows.Add (arrow);
                _renderers.Add (renderer);
            }
        }


        public int References {
            get { return _references; }
            set {
                _references = value;
                if (_references <= 0) Destroy (gameObject);
            }
        }


        public void Update () {
            if (!float.IsNaN (_lockedOnAt)) {
                float t = (Time.time - _lockedOnAt) / _lockOnDuration;
                t = Mathf.LerpUnclamped (_distanceWhenClicked, _distanceWhenSet, t > 1 ? 1 : t);
                
                c1_.transform.localPosition = new Vector3 (-t,  t);
                c2_.transform.localPosition = new Vector3 ( t,  t);
                c3_.transform.localPosition = new Vector3 ( t, -t);
                c4_.transform.localPosition = new Vector3 (-t, -t);
            }
            float arrowOffset = _width / _arrowCount;
            _x = Mathf.Repeat (_x + _currentSpeed * Time.deltaTime, arrowOffset);
            // Debug.Log (_x); -- 0
            for (int i = 0; i < _arrowCount; i++) {
                float x = _x + i * arrowOffset - _width * 0.5f;
                _arrows [i].transform.localPosition = new Vector3 (x, 0, 0);
                float scale = Mathf.Clamp01 ((_width * 0.5f - Mathf.Abs (x)) / arrowOffset);
                _arrows [i].transform.localScale = new Vector3 (scale, scale, 1f);
            }
        }


        public void SetLeftToRight () {
            for (int i = 0; i < _arrowCount; i++) {
                _arrows [i].SetActive (true);
                _renderers [i].flipX = true; // спрайт стрелки смотрит не туда
                _currentSpeed        = _speed;
            }
        }


        public void SetRightToLeft () {
            for (int i = 0; i < _arrowCount; i++) {
                _arrows [i].SetActive (true);
                _renderers [i].flipX = false;
                _currentSpeed        = -_speed;
            }
        }


        public void SetNonDirectional () {
            for (int i = 0; i < _arrowCount; i++) _arrows [i].SetActive (false);
            _currentSpeed = 0f;
        }


        public void PlayLockOnAnimation () {
            _lockedOnAt = Time.time;
        }


        public void AbortLockOn () {
            _lockedOnAt = float.NaN;
        }

    }

}
