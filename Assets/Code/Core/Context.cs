using DataTransfer;
using Net;
using UnityEngine;
using War.Weapons;
using Random = System.Random;


namespace Core {

    public class Context : MonoBehaviour {

        private void Awake () {
            DontDestroyOnLoad (this);
            Application.targetFrameRate = Settings.FPS;

            _.Random = new Random ();

            DTO              .Init ();
            WeaponDescriptors.Init ();

            _.Connection = gameObject.AddComponent <Connection> ();
            _.Connection.Authorize(0,"ws://localhost:7675");

            _.SceneSwitcher = new SceneSwitcher ();
            _.SceneSwitcher.Load (Scenes.Menu, false);
        }

    }

}
