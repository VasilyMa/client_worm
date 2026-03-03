using UnityEngine;


namespace War.Indication {

    public class NuclearCrosshair : MonoBehaviour {

        private int _references = 1;


        public int References {
            get { return _references; }
            set {
                _references = value;
                if (_references <= 0) Destroy (gameObject);
            }
        }

        public void PlayLockOnAnimation () {}

    }

}
