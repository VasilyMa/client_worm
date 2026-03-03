using Core;
using DataTransfer.Data;
using Math;
using UnityEngine;
using Utils;
using War.Systems.Blasts;
using War.Systems.Updating;
using Time = Utils.Time;


namespace War.Entities.Projectiles {

    public class HolyHandGrenade : MobileEntity, IUpdatable, IBlastable {

        public bool       Alive      => !Despawned;
        public GameObject GameObject { get; private set; }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            
            AddCircleCollider (Balance.GrenadeRadius);

            GameObject = Object.Instantiate (
                _.War.Assets.HolyHandGrenade,
                (Vector3) Position,
                Quaternion.identity
            );
            
            PositionChanged += (from, to) => GameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (GameObject);
        }


        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            Velocity.Y += WarScene.Gravity;
            if (_.War.Time >= IdleAt + Time.Seconds (2)) {
                Despawn ();
                _.War.MakeExplosionHuge (Position);
            }
        }

    }

}
