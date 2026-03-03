using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using War.Systems.Updating;
using ParticleSystem = War.Systems.Particles.ParticleSystem;


namespace War.Entities.Smokes {

    public class PoisonGas : Entity, IUpdatable, IPoison {

        private XY                             _position, _velocity;
        private ParticleSystem.ParticleHandler _phFront,  _phBack;
        private float                          _despawnsAt;
        

        public bool Alive => !Despawned;


        public int Dose => Mathf.CeilToInt((_despawnsAt - _.War.Time) / 20f);


        public Circle Hitbox => new Circle (_position, (_despawnsAt - _.War.Time) * 0.5f);


        // 20 единиц размера 1 урон
        public PoisonGas (float size, XY position, XY velocity) {
            _despawnsAt = size;
            _position   = position;
            _velocity   = velocity;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.PoisonSystem.Add (this);
            
            _despawnsAt += _.War.Time;
            _phFront    =  _.War.SmokeFrontParticles.Add (UpdateFront);
            _phBack     =  _.War.SmokeBackParticles .Add (UpdateBack);
        }


        private void UpdateFront (ref UnityEngine.ParticleSystem.Particle particle) {
            float size     = _despawnsAt - _.War.Time;
            var   position = (Vector3) _position;
            position.y += size * 0.1f;
            
            particle.position   = position;
            particle.startSize  = size * 0.8f;
            particle.startColor = new Color (0.5f, 1f, 0.5f);
        }


        private void UpdateBack (ref UnityEngine.ParticleSystem.Particle particle) {
            particle.position   = (Vector3) _position;
            particle.startSize  = _despawnsAt - _.War.Time;
            particle.startColor = new Color (0.4f, 0.9f, 0.4f);
        }


        protected override void OnDespawn () {
            _phFront.Alive =
            _phBack .Alive = false;
        }


        public void Update (TurnData td) {
            if (_.War.Time >= _despawnsAt) {
                Despawn ();
                return;
            }
            _position +=
            _velocity = XY.Lerp (_velocity, new XY (0, 1), Balance.SmokeWindFactor);
        }

    }

}
