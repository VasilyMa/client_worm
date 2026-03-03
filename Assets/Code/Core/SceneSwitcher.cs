using UnityEngine.SceneManagement;


namespace Core {

    public class SceneSwitcher {

        public SceneSwitcher () {
            _.SceneSwitcher = this;
        }


        public object[] Data { get; private set; }


        public void Load (string scene, params object[] data) {
            Data = data;
            SceneManager.LoadScene (scene);
        }

    }

}
