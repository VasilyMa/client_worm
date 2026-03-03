using Core;
using DataTransfer.Data;
using Math;
using TMPro;
using UnityEngine;
using Utils;
using War.Entities.Smokes;
using War.Systems.Blasts;
using War.Systems.Updating;
using Time = Utils.Time;


namespace War.Entities.Projectiles {

    public class GasGrenade : MobileEntity, IUpdatable, IBlastable {

        private int        _activatesAt;
        private int        _explodesAt;
        
        private GameObject _gameObject;
        private GameObject _grenadeGameObject;
        private TMP_Text   _text;


        public GasGrenade () {
            MassRank    = Balance.GrenadeMassRank;
            _activatesAt = 30;
            _explodesAt = Time.Seconds (5);
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (_gameObject);
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);

            AddCircleCollider (Balance.ShellRadius);

            _gameObject = new GameObject ("gas grenade");
            _gameObject.transform.localPosition = (Vector3) Position;
            _grenadeGameObject = Object.Instantiate (_.War.Assets.Grenade, _gameObject.transform, false);
            var canvas  = Object.Instantiate (_.War.Assets.Canvas, _gameObject.transform, false).GetComponent <Canvas> ();
            canvas.worldCamera = Camera.main;
            var area    = Object.Instantiate (_.War.Assets.TextAreaBottom, canvas.transform, false);
            _text       = Object.Instantiate (_.War.Assets.Text24, area.transform, false).GetComponent <TMP_Text> ();
            _text.color = Colors.White;
            _text.text  = Time.ToSeconds (_explodesAt).ToString ();

            _activatesAt += _.War.Time;
            _explodesAt  += _.War.Time;

            PositionChanged += (from, to) => _gameObject.transform.localPosition = (Vector3) to;
        }


        public bool Alive => !Despawned;


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity.Y += WarScene.Gravity;
            if (_.War.Time >= _explodesAt) {
                Despawn ();
                _.War.MakeExplosionPoison (Position);
                return;
            }
            if (_.War.Time >= _activatesAt) {
                var v = new XY (_.Random.Angle ()) * Balance.RadiusExplosionTiny * Balance.SmokeWindFactor;
                _.War.Spawn (new PoisonGas (_.Random.Float (25, 60), Position, v));
            }
            _text.text = Time.CeilToSeconds (_explodesAt - _.War.Time).ToString ();
        }

    }

}