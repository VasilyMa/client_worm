using System;
using System.Collections.Generic;
using Core;
using DataTransfer.Data;
using Math;
using TMPro;
using UnityEngine;
using Utils;
using War.Entities;
using War.Entities.Barrels;
using War.Entities.Liquids;
using War.Entities.Projectiles;
using War.Systems.Blasts;
using War.Systems.Damage;
using War.Systems.Updating;
using Collision = War.Systems.Collisions.Collision;
using Object = UnityEngine.Object;


namespace War.Systems.Debug {

    // в настоящее время эта штука будет отвечать за спавн в мире различных снарядов
    // ну типа, на левую кнопку мы спавним снаряд, на правую включается меню с выбором из кучи снарядов
    public class DebugSystem {

        private List <Tuple <Action <XY>, string>> _fs = new List <Tuple <Action <XY>, string>> {
            new Tuple <Action <XY>, string> (xy => {
                if (_.War.ActiveWorm == null) return;
                _.War.ActiveWorm.Position = xy;
                _.War.ActiveWorm.Velocity = XY.Zero;
                _.War.ActiveWorm.Jump ();
            }, "Телепортировать червяка"),
            new Tuple <Action <XY>, string> (xy => {
                if (_.War.ActiveWorm == null) return;
                _.War.ActiveWorm.Position = xy;
                _.War.ActiveWorm.Velocity = new XY(0, 10);
                _.War.ActiveWorm.Jump ();
            }, "Телепортировать червяка, скорость 10"),
            new Tuple <Action <XY>, string> (xy => {
                if (_.War.ActiveWorm == null) return;
                _.War.ActiveWorm.Position = xy;
                _.War.ActiveWorm.Velocity = new XY(0, 15);
                _.War.ActiveWorm.Jump ();
            }, "Телепортировать червяка, скорость 15"),
            new Tuple <Action <XY>, string> (xy => {
                if (_.War.ActiveWorm == null) return;
                _.War.ActiveWorm.Position = xy;
                _.War.ActiveWorm.Velocity = new XY(0, 25);
                _.War.ActiveWorm.Jump ();
            }, "Телепортировать червяка, скорость 25"),
            new Tuple <Action <XY>, string> (xy => MakeExplosion10 (xy), "Взрыв 10"),
            new Tuple <Action <XY>, string> (xy => MakeExplosion15 (xy), "Взрыв 15"),
            new Tuple <Action <XY>, string> (xy => MakeExplosion25 (xy), "Взрыв 25"),
            new Tuple <Action <XY>, string> (xy => MakeExplosion40 (xy), "Взрыв 40"),
            new Tuple <Action <XY>, string> (xy => MakeExplosion50 (xy), "Взрыв 50"),
            
            new Tuple <Action <XY>, string> (SpawnCloud (() => new TestProjectile3 ()), "Тестовый снаряд 1"),
            // 1 колонка - базуки всякие
            // new Tuple <Action <XY>, string> (Spawn (() => new _BazookaShell ()), "Снаряд базуки"),
            // new Tuple <Action <XY>, string> (xy => {}, "Самонаводящаяся ракета"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _MiniRocket ()), "Мини-ракета"),
            // new Tuple <Action <XY>, string> (xy => {}, "Замораживающий снаряд"),
            // new Tuple <Action <XY>, string> (xy => {}, "Птица"),
            // new Tuple <Action <XY>, string> (xy => {}, "Пила"),
            // // 2 колонка - гранаты
            // new Tuple <Action <XY>, string> (Spawn (() => new _Grenade      (Utils.Time.Seconds (2))), "Граната"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _Limonka      (Utils.Time.Seconds (2))), "Лимонка"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _LimonkaCluster  ()), "Осколок лимонки"),
            // new Tuple <Action <XY>, string> (Spawn (() => new MolotovBottle   ()), "Коктейль Молотова"),
            // new Tuple <Action <XY>, string> (Spawn (() => new GhostGrenade (Utils.Time.Seconds (2))), "Призрачная граната"),
            // new Tuple <Action <XY>, string> (Spawn (() => new Flashbang    (Utils.Time.Seconds (2))), "Светошумовая граната"),
            // new Tuple <Action <XY>, string> (Spawn (() => new GasGrenade      ()), "Газовая граната"),
            // new Tuple <Action <XY>, string> (Spawn (() => new HolyHandGrenade ()), "Святая граната"),
            // // 3 колонка - огнестрельное
            // new Tuple <Action <XY>, string> (xy => {}, "Автоматная пуля"),
            // new Tuple <Action <XY>, string> (xy => {}, "Пистолетная пуля"),
            // new Tuple <Action <XY>, string> (xy => {}, "Отравленный болт"),
            // new Tuple <Action <XY>, string> (xy => {}, "Бластерный снаряд"),
            // new Tuple <Action <XY>, string> (xy => {}, "Тяжелый бластерный снаряд"),
            // new Tuple <Action <XY>, string> (xy => {}, "Лазер"),
            // new Tuple <Action <XY>, string> (xy => {}, "Луч ультравинтовки"),
            // new Tuple <Action <XY>, string> (xy => {}, "Пыщ-луч"),
            // // 4 колонка - ближний бой
            // // 5 колонка - взрывчатка и животные
            // new Tuple <Action <XY>, string> (Spawn (() => new Mine ()), "Мина"),
            // new Tuple <Action <XY>, string> (Spawn (() => new Dynamite ()), "Динамит"),
            // new Tuple <Action <XY>, string> (xy => {}, "Газовая штука"),
            // new Tuple <Action <XY>, string> (xy => {}, "Лягушка"),
            // new Tuple <Action <XY>, string> (xy => {}, "Суперлягушка"),
            // new Tuple <Action <XY>, string> (xy => {}, "Крот"),
            // new Tuple <Action <XY>, string> (xy => {}, "Шагающая бомба"),
            // // 6 колонка - авиаудары
            // new Tuple <Action <XY>, string> (Spawn (() => new _Bomb ()), "Авиабомба"),
            // new Tuple <Action <XY>, string> (Spawn (() => new NapalmBomb ()), "Напалмовая бомба"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _PoisonBomb ()), "Отравляющая бомба"),
            // new Tuple <Action <XY>, string> (Spawn (() => new VacuumBomb ()), "Вакуумная бомба"),
            // new Tuple <Action <XY>, string> (xy => {}, "Кротобомба"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _Nuke ()), "Ядерная ракета"),
            // // 7 колонка - заклинания
            // new Tuple <Action <XY>, string> (Spawn (() => new _MeteorSmall ()), "Маленький метеорит"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _Meteor ()), "Средний метеорит"),
            // new Tuple <Action <XY>, string> (Spawn (() => new _MeteorLarge ()), "Большой метеорит"),
            // // 8-9 колонка - утилиты
            // // бочки и прочее
            // new Tuple <Action <XY>, string> (xy => {}, "Ящик"),
            // new Tuple <Action <XY>, string> (Spawn (() => new ExplosiveBarrel ()), "Бочка со взрывчаткой"),
            // new Tuple <Action <XY>, string> (Spawn (() => new FuelBarrel ()), "Бочка с топливом"),
            // new Tuple <Action <XY>, string> (Spawn (() => new PoisonBarrel ()), "Бочка с ядом"),
            // new Tuple <Action <XY>, string> (Spawn (() => new GlueBarrel ()), "Бочка с клеем"),
            // new Tuple <Action <XY>, string> (Spawn (() => new Flame ()), "Огонек"),
            // new Tuple <Action <XY>, string> (xy => {}, "Капля яда"),
            // new Tuple <Action <XY>, string> (xy => {}, "Капля клея"),
        };


