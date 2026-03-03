using Math;
using UnityEngine;


namespace War.Systems.Camera {

    /* итак какие контроллеры камеры мы делаем
     *
     * ручной режим - камера остается неподвижной
     * мы можем двигать ее правой кнопкой мышки
     * или приближать и отдалять колесиком
     * возврат в режим по умолчанию по таймеру
     * 
     * когда мы ходим, контроллер привязан к червяку
     * причем камера двигается к краю если мы подводим туда мышку
     * колесико будет менять масштаб этого контроллера
     * нажатие пкм включает ручной режим
     * 
     * когда мы не ходим, работает автоматический режим
     * камера будет стараться захватить в кадр все интересные объекты
     * нажатие пкм включает ручной режим
     * отдаление работает как задумано
     * приближение включит ручной режим если объекты уже не попадают в кадр
     *
     * ограничение движения камеры
     * это будет прямоугольник
     * камера не должна вылезать за прямоугольник
     * для простоты его нужно подогнать под максимальный зум камеры
     * но это потом
     */

    public abstract class CameraController {

        public CameraWrapper Camera;
        
        
        public virtual void OnAttach () {}
        public virtual void Update   () {}

    }


    // обрабатывать только зум по колесику мышки, и переключаться по нажатию пкм в следующий режим
    public class IdleCameraController : CameraController {

        public override void Update () {
            Camera.HandleWheel (UnityEngine.Input.mouseScrollDelta.y);
            if (UnityEngine.Input.GetMouseButtonDown (1)) Camera.Controller = new ManualCameraController ();
        }

    }


    public class ManualCameraController : CameraController {

        private XY _clickPosition;
        private XY _target;


        public override void OnAttach () {
            _clickPosition = (XY) UnityEngine.Input.mousePosition;
            _target        = Camera.Target;
        }
        

        public override void Update () {
            Camera.HandleWheel (UnityEngine.Input.mouseScrollDelta.y);

            Camera.Target = _target + _clickPosition - (XY) UnityEngine.Input.mousePosition;
            
            if (UnityEngine.Input.GetMouseButtonUp (1)) Camera.Controller = new IdleCameraController ();
        }

    }


    public class BoundCameraController : CameraController {

        private XY    _anchor;
        public  float Vision;


        public BoundCameraController (XY anchor, float vision = 1) {
            _anchor = anchor;
            Vision  = vision;
        }
        

        public override void Update () {
            Camera.HandleWheel (UnityEngine.Input.mouseScrollDelta.y);

            Camera.Target = _anchor + ((XY) UnityEngine.Input.mousePosition - new XY (Screen.width, Screen.height) * 0.5f) * Vision;
            
            if (UnityEngine.Input.GetMouseButtonDown (1)) {
                Camera.Controller = new ManualCameraController ();
            }
        }

    }


    public class AutoCameraController : CameraController {

        private void ShowBox (Box box) {}

        
        public override void Update () {
            // todo написать логику слежения за объектами и опять же колесико
            
            if (UnityEngine.Input.GetMouseButtonDown (1)) {
                Camera.Controller = new ManualCameraController ();
            }
        }

    }

}
