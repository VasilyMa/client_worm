using War.OldWeapons.Airstrikes;
using War.OldWeapons.CloseCombat;
using War.OldWeapons.Firearms;
using War.OldWeapons.Heavy;
using War.OldWeapons.Launched;
using War.OldWeapons.Spells;
using War.OldWeapons.Thrown;
using War.OldWeapons.Transport;
using War.OldWeapons.Utils;


// ReSharper disable RedundantArgumentDefaultValue


namespace War.Arsenals {

    public class NormalArsenal : Arsenal {

        public NormalArsenal () {
            const int inf = -1;
            
            AddAmmo (BazookaWpn      .Descriptor, inf);
            AddAmmo (MineGunWpn      .Descriptor, 4);
            AddAmmo (MultiLauncherWpn.Descriptor, 4);
            AddAmmo (HomingMissileWpn.Descriptor, 1);

            AddAmmo (GrenadeWpn      .Descriptor, inf);
            AddAmmo (LimonkaWpn      .Descriptor, 4);
            AddAmmo (MolotovWpn      .Descriptor, 2);
            AddAmmo (GasGrenadeWpn   .Descriptor, 1);
            AddAmmo (GhostGrenadeWpn .Descriptor, 2);
            
            AddAmmo (MachineGunWpn   .Descriptor, inf);
            AddAmmo (BlasterWpn      .Descriptor, 4);
            AddAmmo (PistolWpn       .Descriptor, inf);
            AddAmmo (CrossbowWpn     .Descriptor, 2);
            AddAmmo (HeatGunWpn      .Descriptor, 2);

            AddAmmo (FirePunchWpn    .Descriptor, inf);
            AddAmmo (FingerWpn       .Descriptor, inf);
            AddAmmo (GolfClubWpn     .Descriptor, 1);
            AddAmmo (AxeWpn          .Descriptor, 2);
            AddAmmo (FishingRodWpn   .Descriptor, 1);
            AddAmmo (GlueGunWpn      .Descriptor, 1);

            AddAmmo (MineWpn         .Descriptor, inf);
            AddAmmo (DynamiteWpn     .Descriptor, 1);
            AddAmmo (PoisonEmitterWpn.Descriptor, 1);
            AddAmmo (FrogWpn         .Descriptor, 1);
            AddAmmo (WalkingBombWpn  .Descriptor, 1);
            AddAmmo (MoleWpn         .Descriptor, 1);

            AddAmmo (AirstrikeWpn    .Descriptor, 1);
            AddAmmo (PoisonStrikeWpn .Descriptor, 1);
            AddAmmo (MoleStrikeWpn   .Descriptor, 1);
            
            AddAmmo (ErosionWpn      .Descriptor, 1);

            AddAmmo (RopeWpn         .Descriptor, 4);
            AddAmmo (ParachuteWpn    .Descriptor, 4);
            AddAmmo (JumperWpn       .Descriptor, 4);
            AddAmmo (JetpackWpn      .Descriptor, 1);
            AddAmmo (TeleportWpn     .Descriptor, 2);
            AddAmmo (WormSelectWpn   .Descriptor, 2);

            AddAmmo (DrillWpn        .Descriptor, inf);
            AddAmmo (GirderWpn       .Descriptor, 4);
            AddAmmo (MagnetWpn       .Descriptor, 1);
            AddAmmo (TurretWpn       .Descriptor, 1);
            AddAmmo (MedikitWpn      .Descriptor, 2);
            AddAmmo (SkipTurnWpn     .Descriptor, inf);
            AddAmmo (SurrenderWpn    .Descriptor, inf);
        }

    }

}
