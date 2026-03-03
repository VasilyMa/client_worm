using System;
using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;
using War.Entities.Smokes;
using War.Indication;
using War.Systems.Updating;
using Object = UnityEngine.Object;


namespace War.Entities {

    public class Bomber : MobileEntity, IUpdatable {

        private readonly Func <MobileEntity> _generator;
        private readonly int                 _bombs;
        private readonly float               _vx; // скорость бомб

        // private int _ticks;
        private int _spawnedAt;

        private PointCrosshair _crosshair;

        private GameObject _gameObject;

        
        public Bomber (Func <MobileEntity> generator, int bombs, float vx) {
            _generator = generator;
            _bombs     = bombs;
            _vx        = vx;
        }

        
        public Bomber (Func <MobileEntity> generator, int bombs, float vx, PointCrosshair crosshair) {
            _generator = generator;
            _bombs     = bombs;
            _vx        = vx;
            _crosshair = crosshair;
            _crosshair.References++;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            
            _spawnedAt = _.War.Time;
            _gameObject = Object.Instantiate (_.War.Assets.Bomber, (Vector3) Position, Quaternion.identity);
            _gameObject.transform.localScale = new Vector3 (_vx > 0 ? 1 : -1, 1, 1);

            PositionChanged += (from, to) => _gameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (_gameObject);
        }


        public bool Alive => !Despawned;


        public void Update (TurnData td) {
            for (int i = 0; i < 5; i++) {
                _.War.Spawn (
                    new Smoke (
                        25 + _.War.Random.Int (25),
                        Position + new XY (_vx > 0 ? -25 : 25, 0),
                        Danmaku.Shotgun (
                            new XY (_vx > 0 ? -1 : 1, 0),
                            0.2f,
                            40,
                            60
                        )
                    )
                );
            }
            
            _.War.Wait ();
            int t = _.War.Time - _spawnedAt;
            // если бомба одна, сбрасываем ее в _spawnedAt + 1s
            // если больше, то у первой бомбы будет смещение по времени на n тиков где n количество бомб минус 1
            // деспавн самолета в момент _spawned + 2s

            if (t >= 120) {
                Despawn ();
                if ((object) _crosshair != null) _crosshair.References--;
                return;
            }
            t -= 60 - (_bombs - 1) * 1; // поменять коэффициент если надо сбрасывать бомбы с меньшим интервалом
            if (t < 0 || t % 2 != 0) return; // опять же 2 поменять если вдруг что
            t /= 2;
            if (t < _bombs) {
                _.War.Spawn (_generator (), Position, new XY (_vx, 0));
            }
        }

    }

}