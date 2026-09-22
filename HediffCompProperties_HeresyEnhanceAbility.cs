using UnityEngine;
using Verse;

public class HediffCompProperties_HeresyEnhanceAbility : HediffCompProperties {
	public HediffCompProperties_HeresyEnhanceAbility() => this.compClass = typeof(HediffComp_HeresyEnhanceAbility);

	public string baseDef;
	public string enhancedDef;
}