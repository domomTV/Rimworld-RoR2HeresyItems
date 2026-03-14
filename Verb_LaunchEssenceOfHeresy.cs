using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

public class Verb_LaunchEssenceOfHeresy : Verb_LaunchProjectileHeresy {
	public Verb_LaunchEssenceOfHeresy() {
		this.canHitNonTargetPawnsNow = false;
	}

	protected override bool TryCastShot() {
		foreach (Pawn pawn in (IEnumerable<Pawn>) this.caster.Map.mapPawns.AllPawnsSpawned)
		{
			if (this.CanTargetPawn(pawn) && !pawn.Fogged())
			{
				// this.AffectGoodwill(pawn.HomeFaction, !flag, pawn);
				this.FireProjectileAt((LocalTargetInfo) pawn);
			}
		}
		// Stolen from Verb_AbilityShoot
		if (this.Ability.def.cooldownTicksRange.min > 0)
			this.Ability.StartCooldown(this.Ability.def.cooldownTicksRange.RandomInRange);
		return true;
	}

	// Stolen from Verb_LaunchProjectile
	protected bool FireProjectileAt(LocalTargetInfo target) {
		if (target.HasThing && target.Thing.Map != this.caster.Map)
			return false;
		ThingDef projectileDef = this.Projectile;
		if (projectileDef == null)
			return false;
		ShootLine resultingLine;
		bool shootLineFromTo = this.TryFindShootLineFromTo(this.caster.Position, target, out resultingLine);
		if (this.verbProps.stopBurstWithoutLos && !shootLineFromTo)
			return false;
		if (this.EquipmentSource != null)
		{
			this.EquipmentSource.GetComp<CompChangeableProjectile>()?.Notify_ProjectileLaunched();
			this.EquipmentSource.GetComp<CompApparelVerbOwner_Charged>()?.UsedOnce();
		}

		this.lastShotTick = Find.TickManager.TicksGame;
		Thing launcher = this.caster;
		Thing thing = (Thing) this.EquipmentSource;
		// Removed mannable building check
		Vector3 drawPos = this.caster.DrawPos;
		Verse.Projectile projectile = (Verse.Projectile) GenSpawn.Spawn(projectileDef, resultingLine.Source, this.caster.Map);
		// Removed unique weapon check
		// Removed forced miss check
		// Removed can go wild check
		// Removed cover check
		ProjectileHitFlags hitFlags1 = ProjectileHitFlags.IntendedTarget;
		// Removed hit flag checks
		if (target.Thing != null)
		{
			projectile.Launch(launcher, drawPos, target, target, hitFlags1, this.preventFriendlyFire, thing, null);
		}
		else
		{
			projectile.Launch(launcher, drawPos, (LocalTargetInfo) resultingLine.Dest, target, hitFlags1, this.preventFriendlyFire, thing, null);
		}

		return true;
	}

	private bool CanTargetPawn(Pawn p) {
		return !p.Dead && !p.Suspended && p.health.hediffSet.HasHediff(HediffDef.Named("domom_Ruin"));
	}
}