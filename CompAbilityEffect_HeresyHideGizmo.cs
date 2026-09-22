using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

public class CompAbilityEffect_HeresyHideGizmo : CompAbilityEffect {
	public bool hide = false;
	
	public override bool ShouldHideGizmo => hide;

	public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest) => true;

	// Stolen to avoid cast exception for Verb_JumpStridesOfHeresy
	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest) {
		return;
	}
}