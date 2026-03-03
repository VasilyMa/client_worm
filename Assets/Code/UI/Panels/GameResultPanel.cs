using Core;
using TMPro;
using UI.Elements;
using UnityEngine;


namespace UI.Panels {

    public class GameResultPanel : SlidingPanel {

        [SerializeField] private Button   _buttonOk;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _buttonText;
        

        protected override void Activate () {
            _buttonOk.onLeftClick.AddListener (ClickedOk);
        }


        protected override void OnDestroy () {
            _buttonOk.onLeftClick.RemoveListener (ClickedOk);
        }


        private void ClickedOk () {
            _.SceneSwitcher.Load (Scenes.Menu, true); // пропустить меню подключения
        }


        public void ShowVictory () {
            _text.text = "Победа";
            _buttonText.text = "Закрыть";
            Show ();
        }


        public void ShowDraw () {
            _text.text = "Ничья";
            _buttonText.text = "Ясно";
            Show ();
        }


        public void ShowDefeat () {
            _text.text = "Поражение";
            _buttonText.text = "Ясно";
            Show ();
        }


        public void ShowDesync () {
            _text.text = "Рассинхронизация";
            _buttonText.text = "Ясно";
            Show ();
        }

    }

}
