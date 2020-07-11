using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace RoR2.Projectile
{
    //DO NOT TOUCH
    //IF YOU TOUCH I WILL KILL
    //THE POWER OF UNDERSTANDING AND BEING ABLE TO WRITE MONOBEHAVIORS
    //I AM THE UNITY GOD GOD 
    [RequireComponent(typeof(HitBoxGroup))]
    [RequireComponent(typeof(ProjectileController))]
    public class EProjectileDotZone : MonoBehaviour, IProjectileImpactBehavior
    {
        private ProjectileController projController;
        private ProjectileDamage projDamage;
        private OverlapAttack attack;

        //gameObjects
        public GameObject iEffect;

        //VECTO-3 MACHINE 0.3V
        public Vector3 forceVector;

        //public floats
        public float damageCoefficient;
        public float overlapProcCoefficient = 1f;
        public float fireFrequency = 1f;
        public float resetFrequency = 20f;
        public float lifetime = 30f;

        //events
        public UnityEvent onBegin;
        public UnityEvent onEnd;

        //stupid stopwatch stuff argh
        private float fStopwatch;
        private float rStopwatch;
        private float tStopwatch;
        private void Start() {
            this.projController = base.GetComponent<ProjectileController>();
            this.projDamage = base.GetComponent<ProjectileDamage>();
            this.ResetOverlap();
            this.onBegin.Invoke();
        }
        private void ResetOverlap()  {
            //I MADE THE OVERLAPATTACK RETARDED
            //CAN'T COPYRIGHT ME NOW HOPOO
            //CAN'T TAKE SUCKER SHOTS AT MY BANANA CAGE NOW
            //SUCK MY DIIIIIIIIIIIIIICK
            this.attack = new OverlapAttack() {
                attacker = this.projController.owner,
                damage = this.projDamage.damage,
                damageColorIndex = this.projDamage.damageColorIndex,
                damageType = this.projDamage.damageType,
                forceVector = this.forceVector + this.projDamage.force * base.transform.forward,
                hitBoxGroup = base.GetComponent<HitBoxGroup>(),
                hitEffectPrefab = this.iEffect,
                inflictor = base.gameObject,
                isCrit = this.projDamage.crit,
                procChainMask = this.projController.procChainMask,
                procCoefficient = overlapProcCoefficient,
                pushAwayForce = this.projDamage.force,
                teamIndex = this.projController.teamFilter.teamIndex
            };
        }

        public void OnProjectileImpact(ProjectileImpactInfo impactInfo) {
            //I PUT IN WORK AS SNACKALACK
            //PUT MY SOUL ON THE MOD LIKE LUNCHBOX
            //MADE A BETA TESTER TEST MY MOD
            //NOW HE CAN'T EVEN INSTALL MODS RIGHT   
        }
        public void FixedUpdate() {
            if (NetworkServer.active) {
                this.tStopwatch += Time.fixedDeltaTime;
                this.rStopwatch += Time.fixedDeltaTime;
                this.fStopwatch += Time.fixedDeltaTime;
                if (this.rStopwatch >= 1f / this.resetFrequency) {
                    this.ResetOverlap();
                    this.rStopwatch -= 1f / this.resetFrequency;
                }
                if (this.fStopwatch >= 1f / this.fireFrequency) {
                    this.attack.Fire(null);
                    this.fStopwatch -= 1f / this.fireFrequency;
                }
                if (this.tStopwatch >= this.lifetime) {
                    this.onEnd.Invoke();
                    Destroy(base.gameObject);
                }
            }
        }

    }
}
