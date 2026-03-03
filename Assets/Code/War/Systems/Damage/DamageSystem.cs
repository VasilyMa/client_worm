using System.Collections.Generic;
using Math;
using UnityEngine;
using static UnityEngine.Mathf;


namespace War.Systems.Damage {

    public class DamageSystem {

        // ответственность класса:
        // нанесение объектам урона


        private List <IDamageable> _things = new List <IDamageable> ();


        public void Add (IDamageable thing) {
            _things.Add (thing);
        }

        
        public void DealDamage (
            XY    center,      
            float radius,      // радиус, в котором наносится урон
            float innerRadius, // радиус, в котором наносится максимальный урон
            int   damage       // максимальный урон
        ) {
            for (int i = 0; i < _things.Count; i++) {
                var e = _things [i];
                if (!e.Alive) continue;
                
                float sqrDist = XY.SqrDistance (center, e.Position);
                if (sqrDist >= radius * radius) continue;
                
                e.TakeDamage (
                    sqrDist > innerRadius * innerRadius
                    ? CeilToInt (damage * InverseLerp (radius, innerRadius, Sqrt (sqrDist)))
                    : damage
                );
            }
        }

    }

}