        private static void MakeExplosion10 (XY o) {
            _.War.Land.DestroyTerrain (o, 20);
            _.War.DamageSystem.DealDamage (o, 40, 10, 10);
            _.War.BlastSystem.MakeBlast (o, o, 40, 5);
        }


        private static void MakeExplosion15 (XY o) {
            _.War.Land.DestroyTerrain (o, 30);
            _.War.DamageSystem.DealDamage (o, 60, 10, 15);
            _.War.BlastSystem.MakeBlast (o, o, 60, 7);
        }


        private static void MakeExplosion25 (XY o) {
            _.War.Land.DestroyTerrain (o, 50);
            _.War.DamageSystem.DealDamage (o, 100, 10, 25);
            _.War.BlastSystem.MakeBlast (o, o, 100, 10);
        }


        private static void MakeExplosion40 (XY o) {
            _.War.Land.DestroyTerrain (o, 80);
            _.War.DamageSystem.DealDamage (o, 160, 10, 40);
            _.War.BlastSystem.MakeBlast (o, o, 160, 13);
        }


        private static void MakeExplosion50 (XY o) {
            _.War.Land.DestroyTerrain (o, 100);
            _.War.DamageSystem.DealDamage (o, 200, 10, 50);
            _.War.BlastSystem.MakeBlast (o, o, 200, 15);
        }


        private int  _i  = 0;
        private bool _mb = true;


        public void Init () {
            _.War.Hint.text = _fs [0].Item2;
        }


        public void Update (TurnData td) {
            if (UnityEngine.Input.GetKeyDown (KeyCode.LeftArrow )) Left  ();
            if (UnityEngine.Input.GetKeyDown (KeyCode.RightArrow)) Right ();
            if (td.MB && !_mb) _fs [_i].Item1 (td.XY);
            _mb = td.MB;
        }


