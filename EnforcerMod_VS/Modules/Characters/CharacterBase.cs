using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Characters {
    public abstract class CharacterBase {

        public static CharacterBase instance;

        public abstract string characterName { get; }

        public abstract BodyInfo bodyInfo { get; }

        public virtual CustomRendererInfo[] customRendererInfos { get; set; }

        public abstract Type characterMainState { get; }
        public virtual Type characterSpawnState { get; }

        public abstract ItemDisplaysBase itemDisplays { get; }

        public virtual GameObject bodyPrefab { get; set; }
        public virtual CharacterModel characterBodyModel { get; set; }
        public string fullBodyName => characterName + "Body";

        public virtual void Initialize() {
            instance = this;
        }

        protected virtual void InitializeCharacterBodyAndModel() {
            bodyPrefab = Modules.Prefabs.CreateBodyPrefab(characterName + "Body", "mdl" + characterName, bodyInfo);
            InitializeCharacterModel();
        }
        protected virtual void InitializeCharacterModel() {
            characterBodyModel = Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos);
        }

        protected virtual void InitializeCharacterMaster() { }
        protected virtual void InitializeEntityStateMachine() {
            bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(characterMainState);
            Modules.Content.AddEntityState(characterMainState);
            if (characterSpawnState != null) {
                bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new EntityStates.SerializableEntityStateType(characterSpawnState);
                Modules.Content.AddEntityState(characterSpawnState);
            }
        }

        public abstract void InitializeSkills();

        public virtual void InitializeHitboxes() { }

        public virtual void InitializeHurtboxes(HealthComponent healthComponent) {
            Modules.Prefabs.SetupHurtBoxes(healthComponent);
        }

        public virtual void InitializeSkins() { }

        public virtual void InitializeDoppelganger() {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, characterName + "MonsterMaster", "Merc");
        }

        public virtual void InitializeItemDisplays() {

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + characterName;

            characterBodyModel.itemDisplayRuleSet = itemDisplayRuleSet;
        }

        public void SetItemDisplays() {
            if (itemDisplays != null) {
                itemDisplays.SetItemDIsplays(characterBodyModel.itemDisplayRuleSet);
            }
        }

    }

    // for simplifying characterbody creation
    public class BodyInfo {
        public string bodyName = "";
        public string bodyNameToken = "";
        public string subtitleNameToken = "";
        /// <summary>
        /// body prefab you're cloning for your character- commando is the safest
        /// </summary>
        public string bodyNameToClone = "Commando";

        public Color bodyColor = Color.grey;

        public Texture characterPortrait = null;
        public float sortPosition = 69f;

        public GameObject crosshair = null;
        public GameObject podPrefab = null;

        //stats
        public float maxHealth = 100f;
        public float healthRegen = 1f;
        public float armor = 0f;
        /// <summary>
        /// base shield is a thing apparently. neat
        /// </summary>
        public float shield = 0f;

        public float damage = 12f;
        public float attackSpeed = 1f;
        public float crit = 1f;

        public float moveSpeed = 7f;
        public float jumpPower = 15f;

        //growth
        public bool autoCalculateLevelStats = true;

        public float healthGrowth = 30f;
        public float regenGrowth = 0.2f;
        public float shieldGrowth = 0f;
        public float armorGrowth = 0f;

        public float damageGrowth = 2.4f;
        public float attackSpeedGrowth = 0f;
        public float critGrowth = 0f;

        public float moveSpeedGrowth = 0f;
        public float jumpPowerGrowth = 0f;// jump power per level exists for some reason

        //other
        public float acceleration = 80f;
        public int jumpCount = 1;

        //camera
        public Vector3 modelBasePosition = new Vector3(0f, -0.92f, 0f);
        public Vector3 cameraPivotPosition = new Vector3(0f, 1.6f, 0f);
        public Vector3 aimOriginPosition = new Vector3(0f, 2f, 0f);

        public float cameraParamsVerticalOffset = 1.2f;
        public float cameraParamsDepth = -12;

        private CharacterCameraParams _cameraParams;
        public CharacterCameraParams cameraParams {
            get {
                if (_cameraParams == null) {
                    _cameraParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
                    _cameraParams.data.minPitch = -70;
                    _cameraParams.data.maxPitch = 70;
                    _cameraParams.data.wallCushion = 0.1f;
                    _cameraParams.data.pivotVerticalOffset = cameraParamsVerticalOffset;
                    _cameraParams.data.idealLocalCameraPos= new Vector3(0, 0, cameraParamsDepth);
                }
                return _cameraParams;
            }
            set => _cameraParams = value;
        }

        public float SurvivorHeightToSet {
            //todo dynamically set good camera params based on a height that you want
            set {
                float height = value;
            }
        }
    }

    // for simplifying rendererinfo creation
    public class CustomRendererInfo {
        public string childName;
        public Material material = null;
        public bool ignoreOverlays = false;
        public bool dontHotpoo = false;
    }
}
