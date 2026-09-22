using UnityEngine;
using Verse;

public class HediffCompProperties_HeresyEnhanceAbility : HediffCompProperties {
	public HediffCompProperties_HeresyEnhanceAbility() => this.compClass = typeof(HediffComp_HeresyEnhanceAbility);

	// Base ability def, hidden when an enhanced ability is present
	public string baseDef;
	
	// Enhanced ability def, replaces one of the base abilities
	public string enhancedDef;
}