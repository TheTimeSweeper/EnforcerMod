using EntityStates.Enforcer;
using EntityStates.Toolbot;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemforcer
{
    public class AimNemGas : AimThrowableBase
    {
        private AimStunDrone goodState;

        public override void OnEnter()
        {
            if (goodState == null) goodState = Instantiate(typeof(AimStunDrone)) as AimStunDrone;

            maxDistance = 64;
            rayRadius = 2f;
            arcVisualizerPrefab = goodState.arcVisualizerPrefab;
            projectilePrefab = EnforcerPlugin.NemforcerPlugin.nemGasGrenade;
            endpointVisualizerPrefab = goodState.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = 4f;
            setFuse = false;
            damageCoefficient = 0.5f;
            baseMinimumDuration = 0.1f;
            projectileBaseSpeed = 150;
            
            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff))
            {
                base.PlayAnimation("Gesture, Override", "HoldGrenadeMinigun", "ThrowGrenade.playbackRate", 0.5f);
            }
            else
            {
                base.PlayAnimation("Gesture, Override", "HoldGrenadeHammer", "ThrowGrenade.playbackRate", 0.5f);
            }

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.characterBody.SetAimTimer(0.25f);
            this.fixedAge += Time.fixedDeltaTime;

            bool flag = false;

            if (base.isAuthority && !this.KeyIsDown() && base.fixedAge >= this.minimumDuration) flag = true;
            if (base.characterBody && base.characterBody.isSprinting) flag = true;

            if (flag)
            {
                this.UpdateTrajectoryInfo(out this.currentTrajectoryInfo);

                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (base.HasBuff(EnforcerPlugin.EnforcerPlugin.minigunBuff))
            {
                base.PlayAnimation("Gesture, Override", "ThrowGrenadeMinigun", "ThrowGrenade.playbackRate", 0.5f);
            }
            else
            {
                base.PlayAnimation("Gesture, Override", "ThrowGrenadeHammer", "ThrowGrenade.playbackRate", 0.5f);
            }

            Util.PlaySound(EnforcerPlugin.Sounds.NemesisGrenadeThrow, base.gameObject);

            base.AddRecoil(-2f * TearGas.bulletRecoil, -3f * TearGas.bulletRecoil, -1f * TearGas.bulletRecoil, 1f * TearGas.bulletRecoil);
            base.characterBody.AddSpreadBloom(0.33f * TearGas.bulletRecoil);
            EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, "HandR", false);
        }
    }

}