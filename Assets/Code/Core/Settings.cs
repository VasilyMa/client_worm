namespace Core {

    public static class Settings {

        public const int   FPS                         = 60;

        public const float WormSlideXHysteresis        = 5;

        public const int   PhysicsIterations           = 5;
        public const float PhysicsPrecision            = 0.01f;
        public const int   NumericMethodsIterations    = 8;
        public const float NumericMethodsPrecision     = 0.01f;

        public const float EntityAnchorLerpCoefficient = 0.05f;
        public const float EntityAnchorSqrDistance     = 25f;
        public const int   EntityIdleTime              = 30;

        public const float WormSpinSqrVelocity         = 100;
        public const float WormFlySqrVelocity          = 25;

    }

}
