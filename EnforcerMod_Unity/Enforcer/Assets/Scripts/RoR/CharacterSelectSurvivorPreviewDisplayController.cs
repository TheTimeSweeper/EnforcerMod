using System;
using UnityEngine;
using UnityEngine.Events;
// Token: 0x02000261 RID: 609

namespace RoR2 {
    // Token: 0x020001E8 RID: 488
    public class CharacterSelectSurvivorPreviewDisplayController : MonoBehaviour {

		//// Token: 0x04000A86 RID: 2694
		//public GameObject bodyPrefab;

		// Token: 0x04000A87 RID: 2695
		public CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse[] skillChangeResponses;

		// Token: 0x04000A88 RID: 2696
		public CharacterSelectSurvivorPreviewDisplayController.SkinChangeResponse[] skinChangeResponses;

		// Token: 0x04000A8A RID: 2698
		//private Loadout currentLoadout;

		// Token: 0x020001E9 RID: 489
		[Serializable]
		public struct SkillChangeResponse {
			public string name;
			//// Token: 0x04000A8B RID: 2699
			//public SkillFamily triggerSkillFamily;

			//// Token: 0x04000A8C RID: 2700
			//public SkillDef triggerSkill;

			// Token: 0x04000A8D RID: 2701
			public UnityEvent response;
		}

		// Token: 0x020001EA RID: 490
		[Serializable]
		public struct SkinChangeResponse {
			public string name;
			//// Token: 0x04000A8E RID: 2702
			//public SkinDef triggerSkin;

			// Token: 0x04000A8F RID: 2703
			public UnityEvent response;
		}


	}
}
