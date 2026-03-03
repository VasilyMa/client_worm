using Core;
using DataTransfer.Client;
using Fusion;
using System;
using System.Linq;
using System.Threading.Tasks;
using UI.Elements;
using UnityEngine;
using Utils;
using War;
using War.Systems.Teams;


namespace UI.Panels {

    public class PlayPanel : SlidingPanel {

        [SerializeField] private Choice _modeChoice;
        [SerializeField] private Choice _playersChoice;
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _cancelButton;

        
        protected override void Activate () {
            _modeChoice   .OnValueChanged.AddListener (Validate);
            _playersChoice.OnValueChanged.AddListener (Validate);
            _okButton     .onLeftClick   .AddListener (OkClicked);
            _cancelButton .onLeftClick   .AddListener (CancelClicked);
        }


        protected override void OnDestroy () {
            _modeChoice   .OnValueChanged.RemoveListener (Validate);
            _playersChoice.OnValueChanged.RemoveListener (Validate);
            _okButton     .onLeftClick   .RemoveListener (OkClicked);
            _cancelButton .onLeftClick   .RemoveListener (CancelClicked);
        }


        private void Validate (int _) {
            bool ok = _playersChoice.SelectedOption > 0 || _modeChoice.SelectedOption == 1;
            // игроков больше 1 либо локальный режим
            _modeChoice.ErrorFlag = _playersChoice.ErrorFlag = !ok;
            _okButton.Interactable = ok;
        }


        private void OkClicked () {
            switch (_modeChoice.SelectedOption) {
                case 0:
                    //_.Connection.Send (new JoinLobbyCmd (_playersChoice.SelectedOption + 1));
                    //_.Menu.ShowRoom ();
                    var session = StartNetwork();
                    break;
                case 1: // локально
                    StartLocalGame (_playersChoice.SelectedOption + 1);
                    break;
                case 2: // с ботами
                    throw new NotImplementedException ("Боты еще не сделаны");
                default:
                    throw new ArgumentException ();
            }
        }
         
        private async Task StartNetwork()
        {
            await startMatchmaking();
        }

        async Task startMatchmaking()
        {
            var session = new SessionParams
            {
                Mode = GameMode.AutoHostOrClient,
                RoomName = $"MatchMaking",
                ScenePath = "battle_scene_01",
                SceneIndex = 2,
                ProvideInput = true,
                TargetPlayerCount = 2
            };
             
            await PhotonInitializer.Instance.StartSession(session);
        }


        private static void StartLocalGame (int players) 
        {
            const int worms = 5;
        
            string [] names = {"Трарт", "Ллалл"};
            _.Random.Shuffle (names, players * worms);
            
            var colors = Enumerable.Range (0, players).ToArray ();
            _.Random.Shuffle (colors);

            var teams =
            Enumerable.Range (0, players).
            Select <int, Team> (
                i => new LocalTeam (
                    colors [i],
                    Enumerable.Range (i * worms, worms).Select (j => names [j % names.Length]).ToArray ()
                )
            ).
            ToArray ();

            _.SceneSwitcher.Load (
                Scenes.War,
                new GameInitData (true, _.Random.Int (), teams, 0, 10)
            );
        }


        private void CancelClicked () => _.Menu.ShowMain ();

    }

}