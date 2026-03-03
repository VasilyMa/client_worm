using System.Collections.Generic;
using System.Text;
using Core;
using DataTransfer.Data;
using Geometry;
using Math;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using War.Components;
using War.Components.CollisionHandlers;
using War.Components.Controllers;
using War.Components.Other;
using War.Entities.Labels;
using War.Entities.Liquids;
using War.Weapons;
using War.Systems.Blasts;
using War.Systems.Damage;
using War.Systems.Fire;
using War.Systems.Teams;
using War.Systems.Updating;
using BoxCollider = War.Components.BoxCollider;
using Collider = War.Components.Collider;
using Collision = War.Systems.Collisions.Collision;
using Object = UnityEngine.Object;


namespace War.Entities
{

    public partial class Worm : MobileEntity, IUpdatable, IDamageable, IBlastable, IPoisonable, IFireDamageable {

        // внимание: перед тем как править константы загляни в контроллер ползания червяка
        public const  float HeadRadius     = 7;
        public const  float BodyHeight     = 7;
        public const  float HalfBodyHeight = BodyHeight * 0.5f;

        public const  float WalkSpeed      = 0.5f;
        public const  float JumpSpeed      = 5;
        public const  float JumpAngle      = 0.5f;
        public const  float HighJumpSpeed  = 7;
        public const  float HighJumpAngle  = 0.1f;

        public const  float MaxClimb       = 4;
        public const  float MaxDescend     = 4;
        
        public static XY    HeadOffset     = new XY (0,  HalfBodyHeight);
        public static XY    TailOffset     = new XY (0, -HalfBodyHeight);

        // поля класса и авто-свойства
        private readonly string _name;
        private int _hp;
        private int _poison;
        private int _fireDamage = 99; // сколько еще огонь должен действовать чтобы нанести урон (при 0 урон наносится)

        public  Team  Team   { get; private set; }
        public  bool  FacesRight;
        
        // что связано с уроном от падения переношу сюда
        public XY    _previousVelocity; // абсолютная скорость до столкновения
        public float _fallDamageCap; // верхняя граница урона считаемая по скорости относительно второго объекта

        // иерархия объектов:
        // worm            - нужно чтобы объект был виден снаружи для crosshair
        //   hud
        //   crosshair
        //   sorting group - нужно чтобы объект был виден снаружи для weapon
        //     sprite
        //     weapon
        public Transform WeaponSlot    { get; private set; }
        public Transform CrosshairSlot { get; private set; }

        public WormSprite2 Sprite { get; private set; }

        private GameObject              _gameObject;
        private TMP_Text                _nameText, _hpText;
        private Controller       <Worm> _controller;
        private CollisionHandler <Worm> _collisionHandler;
        private Weapon                  _weapon;

        public CircleCollider Head { get; private set; }
        public BoxCollider    Body { get; private set; }
        public CircleCollider Tail { get; private set; }
        
        // для оружий ближнего боя
        public Collider [] Colliders => new Collider [] {Head, Body, Tail};


        // свойства


        public int HP {
            get { return _hp; }
            private set {
                _hp = value;
                UpdateHpText ();
            }
        }


        public int Poison {
            get { return _poison; }
            private set {
                _poison = value;
                UpdateHpText ();
            }
        }


        public override bool PassableFor (IRayOrMobile e) => e == this || e is Liquid;


        public override bool PushableFor (MobileEntity e) {
            if (Controller is WormDeathCtrl) {
                return e == this; // может быть я уже слишком перестраховываюсь
            }
            // if (usingFirePunch) {
            //     return e == this;
            // }
            if (CollisionHandler is WormStandCH) {
                return (e as Worm)?.CollisionHandler is WormFallCh;
            }
            return base.PushableFor (e);
        }


        public bool CanUseWeapon => !(Controller is WormFallCtrl);
        

        public Controller <Worm> Controller {
            get {
                return _controller;
            }
            private set {
                _controller?.OnDetach ();
                value.Entity = this;
                _controller = value;
                value.OnAttach ();
            }
        }


        public CollisionHandler <Worm> CollisionHandler {
            get { return _collisionHandler; }
            set {
                _collisionHandler?.OnDetach ();
                value.Entity = this;
                _collisionHandler = value;
                value.OnAttach ();
            }
        }


        // только что выбрал оружие в арсенале
        public void EquipWeapon (Weapon weapon, TurnData td) {
            _weapon?.OnUnequip ();
            weapon.Worm = this;
            _weapon = weapon;
            weapon.OnEquip (td);
        }


        // когда червяк встает например или хз
        public void ReequipWeapon (TurnData td) {
            _weapon.OnReequip ();
        }


        public void UnequipWeapon () {
            _weapon?.OnUnequip ();
            _weapon = null;
        }


