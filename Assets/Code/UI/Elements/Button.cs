using UnityEngine.Events;
using UnityEngine.UI;


namespace UI.Elements {

    // обычный элемент у которого есть события при нажатии разных кнопок
    public class Button : ButtonBase <Graphic> {

        public bool allowLeftClick = true;
        public bool allowRightClick;
        
        protected override bool AllowLeftClick  => allowLeftClick;
        protected override bool AllowRightClick => allowRightClick;


        public UnityEvent onLeftClick;
        public UnityEvent onRightClick;
        
        protected override void OnLeftClick  () => onLeftClick .Invoke ();
        protected override void OnRightClick () => onRightClick.Invoke ();

    }

}
