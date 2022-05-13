using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterModel : MonoBehaviour {

	public bool autoPopulateLightInfos = true;

	public CharacterModel.RendererInfo[] baseRendererInfos = Array.Empty<CharacterModel.RendererInfo>();

	[Serializable]
	public struct RendererInfo {

		public Renderer renderer;

		public Material defaultMaterial;

		public ShadowCastingMode defaultShadowCastingMode;

		public bool ignoreOverlays;
	}
}