        public void UnequipWeaponSlowly () {
            _weapon?.OnSlowUnequip ();
        }


        // обычные методы

        public Worm (string name, Team team) {
            _name    = name;
            Team     = team;
            _hp      = 600;
            MassRank = Balance.WormMassRank;
        }


        public override void OnSpawn () {
            // работа с системами
            _.War.WormsSystem .Add (this);
            _.War.UpdateSystem.Add (this);
            _.War.DamageSystem.Add (this);
            _.War.BlastSystem .Add (this);
            _.War.PoisonSystem.Add (this);
            _.War.FireSystem  .Add (this);

            AddCollider (Head = new CircleCollider (0,  HalfBodyHeight, HeadRadius, 0.6f, 0.3f));
            AddCollider (Tail = new CircleCollider (0, -HalfBodyHeight, HeadRadius, 0.6f, 0.3f));
            AddCollider (Body = new BoxCollider (-HeadRadius, HeadRadius, -HalfBodyHeight, HalfBodyHeight, 0.6f, 0.3f));

            // работа со спрайтами и трансформами
            _gameObject = Object.Instantiate (_.War.Assets.Worm, (Vector3) Position, Quaternion.identity);
            
            CrosshairSlot = _gameObject.transform;
            WeaponSlot    = _gameObject.GetComponentInChildren <SortingGroup> ().transform;
            
            var canvas = _gameObject.GetComponentInChildren <Canvas> ();
            canvas.worldCamera = Camera.main;
            
            var area = Object.Instantiate (_.War.Assets.TextAreaTop, canvas.transform, false);
            
            _nameText = Object.Instantiate (_.War.Assets.Text24, area.transform, false).GetComponent <TMP_Text> ();
            _hpText   = Object.Instantiate (_.War.Assets.Text24, area.transform, false).GetComponent <TMP_Text> ();

            _nameText.text  = _name;
            UpdateHpText ();
            _nameText.color = _hpText.color = Team.Color;

            Sprite = _gameObject.GetComponent <WormSprite2> ();
//            Sprite.Jump (true, 0);
            PositionChanged += (from, to) => _gameObject.transform.localPosition = (Vector3) to;

            //Jump ();
            Walk();
            Sprite.DoUpdate ();
        }


        public void DebugSetName (string text) {
            _nameText.text = text;
        }


        protected override void OnDespawn () {
            UnequipWeapon ();
            if (this == _.War.ActiveWorm) {
                _.War.EndTurn ();
            }
            base.OnDespawn ();
            Object.Destroy (_gameObject);
        }


        private void UpdateHpText () {
            // if (!_hpText) return;
            var sb = new StringBuilder (_hp.ToString ());
            if (Poison > 0) sb.Append ($" (-{Poison})");
            _hpText.text = sb.ToString ();
        }


        public bool Alive => !Despawned;
        
        
        public bool IsNear (Flame flame) {
            var flameHitbox = flame.Hitbox;
            return
                Circle.Overlap (flameHitbox, Head.Circle) ||
                Circle.Overlap (flameHitbox, Tail.Circle) ||
                Geom  .Overlap (flameHitbox, Body.Box);
        }


        private float DistanceTo (Flame flame) => (Position - flame.Position).Length - flame.Hitbox.R;


        public void ApplyFire (IList <Flame> flames) {
            foreach (var flame in flames) {
                if (flame.OnTrigger ()) _fireDamage++;
            }
            if (_fireDamage >= 100) {
                TakeDamage (_fireDamage / 100);
                _fireDamage %= 100;
            }
            
            var flyingSum = XY.Zero;
            int flyingCount = 0;
            
            Flame nearest = null;
            float minDistance = 0;
            
            foreach (var flame in flames) {
                if (flame.Idle) {
                    float distance = DistanceTo (flame);
                    if (distance < minDistance || nearest == null) {
                        nearest = flame;
                        minDistance = distance;
                    }
                }
                else {
                    flyingSum += flame.Velocity;
                    flyingCount++;
                }
            }

            var result = nearest == null ? XY.Zero : (Position - nearest.Position).WithLength (nearest.HP * 0.015f + 0.1f);
            if (flyingCount > 0) result += (flyingSum / flyingCount - Velocity) * 0.05f;
            TakeBlast (result);
        }


        public void ResetFire () {
            _fireDamage = 99;
        }


        public void TryApplyPoison (IPoison poison) {
            var poisonHitbox = poison.Hitbox;
            bool poisoned =
                Circle.Overlap (poisonHitbox, Head.Circle) ||
                Circle.Overlap (poisonHitbox, Tail.Circle) ||
                Geom  .Overlap (poisonHitbox, Body.Box);
            if (poisoned) AddPoisonUpTo (poison.Dose);
        }


        public void AddPoisonUpTo (int dose) {
            if (dose > Poison) Poison = dose;
        }