        public void Left () {
            UnityEngine.Debug.Log ("left");
            if (--_i < 0) _i += _fs.Count;
            _.War.Hint.text = _fs [_i].Item2;
        }


        public void Right () {
            UnityEngine.Debug.Log ("right");
            if (++_i == _fs.Count) _i = 0;
            _.War.Hint.text = _fs [_i].Item2;
        }


        private static Action <XY> Spawn (Func <MobileEntity> gen) {
            return xy => { _.War.Spawn (gen (), xy, new XY (0, 5)); };
        }


        private static Action <XY> SpawnCloud (Func <MobileEntity> gen) {
            return xy => {
                for (int i = 0; i < 1; i++) _.War.Spawn (gen (), xy, Danmaku.Cloud (30));
            };
        }

    }
    // тут заканчивается класс DebugSystem


    public class TestProjectile3 : MobileEntity, IUpdatable {

        // я сделал его после перерыва и мне надо вспомнить как все работает
        // сначала просто сделаем снаряд без всего только со спрайтом

        protected GameObject GameObject;


        public override void OnSpawn () {
            UnityEngine.Debug.Log ("!!!");
            _.War.UpdateSystem.Add (this);

            GameObject = Object.Instantiate (
                _.War.Assets.Grenade,
                (Vector3) Position,
                Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg)
            );
            
