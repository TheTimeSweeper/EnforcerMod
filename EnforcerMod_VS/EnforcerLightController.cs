using UnityEngine;
using RoR2;

class EnforcerLightController : MonoBehaviour
{
    public uint playID;
    public bool sirenToggle;
    public float lightFlashInterval = 0.5f;
    public float maxEmission = 15f;
    public float minEmission = 1f;
    public float emUp = 65f;
    public float emDown = 25f;
    public float flashDuration = 0.5f;

    private float em;
    private Material[] lights;
    private bool lightsOn;
    private float lightStopwatch;
    private int lightFlashes;
    private CharacterBody characterBody;
    private ChildLocator childLocator;
    private float flashStopwatch;
    private bool sex;

    private void Start()
    {
        if (!this.sex) this.InitLights();
    }

    private void InitLights()
    {
        this.em = this.minEmission;
        this.sex = true;
        this.sirenToggle = false;
        this.characterBody = this.gameObject.GetComponent<CharacterBody>();
        this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();

        if (this.characterBody)
        {
            Transform modelTransform = this.characterBody.GetComponent<ModelLocator>().modelTransform;
            if (modelTransform)
            {
                this.lights = new Material[]
                {
                    modelTransform.GetComponent<CharacterModel>().baseRendererInfos[33].defaultMaterial,
                    modelTransform.GetComponent<CharacterModel>().baseRendererInfos[34].defaultMaterial
                };
            }
        }

        //no longer using shitty lights
        /*var charBody = this.GetComponent<CharacterBody>();

        if (charBody)
        {
            Color lightColor = Color.red;

            switch (charBody.skinIndex)
            {
                case 0:
                    lightColor = Color.red;
                    break;
                case 1:
                    lightColor = Color.red;
                    break;
                case 2:
                    lightColor = Color.red;
                    break;
                case 3:
                    lightColor = Color.green;
                    break;
                case 4:
                    lightColor = Color.white;
                    break;
                case 5:
                    lightColor = Color.red;
                    break;
                case 6:
                    lightColor = Color.black;
                    break;
            }

            foreach (Light i in this.lights)
            {
                if (i)
                {
                    i.color = lightColor;
                    i.intensity *= 2.5f;
                    i.range *= 1.25f;
                }
            }
        }*/
    }

    private void FixedUpdate()
    {
        this.HandleShoulderLights();
    }

    private void HandleShoulderLights()
    {
        if (this.sirenToggle)
        {
            this.flashStopwatch -= Time.fixedDeltaTime;
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
                    //this.FlashLights(0);
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
                if (Input.GetKey("g")) lights[0].SetFloat("_EmPower", 1000);
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

            if (this.characterBody)
            {
                if (this.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.frogIndex) sound = EnforcerPlugin.Sounds.Croak;
            }

            this.playID = Util.PlaySound(sound, base.gameObject);
            this.flashStopwatch = 0;
        }
        else
        {
            if (this.playID != 0) AkSoundEngine.StopPlayingID(this.playID);
        }
    }

    private void OnDestroy()
    {
        if (this.playID != 0) AkSoundEngine.StopPlayingID(this.playID);
    }
}
