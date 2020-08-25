using UnityEngine;
using RoR2;

class EnforcerLightController : MonoBehaviour
{
    private float flashDuration;
    private Light[] lights;
    private float lightStopwatch;
    private int lightFlashes;
    private ChildLocator childLocator;
    private bool sex;

    private void Start()
    {
        if (!this.sex) this.InitLights();
    }

    private void InitLights()
    {
        this.sex = true;
        this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
        this.flashDuration = 0.5f;

        this.lights = new Light[]
        {
            this.childLocator.FindChild("LightL").GetComponentInChildren<Light>(),
            this.childLocator.FindChild("LightR").GetComponentInChildren<Light>()
        };

        var charBody = this.GetComponent<CharacterBody>();

        if (charBody)
        {
            Color lightColor = Color.red;

            switch (charBody.skinIndex)
            {
                case 0:
                    lightColor = Color.red;
                    break;
                case 1:
                    lightColor = Color.black;
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
            }

            foreach (Light i in this.lights)
            {
                if (i)
                {
                    i.color = lightColor;
                    i.intensity *= 1.5f;
                    i.range *= 1.5f;
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
}
