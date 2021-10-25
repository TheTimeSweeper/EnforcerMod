using UnityEngine;
using RoR2;
using System.Collections.Generic;

class EnforcerLightControllerAlt : MonoBehaviour
{
    public bool sirenToggle;
    public float lightFlashInterval = 0.5f;
    public float maxEmission = 35f;
    public float minEmission = 1f;
    public float emUp = 200f;
    public float emDown = 200f;
    public float flashDuration = 0.25f;

    private float em;
    private Material[] lights;
    private bool lightsOn;
    private float lightStopwatch;
    private int lightFlashes;
    private CharacterBody characterBody;
    private ChildLocator childLocator;
    private CharacterModel characterModel;
    private float flashStopwatch;
    private bool sex;

    void Awake() {
        this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
        this.characterModel = this.gameObject.GetComponentInChildren<CharacterModel>();

        this.Invoke("InitLights", 0.25f);
    }

    //void Start() {
    //    InitLights();
    //}

    private void InitLights()
    {
        this.em = this.minEmission;
        this.sex = true;
        this.sirenToggle = false;
        this.characterBody = this.gameObject.GetComponent<CharacterBody>();

        if (this.characterBody)
        {
            if (this.characterModel)
            {
                //fuck me for trying to future proof this right
                int secondToLast = Mathf.Max(0, 2 - this.characterModel.baseRendererInfos.Length);
                this.lights = new Material[]
                {                                         //secondToLast should be this, no?
                    this.characterModel.baseRendererInfos[7].defaultMaterial
                };
            }

            //if (this.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.stormtrooperIndex && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            //{
            //    this.minEmission = 0f;
            //}
        }

        else
        {
            var mats = new List<Material>();
            var model = base.GetComponent<CharacterModel>();

            int secondToLast  = Mathf.Max(0, 2 - model.baseRendererInfos.Length);
            mats.Add(model.baseRendererInfos[7].defaultMaterial);

            this.lights = mats.ToArray();
        }
    }

    private void FixedUpdate()
    {
        this.HandleShoulderLights();
    }

    private void HandleShoulderLights()
    {
        this.flashStopwatch -= Time.fixedDeltaTime;
        if (this.sirenToggle)
        {
            if (this.flashStopwatch <= 0)
            {
                this.flashStopwatch = this.lightFlashInterval;
                this.FlashLights(1);
            }
        }

        this.lightStopwatch -= Time.fixedDeltaTime;
        if (this.lightStopwatch <= 0.5f * this.flashDuration)
        {
            this.lightsOn = false;

            if (this.lightStopwatch <= 0)
            {
                if (this.lightFlashes > 0)
                {
                    this.lightFlashes--;
                    this.FlashLights(0);
                }
            }
        }

        if (this.lightsOn)
        {
            this.em += this.emUp * Time.fixedDeltaTime;
        }
        else
        {
            this.em -= this.emDown * Time.fixedDeltaTime;
        }

        this.em = Mathf.Clamp(this.em, this.minEmission, this.maxEmission);

        if (this.lights != null)
        {
            if (this.lights.Length > 0)
            {
                foreach (Material i in this.lights)
                {
                    i.SetFloat("_EmPower", this.em);
                }
            }
        }
    }

    private void EnableLights()
    {
        if (!this.sex) this.InitLights();

        this.lightsOn = true;
        //this.em = this.maxEmission;
    }

    public void FlashLights(int flashCount)
    {
        this.lightFlashes += flashCount;
        this.lightStopwatch = this.flashDuration;
        this.EnableLights();
    }

    public void ToggleSiren()
    {
        this.sirenToggle = !this.sirenToggle;

        if (this.sirenToggle)
        {
            string sound = EnforcerPlugin.Sounds.SirenButton;

            if (this.characterBody && EnforcerPlugin.EnforcerModPlugin.cursed.Value)
            {
                //if (this.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.frogIndex) sound = EnforcerPlugin.Sounds.Croak;
            }

            this.flashStopwatch = 0;
        }
    }
}