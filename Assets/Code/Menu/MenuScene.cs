using Core;
using UI.Panels;
using UnityEngine;


namespace Menu {

    public class MenuScene : MonoBehaviour {

        [SerializeField] private MainPanel    _mainPanel;
        [SerializeField] private ConnectPanel _connectPanel;
        [SerializeField] private PlayPanel    _playPanel;
        [SerializeField] private NamesPanel   _editTeamPanel;
        [SerializeField] private HelpPanel    _helpPanel;
        [SerializeField] private DonatePanel  _donatePanel;
        [SerializeField] private RoomPanel    _roomPanel;
        


        private void Awake () {
            _.Menu = this;
            
            _mainPanel.Show (true);
        }


        private void OnDestroy () {
            _.Menu = null;
        }


        public void ShowMain () {
            _connectPanel .Hide ();
            _playPanel    .Hide ();
            _editTeamPanel.Hide ();
            _helpPanel    .Hide ();
            _donatePanel  .Hide ();
            _roomPanel    .Hide ();
            
            _mainPanel.Show ();
        }


        public void ShowConnect () {
            _mainPanel.Hide ();
            _connectPanel.Show ();
        }


        public void ShowPlay () {
            _mainPanel.Hide ();
            _playPanel.Show ();
        }


        public void ShowEditTeam () {
            _mainPanel.Hide ();
            _editTeamPanel.Show ();
        }


        public void ShowHelp () {
            _mainPanel.Hide ();
            _helpPanel.Show ();
        }


        public void ShowDonate () {
            _mainPanel.Hide ();
            _donatePanel.Show ();
        }


        public void ShowRoom () {
            _mainPanel.Hide ();
            _roomPanel.Show ();
        }

    }

}
