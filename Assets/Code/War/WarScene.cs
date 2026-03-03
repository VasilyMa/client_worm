using Core;
using DataTransfer.Data;
using Math;
using MemoryPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UI.Elements;
using UI.Panels;
using UnityEngine;
using Utils;
using War.Arsenals;
using War.Entities;
//using War.Entities.Barrels;
using War.Systems.Blasts;
using War.Systems.Camera;
using War.Systems.Collisions;
using War.Systems.Damage;
using War.Systems.Debug;
using War.Systems.Fire;
using War.Systems.Input;
//using War.Systems.Nukes;
using War.Systems.Physics;
using War.Systems.Poison;
using War.Systems.Teams;
using War.Systems.Terrain;
using War.Systems.Terrain.Generation;
using War.Systems.Updating;
//using War.Systems.Water;
using War.Systems.Weapons;
using War.Systems.Worms;  
using Random = System.Random;


namespace War 
{

    public partial class WarScene : MonoBehaviour 
    {

        // ответственность класса:
        // хранить все системы и глобальные переменные боя
        // работа с системами в нужном порядке
        
        
        // системы:
        public DebugSystem     DebugSystem     = new DebugSystem     ();
        
        public UpdateSystem    UpdateSystem    = new UpdateSystem    ();
        public WormsSystem     WormsSystem     = new WormsSystem     ();
        public BlastSystem     BlastSystem     = new BlastSystem     ();
        public DamageSystem    DamageSystem    = new DamageSystem    ();
        public FireSystem      FireSystem      = new FireSystem      ();
        public PoisonSystem    PoisonSystem    = new PoisonSystem    ();
        public InputSystem     InputSystem;
        //public NukesSystem     NukesSystem     = new NukesSystem     ();
        public TeamsSystem     TeamsSystem;
        //public WaterSystem     WaterSystem     = new WaterSystem     ();
        public CollisionSystem CollisionSystem = new CollisionSystem ();
        public WeaponSystem    WeaponSystem    = new WeaponSystem    ();
        public Land            Land;
        public World           World;

        // не забываем в DoUpdate прописывать
        public War.Systems.Particles.ParticleSystem
                               SmokeFrontParticles,
                               SmokeBackParticles,
                               FireParticles;
        
        // компоненты юнити
        public WarAssets       Assets;
        public LandRenderer    LandRenderer;
        public TMP_Text        Hint;
        public TMP_Text        Clock;
        //public WindBar         WindBar;
        public WeaponInfo      WeaponInfo;
        public CameraWrapper   Camera;
        
        public ChatPanel       ChatPanel;
        public ArsenalPanel    ArsenalPanel;
        public GameResultPanel GameResultPanel;

        public ParticleSystem  SmokeFrontUnityParticles,
                               SmokeBackUnityParticles,
                               FireUnityParticles;

        // глобальные переменные:
        public       Random    Random; 
        private      float     _wind;
        public const float     Gravity = -Balance.Gravity; // потом уберу const когда будет меняться
        public       Worm      ActiveWorm { get; private set; }

        public static WarScene Instance;

        private InitialSessionData _sessionData;
        private GameInitData _data;

        private TurnData turnData;
        private bool _myTurn;

        private TurnState _turnState;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        { 
            PhotonRunHandler.Instance?.Init(this);
        }

        public void SetRemoteData(TurnData data)
        {
            turnData = data;

            Debug.Log($"Get data W {data.W} A {data.A} S {data.S} D {data.D} weapon index {data.Weapon} mouse X{data.XY.X} Y{data.XY.Y}");
        } 

