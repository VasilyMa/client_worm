using Core;
using Fusion;
using System.Threading.Tasks;
using UI.Elements;
using UnityEngine;


namespace UI.Panels
{

    public class MainPanel : SlidingPanel
    {

        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _editTeamButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _donateButton;


        protected override void Activate()
        {
            _connectButton.Interactable = false;
            _connectButton.onLeftClick.AddListener(ClickedConnect);
            _playButton.onLeftClick.AddListener(ClickedPlay);
            _editTeamButton.onLeftClick.AddListener(ClickedEditTeam);
            _helpButton.onLeftClick.AddListener(ClickedHelp);
            _donateButton.onLeftClick.AddListener(ClickedDonate);
        }


        protected override void OnDestroy()
        {
            _connectButton.onLeftClick.RemoveListener(ClickedConnect);
            _playButton.onLeftClick.RemoveListener(ClickedPlay);
            _editTeamButton.onLeftClick.RemoveListener(ClickedEditTeam);
            _helpButton.onLeftClick.RemoveListener(ClickedHelp);
            _donateButton.onLeftClick.RemoveListener(ClickedDonate);
        }


        private static void ClickedConnect() => _.Menu.ShowConnect();
        private void ClickedPlay()
        {
            _playButton.Interactable = false;
            var sessoion = StartNetwork();
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

        private static void ClickedEditTeam() { } // => _.Menu.ShowEditTeamMenu   ();
        private static void ClickedHelp() => _.Menu.ShowHelp();
        private static void ClickedDonate() => _.Menu.ShowDonate();

    }

}