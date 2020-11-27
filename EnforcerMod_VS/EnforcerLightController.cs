using UnityEngine;
using RoR2;

class EnforcerLightController : MonoBehaviour
{
    public uint playID;
    public bool sirenToggle;
    public float lightFlashInterval = 0.5f;

    private float flashDuration;
    private Light[] lights;
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
        this.sex = true;
        this.sirenToggle = false;
        this.characterBody = this.gameObject.GetComponent<CharacterBody>();
        this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
        this.flashDuration = 0.5f;

        this.lights = new Light[]
        {
            this.childLocator.FindChild("TempLightL").GetComponentInChildren<Light>(),
            this.childLocator.FindChild("TempLightR").GetComponentInChildren<Light>()
        };

        var charBody = this.GetComponent<CharacterBody>();

        if (charBody)
        {
            Color lightColor = Color.red;

            if (EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                switch (charBody.skinIndex)
                {
                    case 3:
                        lightColor = Color.green;
                        break;
                    case 4:
                        lightColor = Color.white;
                        break;
                    case 6:
                        lightColor = Color.yellow;
                        break;
                    case 7:
                        lightColor = Color.black;
                        break;
                }
            }
            else
            {
                switch (charBody.skinIndex)
                {
                    case 4:
                        lightColor = Color.yellow;
                        break;
                }
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
        }
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
            if (this.lights.Length > 0)
            {
                foreach (Light i in this.lights)
                {
                    if (i) i.enabled = false;
                }
            }

            if (this.lightStopwatch <= 0)
            {
                if (this.lightFlashes > 0)
                {
                    this.lightFlashes--;

                    this.FlashLights(0);
                }
            }
        }
    }

    private void EnableLights()
    {
        if (!this.sex) this.InitLights();

        if (this.lights != null)
        {
            if (this.lights.Length > 0)
            {
                foreach (Light i in this.lights)
                {
                    if (i) i.enabled = true;
                }
            }
        }
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
            if (this.characterBody && EnforcerPlugin.EnforcerPlugin.cursed.Value)
            {
                if (this.characterBody.skinIndex == EnforcerPlugin.EnforcerPlugin.frogIndex ) sound = EnforcerPlugin.Sounds.Croak;
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