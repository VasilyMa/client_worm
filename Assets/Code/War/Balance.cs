using Core;


namespace War {

    public static class Balance {

        public const float Gravity    = 0.3f;
        public const float WindFactor = 0.1f;

        public const int RetreatTime = 3 * Settings.FPS;

        public const int ShellMassRank   = -10;
        public const int GrenadeMassRank = 0;
        public const int MineMassRank    = 10;
        public const int WormMassRank    = 20;
        public const int CrateMassRank   = 30;

        public const float ThrowSpeed      = 30;
        public const float LiquidSpeed     = 20;
        public const float ClusterMinSpeed = 5;
        public const float ClusterMaxSpeed = 10;

        public const float SmokeWindFactor  = 1 / 6f;
        public const float LiquidWindFactor = 1 / 20f;

        public const float FingerReach = 20;
        public const float MeleeReach  = 30;
        public const float RayLength   = 10000;

        public const float FrogRadius         = 7;
        public const float GrenadeRadius      = 6;
        // public const float LargeGrenadeRadius = 8; - радиус червяка 7 и радиус гранаты не может его превышать
        public const float ShellRadius        = 4;
        public const float ClusterRadius      = 2;
        public const float NukeMissileRadius  = 15;
        
        public const float MeteorRadiusSmall = 10;
        public const float MeteorRadius      = 15;
        public const float MeteorRadiusLarge = 25;

        public const float BomberHeight = 500;
        public const float BomberSpeed  = 30;

        public const int DamageBullet           = 1;
        public const int DamageBulletHeavy      = 2;
        public const int DamageCrossbowBolt     = 3;
        public const int DamageHeatRay          = 5;
        public const int DamageExplosionTiny    = 10;
        public const int DamageExplosionSmall   = 15;
        public const int DamageExplosion        = 25;
        public const int DamageExplosionLarge   = 40;
        public const int DamageExplosionHuge    = 50;
        public const int DamageExplosionNuclear = 150;
        public const int DamageMelee            = 15;

        public const int PoisonCrossbowBolt = 3;

        public const int HealingMedikit = 15;

        public const int SawInvuls = 15;

        // todo нотация неоднородна - определиться писать GrenadeRaduis или RadiusExplosion

        public const int HoleBullet           = 5;
        public const int HoleFire             = 5;
        public const int HoleExplosionTiny    = 2 * DamageExplosionTiny;
        public const int HoleExplosionSmall   = 2 * DamageExplosionSmall;
        public const int HoleExplosion        = 2 * DamageExplosion;
        public const int HoleExplosionLarge   = 2 * DamageExplosionLarge;
        public const int HoleExplosionHuge    = 2 * DamageExplosionHuge;
        public const int HoleExplosionNuclear = 2 * DamageExplosionNuclear;

        public const int RadiusExplosionTiny    = 4 * DamageExplosionTiny;
        public const int RadiusExplosionSmall   = 4 * DamageExplosionSmall;
        public const int RadiusExplosion        = 4 * DamageExplosion;
        public const int RadiusExplosionLarge   = 4 * DamageExplosionLarge;
        public const int RadiusExplosionHuge    = 4 * DamageExplosionHuge;
        public const int RadiusExplosionNuclear = 4 * DamageExplosionNuclear;

        public const float PushBullet           = 2;
        public const float PushBulletHeavy      = 3;
        public const float PushExplosionTiny    = 0.5f * DamageExplosionTiny;
        public const float PushExplosionSmall   = 0.5f * DamageExplosionSmall;
        public const float PushExplosion        = 0.5f * DamageExplosion;
        public const float PushExplosionLarge   = 0.5f * DamageExplosionLarge;
        public const float PushExplosionHuge    = 0.5f * DamageExplosionHuge;
        public const float PushExplosionNuclear = 0.5f * DamageExplosionNuclear;
        public const float PushMeleeWeak        = 3;
        public const float PushMelee            = 10;
        public const float PushMeleeStrong      = 15;

        public const float WormFallVelocity = 15;

    }

}