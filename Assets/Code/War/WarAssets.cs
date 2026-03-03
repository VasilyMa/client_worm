using System;
using UnityEngine;
using UnityEngine.Serialization;


namespace War {

    public class WarAssets : MonoBehaviour {

        // [SerializeField] public GameObject Grenade;
        [SerializeField] public GameObject Mine;
        [SerializeField] public GameObject Worm;
        [SerializeField] public GameObject Bomber;
        
        [Space]
        
        [FormerlySerializedAs ("AreaAboveWorm")]
        [SerializeField] public GameObject TextAreaTop;
        [SerializeField] public GameObject TextAreaBottom;
        [SerializeField] public GameObject Canvas;
        [SerializeField] public GameObject Text24;
        [SerializeField] public GameObject LineCrosshair;
        [SerializeField] public GameObject PointCrosshair;
        [SerializeField] public GameObject NuclearCrosshair;
        
        [Space]
        
        [SerializeField] public GameObject ExplosiveBarrel;
        [SerializeField] public GameObject FuelBarrel;
        [SerializeField] public GameObject PoisonBarrel;
        [SerializeField] public GameObject GlueBarrel;
        
        [Space]
        
        #region Icons
        [SerializeField] public GameObject BazookaIcon;
        [SerializeField] public GameObject MineGunIcon;
        [SerializeField] public GameObject HomingMissileIcon;
        [SerializeField] public GameObject SawGunIcon;
        [SerializeField] public GameObject MiniRocketsIcon;
        [SerializeField] public GameObject CryoGunIcon;
        [SerializeField] public GameObject BirdLauncherIcon;
        [Obsolete]
        [SerializeField] public GameObject PlasmaGunIcon;
        
        [SerializeField] public GameObject GrenadeIcon;
        [SerializeField] public GameObject LimonkaIcon;
        [SerializeField] public GameObject MolotovIcon;
        [SerializeField] public GameObject GhostGrenadeIcon;
        [SerializeField] public GameObject HolyHandGrenadeIcon;
        [SerializeField] public GameObject GasGrenadeIcon;
        [SerializeField] public GameObject FlashbangIcon;
        [Obsolete]
        [SerializeField] public GameObject ControlledGrenadeIcon;
        
        [SerializeField] public GameObject AssaultRifleIcon;
        [SerializeField] public GameObject BlasterIcon;
        [SerializeField] public GameObject PistolIcon;
        [SerializeField] public GameObject HeatGunIcon;
        [SerializeField] public GameObject CrossbowIcon;
        [SerializeField] public GameObject UltraRifleIcon;
        [SerializeField] public GameObject GsomRaycasterIcon;
        
        [SerializeField] public GameObject FirePunchIcon;
        [SerializeField] public GameObject FingerIcon;
        [FormerlySerializedAs ("HammerIcon")] [SerializeField] public GameObject GolfClubIcon;
        [SerializeField] public GameObject AxeIcon;
        [SerializeField] public GameObject FlamethrowerIcon;
        [SerializeField] public GameObject FishingRodIcon;
        [SerializeField] public GameObject GlueGunIcon;
        
        [SerializeField] public GameObject MineIcon;
        [SerializeField] public GameObject DynamiteIcon;
        [SerializeField] public GameObject PoisonEmitterIcon;
        [SerializeField] public GameObject TurretIcon;
        [SerializeField] public GameObject FrogIcon;
        [SerializeField] public GameObject MoleIcon;
        [SerializeField] public GameObject SuperfrogIcon;
        [SerializeField] public GameObject WalkingBombIcon;
        
        [SerializeField] public GameObject AirstrikeIcon;
        [SerializeField] public GameObject NapalmStrikeIcon;
        [SerializeField] public GameObject MineStrikeIcon;
        [SerializeField] public GameObject MoleStrikeIcon;
        [SerializeField] public GameObject PoisonStrikeIcon;
        [SerializeField] public GameObject VacuumBombIcon;
        [SerializeField] public GameObject NukeIcon;
        
        [SerializeField] public GameObject ErosionIcon;
        [SerializeField] public GameObject FloodIcon;
        [SerializeField] public GameObject EarthquakeIcon;
        [SerializeField] public GameObject BulletHellIcon;
        [SerializeField] public GameObject MindControlIcon;
        [SerializeField] public GameObject MassDamageIcon;
        [SerializeField] public GameObject ArmageddonIcon;
        
