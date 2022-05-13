using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using EntityStates.Nemforcer;
using static BetterUI.ProcCoefficientCatalog;

namespace EnforcerPlugin {

    public class BetterUICompat {

        public static void init() {

            //enf
            AddSkill("ENFORCER_PRIMARY_SHOTGUN_NAME", "Bullet", RiotShotgun.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_SUPERSHOTGUN_NAME", "Bullet", SuperShotgun2.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_RIFLE_NAME", "Bullet", FireMachineGun.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_HAMMER_NAME", "Bullet", EntityStates.Enforcer.NeutralSpecial.HammerSwing.procCoefficient);

            AddSkill("ENFORCER_SECONDARY_BASH_NAME", "Bash", ShieldBash.procCoefficient);

            AddSkill("ENFORCER_UTILITY_STUNGRENADE_NAME", "Grenade", 1);
            AddSkill("ENFORCER_UTILITY_STUNGRENADESCEPTER_NAME", "Grenade", 1);
            AddSkill("ENFORCER_UTILITY_TEARGASSCEPTER_NAME", "Damaging Gas", 0.05f);

            AddSkill("ENFORCER_SPECIAL_BOARDUP_NAME", "Skateboard", 1000);
            AddSkill("ENFORCER_SPECIAL_BOARDDOWN_NAME", "Skateboard", 1000);

            //nemf
            AddSkill("NEMFORCER_PRIMARY_HAMMER_NAME", "Hammer", 1);
            AddSkill("NEMFORCER_PRIMARY_THROWHAMMER_NAME", "Hammer", 1);

            AddSkill("NEMFORCER_PRIMARY_MINIGUN_NAME", "Bullet", NemMinigunFire.baseProcCoefficient);

            AddSkill("NEMFORCER_SECONDARY_BASH_NAME", "Uppercut", 1);
            AddSkill("NEMFORCER_SECONDARY_SLAM_NAME", "Bash", 1);

            AddSkill("NEMFORCER_UTILITY_GAS_NAME", "Damaging Gas", 0.05f);

            AddSkill("NEMFORCER_UTILITY_CRASH_NAME", "Slam", 1f);

            AddSkill("NEMFORCER_UTILITY_JUMP_NAME", "Jam", 1f);
        }
    }
}