using Core;
using DataTransfer.Data;
using Math;
using TMPro;
using UnityEngine;
using War.Systems.Blasts;
using War.Systems.Updating;
using Time = Utils.Time;


namespace War.Entities.Projectiles {

    // todo: если нет коллайдера то зачем это вообще MobileEntity?
    
    public class GhostGrenade : MobileEntity, IUpdatable {

        private int        _explodesAt;
        private GameObject _gameObject;
        private GameObject _grenadeGameObject;
        private TMP_Text   _text;


        public GhostGrenade (int timer) {
            MassRank    = Balance.GrenadeMassRank;
            _explodesAt = timer;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (_gameObject);
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);

            // без коллайдеров

            _gameObject = new GameObject ("ghost grenade");
            _gameObject.transform.localPosition = (Vector3) Position;
            _grenadeGameObject = Object.Instantiate (_.War.Assets.GhostGrenade, _gameObject.transform, false);
            var canvas  = Object.Instantiate (_.War.Assets.Canvas, _gameObject.transform, false).GetComponent <Canvas> ();
            canvas.worldCamera = Camera.main;
            var area    = Object.Instantiate (_.War.Assets.TextAreaBottom, canvas.transform, false);
            _text       = Object.Instantiate (_.War.Assets.Text24, area.transform, false).GetComponent <TMP_Text> ();
            _text.color = Colors.White;
            _text.text  = Time.ToSeconds (_explodesAt).ToString ();
            
            _explodesAt += _.War.Time;

            PositionChanged += (from, to) => _gameObject.transform.localPosition = (Vector3) to;
        }


        public bool Alive => !Despawned;


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity.Y += WarScene.Gravity;
            if (_.War.Time >= _explodesAt) {
                Despawn ();
                _.War.MakeExplosion (Position);
            }
            else {
                _text.text = Time.CeilToSeconds (_explodesAt - _.War.Time).ToString ();
            }
        }

    }

}