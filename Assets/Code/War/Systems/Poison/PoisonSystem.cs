using System;
using System.Collections.Generic;
using Math;


namespace War.Systems.Poison {

    public class PoisonSystem {

        // ответственность класса:
        // обработка влияния ядовитого газа на объекты

        private List <IPoison>     _poisons     = new List <IPoison>     ();
        private List <IPoisonable> _poisonables = new List <IPoisonable> ();


        public void Update () {
            for (int i = 0; i < _poisons.Count; i++) {
                if (!_poisons [i].Alive) continue;
                for (int j = 0; j < _poisonables.Count; j++) {
                    if (_poisonables [j].Alive) {
                        _poisonables [j].TryApplyPoison (_poisons [i]);
                    }
                }
            }
            _poisons    .RemoveAll (poison     => !poison    .Alive);
            _poisonables.RemoveAll (poisonable => !poisonable.Alive);
        }


        public void Add (IPoison poison) {
            _poisons.Add (poison);
        }


        public void Add (IPoisonable poisonable) {
            _poisonables.Add (poisonable);
        }

    }

}


public interface IPoison {

    bool   Alive  { get; }
    int    Dose { get; }
    Circle Hitbox { get; } // уже со смещением должен быть, здесь же нет требования к координатам

}


public interface IPoisonable {

    bool Alive { get; }
    void TryApplyPoison (IPoison poison);

}