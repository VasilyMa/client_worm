using System;
using System.Collections.Generic;
using Ups = UnityEngine.ParticleSystem;


namespace War.Systems.Particles {

    public class ParticleSystem {

        public delegate void UpdateFunction (ref Ups.Particle particle);


        public sealed class ParticleHandler {

            public bool           Alive = true;
            public UpdateFunction Update;


            public ParticleHandler (UpdateFunction update) {
                Update = update;
            }

        }


        private readonly Ups                    unityParticleSystem;
        private readonly List <ParticleHandler> handlers  = new List <ParticleHandler> (500);
        private          Ups.Particle []        particles = new Ups.Particle [500];
        private          int                    particleCount;


        public ParticleSystem (Ups unityParticleSystem) {
            this.unityParticleSystem = unityParticleSystem;
        }


        public void Update () {
            // удалить лишние обработчики
            handlers.RemoveAll (h => !h.Alive);

            int count = handlers.Count;

            // выделить место под частицы если его не хватает
            if (count > particles.Length) {
                Array.Resize (ref particles, System.Math.Max (count, 2 * particles.Length));
            }

            // выпустить частицы если нужно
            if (count > particleCount) {
                unityParticleSystem.Emit (count - particleCount);
            }

            particleCount = unityParticleSystem.GetParticles (particles);

            // обновление частиц
            for (int i = 0; i < count; i++) {
                handlers [i].Update (ref particles [i]);
            }
            // удаление лишних частиц
            for (int i = count; i < particleCount; i++) {
                particles [i].remainingLifetime = -1;
            }

            unityParticleSystem.SetParticles (particles, particleCount);
            particleCount = count;
        }


        public void Add (ParticleHandler handler) {
            handlers.Add (handler);
        }


        public ParticleHandler Add (UpdateFunction update) {
            var handler = new ParticleHandler (update);
            handlers.Add (handler);
            return handler;
        }

    }

}
