using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EntityStates.Enforcer
{
    public class EnforcerMain : GenericCharacterMain
    {
        private ShieldComponent shieldComponent;
        private bool toggle = false;
        private bool wasShielding = false;
        private float initialTime;

        private uint sirenPlayID;

        public static bool shotgunToggle = false;
        private bool sirenToggle;

        public override void OnEnter()
        {
            base.OnEnter();

            this.shieldComponent = base.characterBody.GetComponent<ShieldComponent>();

            //Loadout loadout = base.characterBody.master.loadout;
            //uint specialVar = loadout.bodyLoadoutManager.GetSkillVariant(base.characterBody.bodyIndex, 3);
            //this.shieldComponent.isAlternate = specialVar == 1;
            //bruh just use a new entitystate for this what

            if (!EnforcerPlugin.EnforcerPlugin.cum && base.characterBody.skinIndex == 1)
            {
                EnforcerPlugin.EnforcerPlugin.cum = true;
                Util.PlaySound(EnforcerPlugin.Sounds.DOOM, base.gameObject);
            }
        }

        public override void Update()
        {
            base.Update();
            this.shieldComponent.aimRay = base.GetAimRay();

            //for ror1 shotgun sounds
            if (Input.GetKeyDown(KeyCode.X))
            {
                this.ToggleShotgun();
            }

            //default dance
            if (base.isAuthority && Input.GetKeyDown(KeyCode.Z))
            {
                if (base.characterMotor.isGrounded)
                {
                    this.outer.SetNextState(new DefaultDance());
                    return;
                }
            }

            //sirens
            if (base.isAuthority && Input.GetKeyDown(KeyCode.C))
            {
                this.ToggleSirens();
            }

            // this is a temp toggle, remove this later
            /*if (Input.GetKeyDown(KeyCode.O))
            {
                this.toggle = !this.toggle;
            }

            if (this.toggle)
            {
                getList();
            }*/

            if (shieldComponent.isShielding != this.wasShielding)
            {
                this.wasShielding = shieldComponent.isShielding;
                initialTime = Time.fixedTime;
            }

            if (shieldComponent.isShielding)
            {
                CameraTargetParams ctp = base.cameraTargetParams;
                float denom = (1 + Time.fixedTime - this.initialTime);
                float smoothFactor = 8 / Mathf.Pow(denom, 2);
                Vector3 smoothVector = new Vector3(-3 /20, 1 / 16, -1);
                ctp.idealLocalCameraPos = new Vector3(1.8f, -0.5f, -6f) + smoothFactor * smoothVector;
            }

            if (shieldComponent.shieldHealth <= 0)
            {
                outer.SetNextState(new EnergyShield());
                return;
            }

            //manageTestValues();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.characterBody.HasBuff(EnforcerPlugin.EnforcerPlugin.jackBoots))
            {
                base.characterBody.isSprinting = false;
                base.characterBody.SetAimTimer(0.2f);
            }
        }

        private void manageTestValues() {

            setTestValue(ref ShieldBash.beefDurationShield, 0.05f, KeyCode.O, "bash shielded");
            setTestValue(ref ShieldBash.beefDurationShield, -0.05f, KeyCode.K, "bash shielded");

            setTestValue(ref ShieldBash.beefDurationNoShield, 0.05f, KeyCode.P, "bash unShielded");
            setTestValue(ref ShieldBash.beefDurationNoShield, -0.05f, KeyCode.L, "bash unShielded");

            setTestValue(ref RiotShotgun.beefDurationShield, 0.05f, KeyCode.LeftBracket, "gun shielded");
            setTestValue(ref RiotShotgun.beefDurationShield, -0.05f, KeyCode.Semicolon, "gun shielded");

            setTestValue(ref RiotShotgun.beefDurationNoShield, 0.05f, KeyCode.RightBracket, "gun unShielded");
            setTestValue(ref RiotShotgun.beefDurationNoShield, -0.05f, KeyCode.Quote, "gun unShielded");
        }

        private static void setTestValue(ref float field, float value, KeyCode key, string subject) {
            if (Input.GetKeyDown(key)) {
                field += value;

                Chat.AddMessage($"{subject} set to: {field}");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void getList()
        {
            Collider[] array = Physics.OverlapSphere(base.characterBody.corePosition, 4f, LayerIndex.projectile.mask);
            int num = 0;
            while (num < array.Length)
            {
                ProjectileController pc = array[num].GetComponentInParent<ProjectileController>();
                if (pc)
                {
                    if (pc.teamFilter.teamIndex != TeamIndex.Player)
                    {
                        Ray aimRay = base.GetAimRay();
                        Vector3 aimSpot = (aimRay.origin + 100 * aimRay.direction) - pc.gameObject.transform.position;
                        FireProjectileInfo info = new FireProjectileInfo()
                        {
                            projectilePrefab = pc.gameObject,
                            position = pc.gameObject.transform.position,
                            rotation = base.characterBody.transform.rotation * Quaternion.FromToRotation(new Vector3(0, 0, 1), aimSpot),
                            owner = base.characterBody.gameObject,
                            damage = base.characterBody.damage * 10f,
                            force = 200f,
                            crit = base.RollCrit(),
                            damageColorIndex = DamageColorIndex.Default,
                            target = null,
                            speedOverride = 120f,
                            fuseOverride = -1f
                        };
                        ProjectileManager.instance.FireProjectile(info);

                        Destroy(pc.gameObject);
                    }
                }
                num++;
            }
        }

        private void ToggleShotgun()
        {
            EnforcerMain.shotgunToggle = !EnforcerMain.shotgunToggle;

            if (EnforcerMain.shotgunToggle)
            {
                Chat.AddMessage("Using classic shotgun sounds");
            }
            else
            {
                Chat.AddMessage("Using modern shotgun sounds");
            }
        }

        private void ToggleSirens()
        {
            this.sirenToggle = !this.sirenToggle;

            if (this.sirenToggle)
            {
                this.sirenPlayID = Util.PlaySound(EnforcerPlugin.Sounds.SirenButton, base.gameObject);
            }
            else
            {
                if (this.sirenPlayID != 0) AkSoundEngine.StopPlayingID(this.sirenPlayID);
            }
        }
    }
}