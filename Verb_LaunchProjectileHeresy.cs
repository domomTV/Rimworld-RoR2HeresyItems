using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

// Have to make a class separate from Verb_AbilityShoot and Verb_Shoot because pawns will move before shooting for some reason.
// Shooting XP will not be gained from casting
public class Verb_LaunchProjectileHeresy : Verb_LaunchProjectileStatic, IAbilityVerb {
	// Stolen from Verb_Shoot
	protected override int ShotsPerBurst => this.BurstShotCount;
	
	// Stolen from Verb_AbilityShoot
	private Ability ability;

	public Ability Ability
	{
		get => this.ability;
		set => this.ability = value;
	}

	protected override bool TryCastShot()
	{
		bool success = base.TryCastShot();
		if (success && this.ability.def.cooldownTicksRange.min > 0)
			this.ability.StartCooldown(this.ability.def.cooldownTicksRange.RandomInRange);
		return success;
	}

	public override void ExposeData()
	{
		Scribe_References.Look<Ability>(ref this.ability, "ability");
		base.ExposeData();
	}
}

