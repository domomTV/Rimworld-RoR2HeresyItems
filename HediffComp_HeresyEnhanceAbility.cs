using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
	
public class HediffComp_HeresyEnhanceAbility : HediffComp {
	public HediffCompProperties_HeresyEnhanceAbility Props => (HediffCompProperties_HeresyEnhanceAbility) this.props;
	
	public override void CompPostPostAdd(DamageInfo? dinfo) {
		base.CompPostPostAdd(dinfo);
		Log.Message("Added");
		List<Ability> abilityList = getAbilityList();
		if (abilityList.Count <= 1)
		{
			return;
		}
		
		bool enhanced = false;
		foreach (Ability ability in abilityList)
		{
			if (ability.def.defName == Props.enhancedDef)
			{
				enhanced = true;
				break;
			}
		}
		
		foreach (Ability ability in abilityList)
		{
			if (!enhanced && ability == this.parent.AllAbilitiesForReading[0])
			{
				ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.enhancedDef);
				enhanced = true;
			}
			else
			{
				setHideAbilityGizmo(ability, true);
			}
		}
	}

	public override void CompPostPostRemoved() {
		base.CompPostPostRemoved();
		Log.Message("Removed");
		List<Ability> abilityList = getAbilityList();
		
		bool enhanced = false;
		foreach (Ability ability in abilityList)
		{
			if (ability.def.defName == Props.enhancedDef)
			{
				enhanced = true;
				break;
			}
		}

		if (!enhanced && abilityList.Count >= 2)
		{
			foreach (Ability ability in abilityList)
			{
				if (!enhanced)
				{
					ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.enhancedDef);
					enhanced = true;
				}
				else
				{
					setHideAbilityGizmo(ability, true);
				}
			}
		}
		else
		{
			foreach (Ability ability in abilityList)
			{
				if (enhanced && ability.def.defName == Props.enhancedDef)
				{
					ability.def = DefDatabase<AbilityDef>.GetNamed(this.Props.baseDef);
					enhanced = false;
				}
				else
				{
					setHideAbilityGizmo(ability, false);
				}
			}
		}
	}

	public override void Notify_Spawned() {
		base.Notify_Spawned();
		List<Ability> abilityList = getAbilityList();
		if (abilityList.Count <= 1)
		{
			return;
		}

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
			if (ability.def.defName == Props.baseDef)
			{
				abilityList.Add(ability);
			}
			else if (ability.def.defName == Props.enhancedDef)
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
	
}