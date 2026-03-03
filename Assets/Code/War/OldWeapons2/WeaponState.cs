namespace War.OldWeapons2 {

    public enum WeaponState {

//        Invalid,   // когда атака прервана
//        Idle1,     // когда оружие мы ждем нажатия лкм
//        Idle2,     // ждем отпускания лкм
//        Adjusting, // когда идет этап установки силы выстрела
//        Shooting   // оружие в процессе стрельбы
        
        
        Invalid            , // когда оружие не взято
        BeforeCanLockOn    , // ждем отпускания мышки после чего задаем цель
        CanLockOn          , // указываем цель
        BeforeCanAttack    , // ждем отпускания мышки после чего атакуем
        CanAttack          , // можно стрелять
        Adjusting          , // нажата атака, мощность регулируется
        Shooting           , // оружие стреляет
        

    }

}
