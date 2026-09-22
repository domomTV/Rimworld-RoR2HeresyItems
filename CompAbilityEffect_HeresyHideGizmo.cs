using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

public class CompAbilityEffect_HeresyHideGizmo : CompAbilityEffect {
	public bool hide = false;
	
	public override bool ShouldHideGizmo => hide;

	// Always true to avoid exception when casting ability
	public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest) => true;

	// Removed to avoid invalid cast exception for Verb_JumpStridesOfHeresy
	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest) {
		return;
	}
}