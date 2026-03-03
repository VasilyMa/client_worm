using System.Collections.Generic;
using War.OldWeapons.Airstrikes;
using War.OldWeapons.CloseCombat;
using War.OldWeapons.Firearms;
using War.OldWeapons.Heavy;
using War.OldWeapons.Launched;
using War.OldWeapons.Spells;
using War.OldWeapons.Thrown;
using War.OldWeapons.Transport;
using War.OldWeapons.Utils;


namespace War.OldWeapons {

    public static class Weapons {

        private static List <WeaponDescriptor> _list = new List <WeaponDescriptor> {null};
        public static IReadOnlyList <WeaponDescriptor> List => _list;


        public static void Init () {
            Register (BazookaWpn        .Descriptor);
            Register (PlasmaGunWpn      .Descriptor);
            Register (MineGunWpn        .Descriptor);
            Register (HomingMissileWpn  .Descriptor);
            Register (MultiLauncherWpn  .Descriptor);
            Register (CryoGunWpn        .Descriptor);
            Register (BirdLauncherWpn   .Descriptor);
            
            Register (GrenadeWpn        .Descriptor);
            Register (LimonkaWpn        .Descriptor);
            Register (MolotovWpn        .Descriptor);
            Register (GasGrenadeWpn     .Descriptor);
            Register (GhostGrenadeWpn   .Descriptor);
            Register (FlashbangWpn      .Descriptor);
            Register (HolyHandGrenadeWpn.Descriptor);
            
            Register (MachineGunWpn     .Descriptor);
            Register (BlasterWpn        .Descriptor);
            Register (PistolWpn         .Descriptor);
            Register (CrossbowWpn       .Descriptor);
            Register (HeatGunWpn        .Descriptor);
            Register (UltraRifleWpn     .Descriptor);
            Register (GsomRaycasterWpn  .Descriptor);
            
            Register (FirePunchWpn      .Descriptor);
            Register (FingerWpn         .Descriptor);
            Register (AxeWpn            .Descriptor);
            Register (GolfClubWpn       .Descriptor);
            Register (FishingRodWpn     .Descriptor);
            Register (GlueGunWpn        .Descriptor);
            Register (FlamethrowerWpn   .Descriptor);
            
            Register (MineWpn           .Descriptor);
            Register (DynamiteWpn       .Descriptor);
            Register (PoisonEmitterWpn  .Descriptor);
            Register (FrogWpn           .Descriptor);
            Register (WalkingBombWpn    .Descriptor);
            Register (MoleWpn           .Descriptor);
            Register (SuperfrogWpn      .Descriptor);
            
            Register (AirstrikeWpn      .Descriptor);
            Register (NapalmStrikeWpn   .Descriptor);
            Register (PoisonStrikeWpn   .Descriptor);
            Register (MoleStrikeWpn     .Descriptor);
            Register (MineStrikeWpn     .Descriptor);
            Register (VacuumBombWpn     .Descriptor);
            Register (NukeWpn           .Descriptor);
            
            Register (ErosionWpn        .Descriptor);
            Register (FloodWpn          .Descriptor);
            Register (EarthquakeWpn     .Descriptor);
            Register (BulletHellWpn     .Descriptor);
            Register (MindControlWpn    .Descriptor);
            Register (MassHealingWpn    .Descriptor);
            Register (ArmageddonWpn     .Descriptor);
            
            Register (RopeWpn           .Descriptor);
            Register (ParachuteWpn      .Descriptor);
            Register (JumperWpn         .Descriptor);
            Register (JetpackWpn        .Descriptor);
            Register (TeleportWpn       .Descriptor);
            Register (MassTeleportWpn   .Descriptor);
            Register (WormSelectWpn     .Descriptor);
            
            Register (DrillWpn          .Descriptor);
            Register (GirderWpn         .Descriptor);
            Register (MagnetWpn         .Descriptor);
            Register (TurretWpn         .Descriptor);
            Register (MedikitWpn        .Descriptor);
            Register (SkipTurnWpn       .Descriptor);
            Register (SurrenderWpn      .Descriptor);
            
            // дальше могут идти специальные оружия типа уникальные для боссов или же созданные игроками
        }


        private static void Register (WeaponDescriptor desc) {
            if (desc == null) return;
            desc.Id = _list.Count;
            _list.Add (desc);
        }

    }

}
