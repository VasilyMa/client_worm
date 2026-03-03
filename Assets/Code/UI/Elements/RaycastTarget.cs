using UnityEngine.UI;


namespace UI.Elements {

    public class RaycastTarget : Graphic {

        protected override void OnPopulateMesh (VertexHelper vh) => vh.Clear ();

    }

}