        public IEnumerator RuntimeHandle(InitialSessionData data)
        {
            /*string[] names = { "Трарт", "Ллалл" };
            _.Random.Shuffle(names, 2 * 5);

            var colors = Enumerable.Range(0, 2).ToArray();
            _.Random.Shuffle(colors);

            var teams =
            Enumerable.Range(0, 2).
            Select<int, Team>(
                i => new LocalTeam(
                    colors[i],
                    Enumerable.Range(i * 5, 5).Select(j => names[j % names.Length]).ToArray()
                )
            ).
            ToArray();*/

            int playerID = PhotonInitializer.Instance.PlayerID;
            
            _sessionData = data;

            Debug.Log($"Player ID {playerID}");

            if (playerID == 1)
            {  
                _turnState = new TurnState(true, playerID);
            }
            else
            {
                _turnState = new TurnState(false, playerID);
            }

            string[] names = { "Трарт", "Ллалл" };
            _.Random.Shuffle(names, _sessionData.Players * _sessionData.Worms);

            var colors = Enumerable.Range(0, _sessionData.Players).ToArray();
            _.Random.Shuffle(colors);

            var teams =
            Enumerable.Range(0, _sessionData.Players)
            .Select<int, Team>(i =>
            {
                string[] wormsNames = Enumerable
                    .Range(i * _sessionData.Worms, _sessionData.Worms)
                    .Select(j => names[j % names.Length])
                    .ToArray();

                if (i == playerID - 1)
                    return new LocalTeam(i, wormsNames);
                else
                    return new RemoteTeam(i, wormsNames);
            })
            .ToArray();
             
            _data = new GameInitData(true, _.Random.Int(), teams, 0, 10);
             
            foreach (var step in InitializationRoutine())
            {
                yield return step;
            }

            InputSystem.Update();
            foreach (var step in UpdateRoutine())
            {
                yield return step;
                InputSystem.Update();
            }
            // почему так сделано? вот лог:
            //     UPDATE
            //     здесь проходит 1 такт
            //     WEAPON SELECTED
            //     GET TD (это действие закрывает арсенал)
            //     UPDATE (так как арсенал закрыт то система ввода запишет клик в TD

            foreach (var step in FinalizationRoutine())
            {
                yield return step;
            }
        }

        private IEnumerable InitializationRoutine ()
        {
            _.War = this;
            Random = new Random(_sessionData.Seed);

            // Логируем начало инициализации
            Core.Logger.LogGameInitialization(_sessionData.Seed, _sessionData.Players, _sessionData.Worms);
            
            SmokeFrontParticles = new Systems.Particles.ParticleSystem(SmokeFrontUnityParticles);
            SmokeBackParticles = new Systems.Particles.ParticleSystem(SmokeBackUnityParticles);
            FireParticles = new Systems.Particles.ParticleSystem(FireUnityParticles);

            TeamsSystem = new TeamsSystem(_data.Teams);
            InputSystem = new InputSystem(ArsenalPanel, ChatPanel);

            var generator = new LandGenerator(Random, null);
            Hint.text = "0%";
            int percent = 0;
            foreach (float progress in generator.Generate())
            {
                int newPercent = Mathf.FloorToInt(progress * 100);
                if (newPercent == percent) continue;
                percent = newPercent;
                Hint.text = percent + "%";
                yield return null;
            }

            Land = new Land(generator.Land, LandRenderer);
            World = new World();

            Random.Shuffle(generator.Spawns);
            int i = 0;

            foreach (var team in _data.Teams)
            {
                foreach (var worm in team.Worms)
                {
                    Spawn(worm, generator.Spawns[i++]);
                }
            }

            ArsenalPanel.Init(); 
            ArsenalPanel.Track(new FullArsenal());

            Core.Logger.LogBattleEvent("Инициализация завершена, начинается боевой цикл");
        }


        // нельзя назвать Update из-за б-гомерзкой юнити
        private void DoUpdate () {
            Time++;
            Core.Logger.UpdateGameTime(Time); // Обновляем игровое время в логгере
            
            TurnTime--;
            UpdateSystem       .Update (null);
            World              .Update ();
            FireSystem         .Update ();
            PoisonSystem       .Update ();
            
            SmokeBackParticles .Update ();
            SmokeFrontParticles.Update ();
            FireParticles      .Update ();
        }


