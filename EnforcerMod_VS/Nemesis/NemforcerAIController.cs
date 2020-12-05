using RoR2.CharacterAI;
using UnityEngine;

public class NemforcerAIController : MonoBehaviour
{
    //this is retarded none of it works
    //this component does nothing
    public AISkillDriver[] hammerDrivers;
    public AISkillDriver[] minigunDrivers;

    public void SwapToHammer()
    {
        if (minigunDrivers != null && minigunDrivers.Length > 0)
        {
            for (int i = 0; i < minigunDrivers.Length; i++)
            {
                if (minigunDrivers[i]) minigunDrivers[i].enabled = false;
            }
        }

        if (hammerDrivers != null && hammerDrivers.Length > 0)
        {
            for (int i = 0; i < hammerDrivers.Length; i++)
            {
                if (hammerDrivers[i]) hammerDrivers[i].enabled = true;
            }
        }
    }

    public void SwapToMinigun()
    {
        if (hammerDrivers != null && hammerDrivers.Length > 0)
        {
            for (int i = 0; i < hammerDrivers.Length; i++)
            {
                if (hammerDrivers[i]) hammerDrivers[i].enabled = false;
            }
        }

        if (minigunDrivers != null && minigunDrivers.Length > 0)
        {
            for (int i = 0; i < minigunDrivers.Length; i++)
            {
                if (minigunDrivers[i]) minigunDrivers[i].enabled = true;
            }
        }
    }
}