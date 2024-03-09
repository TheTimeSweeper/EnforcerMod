using EntityStates.Enforcer;
using EntityStates.Enforcer.NeutralSpecial;
using EntityStates.Nemforcer;
using static BetterUI.ProcCoefficientCatalog;

namespace EnforcerPlugin {

    public class BetterUICompat {

        public static void init() {

            //enf
            AddSkill("ENFORCER_PRIMARY_SHOTGUN_NAME", "ENFORCER_PROC_BULLET", RiotShotgun.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_SUPERSHOTGUN_NAME", "ENFORCER_PROC_BULLET", SuperShotgun2.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_RIFLE_NAME", "ENFORCER_PROC_BULLET", FireMachineGun.procCoefficient);
            AddSkill("ENFORCER_PRIMARY_HAMMER_NAME", "ENFORCER_PROC_HAMMER", EntityStates.Enforcer.NeutralSpecial.HammerSwing.procCoefficient);

            AddSkill("ENFORCER_SECONDARY_BASH_NAME", "ENFORCER_PROC_BASH", ShieldBash.procCoefficient);

            AddSkill("ENFORCER_UTILITY_STUNGRENADE_NAME", "ENFORCER_PROC_GRENADE", 1);
            AddSkill("ENFORCER_UTILITY_STUNGRENADESCEPTER_NAME", "ENFORCER_PROC_GRENADE", 1);
            AddSkill("ENFORCER_UTILITY_TEARGASSCEPTER_NAME", "ENFORCER_PROC_DAMAGING_GAS", 0.05f);

            AddSkill("ENFORCER_SPECIAL_BOARDUP_NAME", "ENFORCER_PROC_SKATEBOARD", 1000);
            AddSkill("ENFORCER_SPECIAL_BOARDDOWN_NAME", "ENFORCER_PROC_SKATEBOARD", 1000);

            //nemf
            AddSkill("NEMFORCER_PRIMARY_HAMMER_NAME", "ENFORCER_PROC_HAMMER", 1);
            AddSkill("NEMFORCER_PRIMARY_THROWHAMMER_NAME", "ENFORCER_PROC_HAMMER", 1);

            AddSkill("NEMFORCER_PRIMARY_MINIGUN_NAME", "ENFORCER_PROC_BULLET", NemMinigunFire.baseProcCoefficient);

            AddSkill("NEMFORCER_SECONDARY_BASH_NAME", "NEMFORCER_PROC_UPPERCUT", 1);
            AddSkill("NEMFORCER_SECONDARY_SLAM_NAME", "ENFORCER_PROC_BASH", 1);

            AddSkill("NEMFORCER_UTILITY_GAS_NAME", "ENFORCER_PROC_DAMAGING_GAS", 0.05f);

            AddSkill("NEMFORCER_UTILITY_CRASH_NAME", "NEMFORCER_PROC_SLAM", 1f);

            AddSkill("NEMFORCER_UTILITY_JUMP_NAME", "NEMFORCER_PROC_JAM", 1f);
        }
    }
}