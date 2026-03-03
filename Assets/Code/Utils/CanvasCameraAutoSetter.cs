using UnityEngine;


namespace Utils {

    public class CanvasCameraAutoSetter : MonoBehaviour {

        private void Awake () {
//            GetComponent <Canvas> ().worldCamera = _.Camera.Camera;
            Destroy (this);
        }

    }

}