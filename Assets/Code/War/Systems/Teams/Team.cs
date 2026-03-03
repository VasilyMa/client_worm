using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using DataTransfer.Data;
using UnityEngine;
using War.Arsenals;
using War.Entities;


namespace War.Systems.Teams {

    public abstract class Team {

        private Queue <Worm> _worms;
        public IEnumerable <Worm> Worms => _worms;

        public readonly Color Color;
        public readonly int TeamId;

        public Arsenal Arsenal;


        public bool Alive {
            get {
                while (_worms.Count != 0) {
                    var w = _worms.Peek ();
                    if (!w.Despawned) return true;
                    _worms.Dequeue ();
                }
                return false;
            }
        }


        protected Team (int colorId, string [] wormsNames) {
            TeamId = colorId;
            Color  = Colors.PlayerColors [colorId];
            _worms = new Queue <Worm> (wormsNames.Select (name => new Worm (name, this)));
        }


        // метод изменяет состояние объекта, он будет возвращать всех червяков по очереди
        public Worm NextWorm () {
            while (_worms.Count != 0) {
                var w = _worms.Dequeue ();
                if (w.Despawned) continue;
                _worms.Enqueue (w);
                return w;
            }
            return null;
        }


        public abstract TurnData GetTurnData ();

    }

}