            PositionChanged += (from, to) => GameObject.transform.localPosition = (Vector3) to;
        }


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (GameObject);
        }


        public bool Alive => !Despawned;
        
        
        public void Update (TurnData td) {
            // Velocity.X += _.War.Wind * Balance.WindFactor; // wind
            
            // Velocity.Y += WarScene.Gravity; // gravity
            
            // Velocity.Length += 0.25f; // accelerating?
            
            // Velocity = XY.Lerp (Velocity, new XY (_.War.Wind, 0), Balance.LiquidWindFactor); // decelerating + wind
            
            // homing
            // var target = new XY (500, 250);
            // var v      = ((target - Position) / 2).WithLengthClamped (Mathf.Max(30f, Velocity.Length));
            // Velocity   = XY.Lerp (Velocity, v, 1 / 15f);
            
            // flail
            // var anchor = new XY (500, 250);
            // нам нужно не давать снаряду улететь за длину веревки
            // при этом воздействуя на него достаточно плавно
            // у нас есть код для дыма где он никогда не улетает за свой радиус
        }

    }


    public class TestProjectile : MobileEntity {
        // что мы тут делаем
        // полет снаряда (чем и куда сносится)
        // визуал (какие спрайты, как крутится и т.д.)
        // триггеры событий

        protected int        ExpiresAt = Utils.Time.Seconds (15);
        protected GameObject GameObject, ProjectileGameObject;
        protected TMP_Text   Text;


        protected override void OnDespawn () {
            base.OnDespawn ();
            Object.Destroy (GameObject);
        }
        
        
        // методы утилиты


        //protected void ApplyWind ()        => Velocity.X += _.War.Wind * Balance.WindFactor;
        protected void ApplyGravity ()     => Velocity.Y += WarScene.Gravity;
        protected void UpdateTimer ()      => Text.text = Utils.Time.CeilToSeconds (ExpiresAt - _.War.Time).ToString ();
        protected void OrientByVelocity () => ProjectileGameObject.transform.localRotation = Quaternion.Euler (0, 0, Velocity.Angle * Mathf.Rad2Deg);
        protected void MakeSmokeTrail () {}
        protected bool Expired => ExpiresAt <= _.War.Time;


        protected void SetupGameObject (string name, GameObject sprite, bool withTimer) {
            GameObject = new GameObject (name);
            GameObject.transform.localPosition = (Vector3) Position;
            ProjectileGameObject = Object.Instantiate (sprite, GameObject.transform, false);
            PositionChanged += (from, to) => GameObject.transform.localPosition = (Vector3) to;
            
            if (!withTimer) return;

            var canvas = Object.Instantiate (_.War.Assets.Canvas, GameObject.transform, false).GetComponent <Canvas> ();
            canvas.worldCamera = UnityEngine.Camera.main;
            var area = Object.Instantiate (_.War.Assets.TextAreaBottom, canvas.transform, false);
            Text = Object.Instantiate (_.War.Assets.Text24, area.transform, false).GetComponent <TMP_Text> ();
            Text.color = Colors.White;
            UpdateTimer ();
        }

    }


    public abstract class _TestProjectile2 : TestProjectile, IUpdatable, IBlastable {

        private readonly string     _name;
        private readonly GameObject _sprite;
        private readonly bool       _gravity;
        private readonly bool       _wind;
        private readonly bool       _orientByVelocity;
        private readonly float      _colliderRadius;
        private readonly int        _massRank;
        private readonly bool       _explodesOnImpact;
        private readonly int        _timer;

        
        public bool Alive => !Despawned;


        public _TestProjectile2 (
            string name,
            GameObject sprite,
            bool explodesOnImpact,
            bool gravity,
            bool wind,
            bool orientByVelocity,
            float colliderRadius,
            int massRank,
            int timer = 0
        ) {
            _name             = name;
            _sprite           = sprite;
            _explodesOnImpact = explodesOnImpact;
            _gravity          = gravity;
            _wind             = wind;
            _orientByVelocity = orientByVelocity;
            _colliderRadius   = colliderRadius;
            MassRank          = massRank;

            if (timer != 0) _timer = timer;
        }
        
        
        public void TakeBlast (XY impulse) {
            Velocity += impulse;
        }


        public override void OnSpawn () {
            _.War.UpdateSystem.Add (this);
            _.War.BlastSystem .Add (this);
            AddCircleCollider (_colliderRadius);
            ExpiresAt += _.War.Time;
            SetupGameObject (_name, _sprite, true);
        }


        public void Update (TurnData td) {
            _.War.Wait ();
            if (Expired) {
                Explode ();
                return;
            }
            if (_gravity) ApplyGravity ();
            //if (_wind) ApplyWind ();
            UpdateTimer ();
            if (_orientByVelocity) OrientByVelocity ();
        }


        protected abstract void Explode ();


        public override void OnCollision (Collision collision) {
            if (_explodesOnImpact) Explode ();
        }

    }
    
    
    public class _BazookaShell : _TestProjectile2 {

        public _BazookaShell () : base ("bazooka shell", _.War.Assets.BazookaShell, true, true, true, true, Balance.ShellRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosion (Position);
        }

    }
    
    
    public class _MiniRocket : _TestProjectile2 {

        public _MiniRocket () : base ("mini rocket", _.War.Assets.MiniRocket, true, true, true, true, Balance.ShellRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionSmall (Position);
        }

    }
    
    
    public class _Bomb : _TestProjectile2 {

        public _Bomb () : base ("bomb", _.War.Assets.Bomb, true, true, false, true, Balance.ShellRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionSmall (Position);
        }

    }
    
    
    public class _PoisonBomb : _TestProjectile2 {

        public _PoisonBomb () : base ("poison bomb", _.War.Assets.PoisonBomb, true, true, false, true, Balance.ShellRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionPoisonSmall (Position);
        }

    }
    
    
    public class _MeteorSmall : _TestProjectile2 {

        public _MeteorSmall () : base ("small meteor", _.War.Assets.BazookaShell, true, true, false, true, Balance.MeteorRadiusSmall, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionSmall (Position);
        }

    }
    
    
    public class _Meteor : _TestProjectile2 {

        public _Meteor () : base ("meteor", _.War.Assets.BazookaShell, true, true, false, true, Balance.MeteorRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosion (Position);
        }

    }
    
    
    public class _MeteorLarge : _TestProjectile2 {

        public _MeteorLarge () : base ("large meteor", _.War.Assets.BazookaShell, true, true, false, true, Balance.MeteorRadiusLarge, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionLarge (Position);
        }

    }
    
    
    public class _Nuke : _TestProjectile2 {

        public _Nuke () : base ("large meteor", _.War.Assets.BazookaShell, true, true, false, true, Balance.NukeMissileRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionNuclear (Position);
        }

    }
    
    
    public class _Grenade : _TestProjectile2 {

        public _Grenade (int timer) : base ("grenade", _.War.Assets.Grenade, false, true, false, false, Balance.GrenadeRadius, Balance.GrenadeMassRank, timer) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosion (Position);
        }

    }
    
    
    public class _Limonka : _TestProjectile2 {

        public _Limonka (int timer) : base ("limonka", _.War.Assets.Limonka, false, true, false, false, Balance.GrenadeRadius, Balance.GrenadeMassRank, timer) {}

        protected override void Explode () {
            Despawn ();
            for (int i = 0; i < 6; i++) _.War.Spawn (
                new _LimonkaCluster (),
                Position,
                Danmaku.Shotgun (XY.Up, Mathf.PI / 6, Balance.ClusterMinSpeed, Balance.ClusterMaxSpeed)
            );
        }

    }
    
    
    public class _LimonkaCluster : _TestProjectile2 {

        public _LimonkaCluster () : base ("limonka cluster", _.War.Assets.LimonkaCluster, true, true, false, false, Balance.ClusterRadius, Balance.ShellMassRank) {}

        protected override void Explode () {
            Despawn ();
            _.War.MakeExplosionCluster (Position);
        }

    }

}