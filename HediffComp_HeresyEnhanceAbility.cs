using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
	
public class HediffComp_HeresyEnhanceAbility : HediffComp {
	public HediffCompProperties_HeresyEnhanceAbility Props => (HediffCompProperties_HeresyEnhanceAbility) this.props;
	
	// Called after body part is added
	public override void CompPostPostAdd(DamageInfo? dinfo) {
		base.CompPostPostAdd(dinfo);
		List<Ability> abilityList = getAbilityList();
		// If there's only one, do not modify
		if (abilityList.Count <= 1)
			return;

		bool enhanced = checkForEnhanced(abilityList);
		foreach (Ability ability in abilityList)
		{
			// If no ability is enhanced and current ability is mine, enhance it
			if (!enhanced && ability == this.parent.AllAbilitiesForReading[0])
			{
				ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.enhancedDef);
				ability.VerbTracker.InitVerbsFromZero();
				ability.Initialize();
				enhanced = true;
			}
			else
			{
				setHideAbilityGizmo(ability, true);
			}
		}
	}

	// Called after body part is removed
	public override void CompPostPostRemoved() {
		base.CompPostPostRemoved();
		List<Ability> abilityList = getAbilityList();
		bool enhanced = this.checkForEnhanced(abilityList);
		// If no ability is enhanced and >= 2 copies remain, ensure one is enhanced and the rest are hidden
		if (!enhanced && abilityList.Count >= 2)
		{
			foreach (Ability ability in abilityList)
			{
				if (!enhanced)
				{
					ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.enhancedDef);
					ability.VerbTracker.InitVerbsFromZero();
					ability.Initialize();
					enhanced = true;
				}
				else
				{
					setHideAbilityGizmo(ability, true);
				}
			}
		}
		// Else revert enhanced ability to base or unhide hidden ability
		else
		{
			foreach (Ability ability in abilityList)
			{
				if (enhanced && ability.def.defName == Props.enhancedDef)
				{
					ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.baseDef);
					ability.VerbTracker.InitVerbsFromZero();
					ability.Initialize();
					enhanced = false;
				}
				else
				{
					setHideAbilityGizmo(ability, false);
				}
			}
		}
	}

	// Called when a save is loaded
	public override void Notify_Spawned() {
		base.Notify_Spawned();
		List<Ability> abilityList = getAbilityList();
		// If there's only one, we don't hide it
		if (abilityList.Count <= 1)
			return;

		// Hide all base abilities
		foreach (Ability ability in abilityList)
		{
			if (ability.def.defName == this.Props.baseDef)
			{
				setHideAbilityGizmo(ability, true);
			}
		}
	}

	private List<Ability> getAbilityList() {
		List<Ability> abilityList = new List<Ability>();
		
		foreach (Ability ability in this.parent.pawn.abilities.AllAbilitiesForReading)
		{
			if (ability.def.defName == Props.baseDef || ability.def.defName == Props.enhancedDef)
			{
				abilityList.Add(ability);
			}
		}

		return abilityList;
	}

	private static void setHideAbilityGizmo(Ability ability, bool val) {
		foreach (CompAbilityEffect effectComp in ability.EffectComps)
		{
			if (effectComp is CompAbilityEffect_HeresyHideGizmo compAbilityEffectHeresy)
			{
				compAbilityEffectHeresy.hide = val;
				break;
			}
		}
	}

	// Check if pawn already has an enhanced ability
	private bool checkForEnhanced(List<Ability> abilityList) {
		foreach (Ability ability in abilityList)
		{
			if (ability.def.defName == Props.enhancedDef)
			{
				return true;
			}
		}

		return false;
	}
	
}