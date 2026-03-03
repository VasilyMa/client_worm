using Core;
using DataTransfer.Data;
using Math;
using TMPro;
using UnityEngine;
using War.Systems.Updating;


namespace War.Entities.Labels {

    public class FallingLabel : Entity, IUpdatable {

        private          int        _spawnedAt;
        private readonly string     _text;
        private readonly Color      _color;
        private          XY         _position;
        private          XY         _velocity;
        private          GameObject _gameObject;


        public FallingLabel (string text, Color color, XY position, XY velocity) {
            _text     = text;
            _color    = color;
            _position = position;
            _velocity = velocity;
        }
        
        
        public bool Alive => !Despawned;


        public override void OnSpawn () {
            _spawnedAt = _.War.Time;
            
            _.War.UpdateSystem.Add (this);

            _gameObject = Object.Instantiate (_.War.Assets.Canvas, (Vector3) _position, Quaternion.identity);
            _gameObject.GetComponent <Canvas> ().worldCamera = Camera.main;

            var text = Object.Instantiate (_.War.Assets.Text24, _gameObject.transform, false).GetComponent <TMP_Text> ();
            text.text = _text;
            text.color = _color;
        }


        protected override void OnDespawn () {
            Object.Destroy (_gameObject);
        }
        
        
        public void Update (TurnData td) {
            int t = _.War.Time - _spawnedAt;
            if (t >= 60) {
                Despawn ();
                return;
            }
            _velocity.Y += WarScene.Gravity;
            _position += _velocity;
            _gameObject.transform.localPosition = (Vector3) _position;
            float size = Mathf.InverseLerp (60, 30, t);
            _gameObject.transform.localScale = new Vector3 (size, size, 1);
        }

    }

}
