using UnityEngine;
using UnityEngine.UI;


namespace UI.Panels {

    public class ChatPanel : Panel {

        [SerializeField] private Graphic _background;
        [SerializeField] private Graphic _border;
        [SerializeField] private Graphic _text;
        [Space] //
        [SerializeField] private Color _activeBackground   = new Color32 (0x00, 0x55, 0x80, 0xbf);
        [SerializeField] private Color _inactiveBackground = new Color32 (0x00, 0x55, 0x80, 0x00);
        [SerializeField] private Color _activeBorder       = Color.white;
        [SerializeField] private Color _inactiveBorder     = new Color (1, 1, 1, 0);
        [SerializeField] private Color _activeText         = Color.white;
        [SerializeField] private Color _inactiveText       = new Color (1, 1, 1, 0.5f);


        // возвращает true, если сообщение присутствовало, и false, если нет, и чат надо закрыть
        public bool TrySend () {
            return false;
        }


        protected override void Adjust () {
            _background.color = Color.Lerp (_inactiveBackground, _activeBackground, _openness);
            _border.color     = Color.Lerp (_inactiveBorder,     _activeBorder,     _openness);
            _text.color       = Color.Lerp (_inactiveText,       _activeText,       _openness);
        }

    }

}
