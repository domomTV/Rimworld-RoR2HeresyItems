using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

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