        private void DoUpdate (TurnData td)
        {
            Time++;
            Core.Logger.UpdateGameTime(Time); // Обновляем игровое время в логгере

            if (Time < ReadyAt)
            {
                if (td != null && !td.Empty) ReadyAt = Time;
            }
            else if (!TurnPaused)
            {
                TurnTime--;
            }

            WeaponSystem.Update(td);
            UpdateSystem.Update(td);
            World.Update();
            FireSystem.Update();
            PoisonSystem.Update();

            SmokeBackParticles.Update();
            SmokeFrontParticles.Update();
            FireParticles.Update();
        }

        private void SyncUpdate()
        {
            Debug.Log($"This turn is mine: {_turnState.IsMineTurn}");

            if (_turnState.IsMineTurn)
            {
                turnData = TeamsSystem.GetCurrentTeam(_turnState.PlayerID).GetTurnData();

                var inputData = new InputData()
                {
                    X = turnData.XY.X,
                    Y = turnData.XY.Y,
                    W = turnData.W,
                    A = turnData.A,
                    S = turnData.S,
                    D = turnData.D,
                    MB = turnData.MB,
                    Weapon = turnData.Weapon,
                };

                PhotonRunHandler.Instance?.SendMessageTurnRPC(MemoryPackSerializer.Serialize(inputData));

                Debug.Log($"Set data W {inputData.W} A {inputData.A} S {inputData.S} D {inputData.D} Weapon Index {inputData.Weapon} Mouse X{inputData.X} Y{inputData.Y}");
            }

            DoUpdate(turnData);
        }

        private IEnumerable FinalizationRoutine() 
        { 
            ArsenalPanel.Hide();
            GameResultPanel.Show();
            
            // Логируем завершение и выводим путь к файлу
            Core.Logger.LogGameEnded(-1, "Игра завершена");
            
            string logPath = Core.Logger.GetLogFilePath();
            Debug.Log($"<color=green>[WarScene] ИГРА ЗАВЕРШЕНА</color>");
            Debug.Log($"<color=green>Лог-файл сохранён в:\n{logPath}</color>");
            
            yield break;
        }

        /// <summary>
        /// Спавнит сущность без параметров
        /// </summary>
        public void Spawn(Entity e) => e.OnSpawn();
        
        /// <summary>
        /// Спавнит мобильную сущность с позицией и скоростью
        /// </summary>
        public void Spawn(MobileEntity e, XY position, XY velocity = default(XY)) 
        {
            e.OnSpawn(position, velocity);
        }

        /// <summary>
        /// Логирует событие боя
        /// </summary>
        public void LogBattleEvent(string message)
        {
            Core.Logger.LogBattleEvent(message);
        }

        private void LogAllWormsState()
        {
            var wormsData = new List<(string name, int teamId, float x, float y, int hp)>();

            foreach (var team in _data.Teams)
            {
                foreach (var worm in team.Worms)
                {
                    if (!worm.Despawned)
                    {
                        wormsData.Add((worm.Name, team.TeamId, worm.Position.X, worm.Position.Y, worm.HP));
                    }
                }
            }

            Core.Logger.LogWormsState(wormsData);
        }

        private void Update()
        {
            // Debug: L - открыть папку логов
            if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            {
                Core.Logger.OpenLogsFolder();
            }

            // Debug: O - показать путь логов
            if (UnityEngine.Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log($"[WarScene] Логи находятся в: {Core.Logger.GetLogFilePath()}");
            }

            // Debug: M - открыть файл логов
            if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                Core.Logger.OpenCurrentLogFile();
            }
        }

        //private MobileEntity NewBarrel () {
        //    switch (Random.Int (3)) {
        //        case 0:  return new ExplosiveBarrel ();
        //        case 1:  return new PoisonBarrel    ();
        //        case 2:  return new FuelBarrel      ();
        //        default: return new GlueBarrel      ();
        //    }
        //} 

        //public float Wind {
        //     get { return _wind; }
        //    private set { WindBar.Wind = _wind = value; }
        //} 
    }

}
