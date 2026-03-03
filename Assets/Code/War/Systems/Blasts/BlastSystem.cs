using System;
using System.Collections.Generic;
using Math;
using UnityEngine;


namespace War.Systems.Blasts {

    public class BlastSystem {

        // ответственность класса:
        // делать чтобы взрывы отбрасывали объекты


        private List <IBlastable> _things = new List <IBlastable> ();


        public void Add (IBlastable thing) {
            _things.Add (thing);
        }


        public void MakeBlast (XY center, XY shiftedCenter, float radius, float magnitude) {
            // todo: сделать чтобы брались только те объекты которые близко

            for (int i = 0; i < _things.Count; i++) {
                var thing = _things [i];
                if (!thing.Alive) continue;
                
                float sqrDist = XY.SqrDistance (center, thing.Position);
                if (sqrDist >= radius * radius) continue;
                
                float factor = (radius - Mathf.Sqrt (sqrDist)) / radius;
                thing.TakeBlast ((thing.Position - shiftedCenter).WithLength (magnitude * factor));
            }
            
            _things.RemoveAll (thing => !thing.Alive);
        }

    }

}
