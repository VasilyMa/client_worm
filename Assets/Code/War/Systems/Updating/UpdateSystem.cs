using System.Collections.Generic;
using DataTransfer.Data;
using UnityEngine;


namespace War.Systems.Updating {

    public class UpdateSystem {

        // ответственность класса:
        // обновление всего, что может быть обновлено


        private List <IUpdatable> _things = new List <IUpdatable> ();


        public void Add (IUpdatable thing) {
            _things.Add (thing);
        }


        public void Update (TurnData td) 
        {
            if (td == null) return;

            UnityEngine.Debug.Log("Update system run");

            for (int i = 0; i < _things.Count; i++) {
                if (_things [i].Alive) _things [i].Update (td);
            }
            _things.RemoveAll (thing => !thing.Alive);
        }
        
    }

}