        [SerializeField] public GameObject RopeIcon;
        [SerializeField] public GameObject ParachuteIcon;
        [SerializeField] public GameObject JumperIcon;
        [SerializeField] public GameObject JetpackIcon;
        [SerializeField] public GameObject TeleportIcon;
        [SerializeField] public GameObject MassTeleportIcon;
        [SerializeField] public GameObject WormSelectIcon;
        
        [SerializeField] public GameObject DrillIcon;
        [SerializeField] public GameObject GirderIcon;
        [SerializeField] public GameObject MagnetIcon;
        [SerializeField] public GameObject MedikitIcon;
        [SerializeField] public GameObject MassHealingIcon;
        [SerializeField] public GameObject SkipTurnIcon;
        [SerializeField] public GameObject SurrenderIcon;
        #endregion
        
        [Space]

        #region Weapons
        [SerializeField] public GameObject BazookaWeapon;
        [SerializeField] public GameObject MinegunWeapon;
        [SerializeField] public GameObject HomingMissileWeapon;
        [SerializeField] public GameObject SawGunWeapon;
        [SerializeField] public GameObject MiniRocketsWeapon;
        [SerializeField] public GameObject CryogunWeapon;
        [SerializeField] public GameObject BirdLauncherWeapon;
        
        [SerializeField] public GameObject GrenadeWeapon;
        [SerializeField] public GameObject LimonkaWeapon;
        [SerializeField] public GameObject MolotovWeapon;
        [SerializeField] public GameObject GhostGrenadeWeapon;
        [SerializeField] public GameObject GasGrenadeWeapon;
        [SerializeField] public GameObject FlashbangWeapon;
        [SerializeField] public GameObject HolyHandGrenadeWeapon;

        [SerializeField] public GameObject AssaultRifleWeapon;
        [SerializeField] public GameObject BlasterWeapon;
        [SerializeField] public GameObject PistolWeapon;
        [SerializeField] public GameObject CrossbowWeapon;
        [SerializeField] public GameObject HeatGunWeapon;
        [SerializeField] public GameObject UltraRifleWeapon;
        [SerializeField] public GameObject GsomRaycasterWeapon;
        
        [SerializeField] public GameObject FirePunchWeapon;
        [SerializeField] public GameObject FingerWeapon;
        [SerializeField] public GameObject AxeWeapon;
        [SerializeField] public GameObject GolfClubWeapon;
        [SerializeField] public GameObject FishingRodWeapon;
        [SerializeField] public GameObject GlueGunWeapon;
        [SerializeField] public GameObject FlamethrowerWeapon;
        
        [SerializeField] public GameObject MineWeapon;
        [SerializeField] public GameObject DynamiteWeapon;
        [SerializeField] public GameObject TurretWeapon;
        [SerializeField] public GameObject FrogWeapon;
        [SerializeField] public GameObject MoleWeapon;
        [SerializeField] public GameObject WalkingBombWeapon;
        [SerializeField] public GameObject SuperfrogWeapon;
        
        [FormerlySerializedAs ("RadioThing")]
        [SerializeField] public GameObject Radio;
        
        [SerializeField] public GameObject SpellScroll;
        
        #endregion
        
        [Space]

        #region Projectiles
        [SerializeField] public GameObject BazookaShell;
        [SerializeField] public GameObject HomingMissile;
        [SerializeField] public GameObject MiniRocket;
        [SerializeField] public GameObject CryoBall;
        [SerializeField] public GameObject AngryBird;
        
        [SerializeField] public GameObject Grenade;
        [SerializeField] public GameObject Limonka;
        [SerializeField] public GameObject LimonkaCluster;
        [SerializeField] public GameObject Molotov;
        [SerializeField] public GameObject GhostGrenade;
        [SerializeField] public GameObject GasGrenade;
        [SerializeField] public GameObject Flashbang;
        [SerializeField] public GameObject HolyHandGrenade;

        [SerializeField] public GameObject Bomb;
        [SerializeField] public GameObject NapalmBomb;
        [SerializeField] public GameObject PoisonBomb;
        [SerializeField] public GameObject VacuumBomb;
        
        #endregion

    }

}