using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

public class Verb_LaunchEssenceOfHeresy : Verb_LaunchProjectileHeresy {
	public Verb_LaunchEssenceOfHeresy() {
		// Sets projectile to pass through other pawns
		this.canHitNonTargetPawnsNow = false;
	}

	protected override bool TryCastShot() {
		// Searching through all pawns on caster's map
		foreach (Pawn pawn in this.caster.Map.mapPawns.AllPawnsSpawned)
		{
			// If pawn is targetable & not in fog, try to fire at them
			if (this.CanTargetPawn(pawn) && !pawn.Fogged())
				this.FireProjectileAt((LocalTargetInfo) pawn);
		}
		// Stolen from Verb_AbilityShoot
		// Sets ability cooldown if needed
		if (this.Ability.def.cooldownTicksRange.min > 0)
			this.Ability.StartCooldown(this.Ability.def.cooldownTicksRange.RandomInRange);
		return true;
	}

	// Stolen from Verb_LaunchProjectile
	protected bool FireProjectileAt(LocalTargetInfo target) {
		// Does nothing if target is invalid, or target is on another map
		if (target.HasThing && target.Thing.Map != this.caster.Map)
			return false;
		// Gets projectile def
		ThingDef projectileDef = this.Projectile;
		// Does nothing if projectile isn't defined
		if (projectileDef == null)
			return false;
		// Stores projectile's travel line
		ShootLine resultingLine;
		// Gets travel line, if line of sight is needed & doesn't have, returns 
		bool shootLineFromTo = this.TryFindShootLineFromTo(this.caster.Position, target, out resultingLine);
		if (this.verbProps.stopBurstWithoutLos && !shootLineFromTo)
			return false;
		// Calls equipment comps
		if (this.EquipmentSource != null)
		{
			this.EquipmentSource.GetComp<CompChangeableProjectile>()?.Notify_ProjectileLaunched();
			this.EquipmentSource.GetComp<CompApparelVerbOwner_Charged>()?.UsedOnce();
		}

		// Don't really know whats happening below
		
		this.lastShotTick = Find.TickManager.TicksGame;
		Thing launcher = this.caster;
		Thing thing = (Thing) this.EquipmentSource;
			// Removed mannable building check
		Vector3 drawPos = this.caster.DrawPos;
		Projectile projectile = (Projectile) GenSpawn.Spawn(projectileDef, resultingLine.Source, this.caster.Map);
			// Removed unique weapon check
			// Removed forced miss check
			// Removed can go wild check
			// Removed cover check
		ProjectileHitFlags hitFlags1 = ProjectileHitFlags.IntendedTarget;
			// Removed hit flag checks
		if (target.Thing != null)
			projectile.Launch(launcher, drawPos, target, target, hitFlags1, this.preventFriendlyFire, thing, null);
		else
			projectile.Launch(launcher, drawPos, (LocalTargetInfo) resultingLine.Dest, target, hitFlags1, this.preventFriendlyFire, thing, null);

		return true;
	}

	private bool CanTargetPawn(Pawn p) {
		// Returns true if pawn isn't dead or suspended, and has the Ruin hediff
		return !p.Dead && !p.Suspended && p.health.hediffSet.HasHediff(HediffDef.Named("domom_Ruin"));
	}
}