using System;
using Core;
using DataTransfer.Data;
using Math;
using War.Systems.Blasts;
using War.Systems.Updating;


namespace War.Entities.Liquids {

    public class Liquid : MobileEntity, IBlastable, IUpdatable {

        [Obsolete] public bool Fixed { get; private set; }
        // я пока не убираю это на тот случай если Fixed и Idle все-таки будут отличаться но имхо это одно и то же

        public bool Alive => !Despawned;


        public Liquid () {
            MassRank = Balance.ShellMassRank;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            
            AddCircleCollider (0);
        }


        public virtual void Update (TurnData td) {
            if (Idle) return;
            _.War.Wait ();
            Velocity.Y += WarScene.Gravity;
            Velocity   =  XY.Lerp (Velocity, new XY (0, 0), Balance.LiquidWindFactor);
        }


        public void TakeBlast (XY impulse) {
            Unfix ();
            Velocity += impulse;
        }


        protected void Fix () {
            Velocity = XY.Zero;
            IdleAt   = _.War.Time;
            // todo запомнить пиксель к которому прилепились
        }


        protected void Unfix () {
            Poke ();
            _.War.Wait ();
        }

    }

}