        public void AddPoison (int dose) {
            Poison += dose;
        }


        public void Update (TurnData td) 
        {
            Controller.Update (td);
            if(_weapon != null) 
                _weapon.Update (td);
            Sprite.DoUpdate ();
        }


        public override void OnBeforeCollision (Collision collision) {
            if (!collision.IsLandCollision && collision.Collider2.Entity.PassableFor (this)) return;
            
            var v1 = Velocity;
            var v2 = collision.IsLandCollision ? XY.Zero : collision.Collider2.Entity.Velocity;
            _previousVelocity = v1;
            _fallDamageCap    = (v1 - v2).Length - 10;
            
            CollisionHandler.OnBeforeCollision (collision);
        }


        public override void OnCollision (Collision collision) {
            if (_fallDamageCap > 0) {
                Fall ();
                float dv = (Velocity - _previousVelocity).Length;
                Debug.Log (dv); // 20 при лобовом столкновении, 8 и 5 при столкновении по касательной
                float fallDamage = Mathf.Min (_fallDamageCap, 0.4f * dv);
                TakeDamage (Mathf.CeilToInt (fallDamage)); // минимум 1, правильно?
                _fallDamageCap = 0; // от повторных срабатываний
            }
            
            CollisionHandler.OnCollision (collision);
        }


        public void TakeDamage (int damage) {
            UnequipWeapon ();
            
            // чтобы одновременные взрывы ничего не портили
            if (Controller is WormDeathCtrl) return;
            
            if (this == _.War.ActiveWorm) {
                _.War.EndTurn ();
            }
            _.War.Spawn (new FallingLabel (damage.ToString(), Team.Color, Position, XY.Zero));
            if      (damage < HP) HP -= damage;
            else if (HP > 0)      HP = 0;
        }


        public void TakeAxeDamage () {
            TakeDamage (Mathf.Clamp (HP / 2, 10, 60));
        }


        public void TakeBlast (XY impulse) {
            // чтобы одновременные взрывы ничего не портили - здесь особенно важно
            if (Controller is WormDeathCtrl) return;
            
            Velocity += impulse;
            // if (Velocity.SqrLength > Settings.WormFlySqrVelocity) Fly ();
            // else
            Fall ();
        }


        public void TakePoisonDamage () {
            if (Poison > 0) TakeDamage (Poison);
        }


        // вызывать только из контроллера, причем предварительно вызвать ExcludeEntities
        public Collision Cast (Collider c, XY v) {
            Collision temp, min = null;
            foreach (var o in _.War.CollisionSystem.FindObstacles (c, v, ObstacleFilter)) {
                temp = c.FlyInto (o, v);
                if (temp < min) min = temp;
            }
            temp = c.FlyInto (_.War.Land, min?.Offset ?? v);
            return temp ?? min;
        }
        
        
        // переходы состояний
        
        public void Walk () {
            Velocity         = XY.Zero;
            Controller       = new WormWalkCtrl ();
            CollisionHandler = new WormStandCH ();
        }


        public void Jump () {
            Poke ();
            Controller       = new WormJumpCtrl ();
            CollisionHandler = new WormJumpCH ();
        }


        public void Fall (bool poke = true) {
            if (poke) Poke ();
            Controller       = new WormFallCtrl (); // todo: посмотреть как в старом
            CollisionHandler = new WormFallCh ();
        }


        public void Fall (MobileEntity cause) {
            Poke (cause, false);
            Controller       = new WormFallCtrl (); // todo: посмотреть как в старом
            CollisionHandler = new WormFallCh ();
        }


        public void Explode () {
            Despawn ();
            _.War.Wait ();
            _.War.MakeExplosionSmall (Position);
        }


        public void Recover () {
            IdleAt           = _.War.Time;
            Controller       = new WormRecoverCtrl ();
            CollisionHandler = new WormStandCH ();
        }


        public void BeforeJump (XY vj) {
            Velocity         = XY.Zero;
            Controller       = new WormBeforeJumpCtrl (vj);
            CollisionHandler = new WormStandCH ();
        }


        public void AfterJump () {
            Velocity         = XY.Zero;
            Controller       = new WormAfterJumpCtrl ();
            CollisionHandler = new WormStandCH ();
        }


        public void Die () {
            Controller = new WormDeathCtrl ();
            // CollisionHandler = new...
            // короче я думаю надо сделать чтобы падающие червяки не могли сбить его и поменять контроллер
            // но в то же время логика игры вроде бы гарантирует что таковых не будет
        }
        
        
        // оружие всякое
        // возвращает true если нажалось

        public bool UseCriticalStrike () {
            if (!(Controller is WormJumpCtrl)) return false;
            CollisionHandler = new WormCriticalStrikeCH ();
            return true;
        }

    }


}
