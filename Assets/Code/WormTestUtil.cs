using System;
using System.Collections;
using UI.Elements;
using UnityEngine;
using War.Components.Other;


// ReSharper disable IteratorNeverReturns


[Obsolete]
public class WormTestUtil : MonoBehaviour {

    [SerializeField]
    private Button _stand, _walk, _beforeJump, _jump, _afterJump, _fly, _spin, _slide, _recover, _death;


    [SerializeField] private WormSprite2 _worm;


    private Coroutine _coroutine;


    private void Awake () {
        _stand     .onLeftClick .AddListener (() => SwapCoroutine (TestStand      (true )));
        _stand     .onRightClick.AddListener (() => SwapCoroutine (TestStand      (false)));
        
        _walk      .onLeftClick .AddListener (() => SwapCoroutine (TestWalk       ()));
        _beforeJump.onLeftClick .AddListener (() => SwapCoroutine (TestBeforeJump ()));
        _jump      .onLeftClick .AddListener (() => SwapCoroutine (TestJump       ()));
        _afterJump .onLeftClick .AddListener (() => SwapCoroutine (TestAfterJump  ()));
        _fly       .onLeftClick .AddListener (() => SwapCoroutine (TestFly        ()));
        _spin      .onLeftClick .AddListener (() => SwapCoroutine (TestSpin       ()));
        _slide     .onLeftClick .AddListener (() => SwapCoroutine (TestSlide      ()));
        _recover   .onLeftClick .AddListener (() => SwapCoroutine (TestRecover    ()));
        _death     .onLeftClick .AddListener (() => SwapCoroutine (TestDeath      ()));
    }


    private IEnumerator TestStand (bool facesRight) {
//        _worm.Stand (facesRight, 0, 0);
        yield break;
    }


    private IEnumerator TestWalk () {
        for (int i = 16; ; i++) {
//            _worm.Walk (true, 0, 0, System.Math.Abs (i % 22 - 11));
            yield return null;
        }
    }


    private IEnumerator TestBeforeJump () {
        for (int i = 0; i < 8; i++) {
//            _worm.BeforeJump (true, 0, 0, i);
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestJump () {
        for (int i = 0; ; i++) {
//            _worm.Jump (true, System.Math.Abs (i % 4 - 2));
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestAfterJump () {
        for (int i = 0; i < 8; i++) {
//            _worm.AfterJump (true, 0, 0, i);
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestFly () {
        for (int i = 0; ; i++) {
//            _worm.Fly (true, i*i, System.Math.Abs (i % 4 - 2));
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestSpin () {
        for (int i = 0; ; i++) {
//            _worm.Spin (true, -i * i);
            yield return null;
        }
    }


    private IEnumerator TestSlide () {
        for (int i = 0; ; i++) {
//            _worm.Slide (true, System.Math.Abs (i % 4 - 2));
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestRecover () {
        for (int i = 0; i < 30; i++) {
//            _worm.Recover (true, i);
            yield return null;
            yield return null;
        }
    }


    private IEnumerator TestDeath () {
        for (int i = 0; i < 15; i++) {
//            _worm.Death (true, i);
            yield return null;
            yield return null;
        }
        for (int i = 0; i < 30; i++) {
            yield return null;
        }
        for (int i = 15; i < 33; i++) {
//            _worm.Death (true, i);
            yield return null;
            yield return null;
        }
    }


    private void SwapCoroutine (IEnumerator coroutine) {
        if (_coroutine != null) StopCoroutine (_coroutine);
        _coroutine = StartCoroutine (coroutine);
    }

}
