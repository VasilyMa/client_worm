using System.Collections;
using UnityEngine;


namespace UI.Elements {

    public class UiElement : MonoBehaviour {

        private Coroutine _coroutine;


        public void Push () {
            StopCoroutine ();
            StartCoroutine (PushCoroutine ());
        }


        public void StopCoroutine () {
            if (_coroutine != null) StopCoroutine (_coroutine);
        }


        protected virtual IEnumerator PushCoroutine () => null;

    }

}
