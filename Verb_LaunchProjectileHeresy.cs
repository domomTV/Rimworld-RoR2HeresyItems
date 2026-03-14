using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

// Have to make a class separate from Verb_AbilityShoot and Verb_Shoot because pawns will move before shooting for some reason.
// Note that XP will not be gained
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

// Fixes wrong rulepack being used for battle log
public class Verb_LaunchHooksOfHeresy : Verb_LaunchProjectileHeresy {
	
	public override void WarmupComplete() {
		// Stolen from Verb
		this.burstShotsLeft = this.ShotsPerBurst;
		this.state = VerbState.Bursting;
		this.TryCastNextBurstShot();
		
		// Stolen & Changed from Verb_LaunchProjectile
		Find.BattleLog.Add((LogEntry) new BattleLogEntry_RangedFire(this.caster, this.currentTarget.HasThing ? this.currentTarget.Thing : (Thing) null, ThingDef.Named("domom_Hooks_VerbScum"), this.Projectile, this.ShotsPerBurst > 1));
	}
	
}