using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

public class Projectile_HungeringShard : Projectile {
	// Stolen & changed from Bullet
	protected override void Impact(Thing hitThing, bool blockedByShield = false) {
		Map map = this.Map;
		IntVec3 position = this.Position;
		base.Impact(hitThing, blockedByShield);
		BattleLogEntry_RangedImpact entryRangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
		Find.BattleLog.Add(entryRangedImpact);
		// removed NotifyImpact
		if (hitThing != null)
		{
			bool instigatorGuilty = !(this.launcher is Pawn launcher) || !launcher.Drafted;
			DamageInfo dinfo1 = new DamageInfo(this.DamageDef, this.DamageAmount, this.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, weapon: this.equipmentDef, intendedTarget: this.intendedTarget.Thing, instigatorGuilty: instigatorGuilty);
			dinfo1.SetWeaponQuality(this.equipmentQuality);
			dinfo1.SetWeaponHediff(HediffDef.Named("domom_VisionsOfHeresy"));
			DamageWorker.DamageResult result = hitThing.TakeDamage(dinfo1);
			result.AssociateWithLog(entryRangedImpact);
			// Attach shard to hit thing
			if (result.totalDamageDealt > 0 && !blockedByShield)
				this.TryAttachShard(hitThing, result.LastHitPart, entryRangedImpact);
			if (this.ExtraDamages == null)
				return;
			foreach (ExtraDamage extraDamage in this.ExtraDamages)
			{
				if (Rand.Chance(extraDamage.chance))
				{
					DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), this.ExactRotation.eulerAngles.y, this.launcher, weapon: this.equipmentDef, intendedTarget: this.intendedTarget.Thing, instigatorGuilty: instigatorGuilty);
					hitThing.TakeDamage(dinfo2).AssociateWithLog(entryRangedImpact);
				}
			}
		}
		else
		{
			if (!blockedByShield)
			{
				TryGroundShard(position);
				SoundDefOf.BulletImpact_Ground.PlayOneShot((SoundInfo) new TargetInfo(this.Position, map));
				if (this.Position.GetTerrain(map).takeSplashes)
					FleckMaker.WaterSplash(this.ExactPosition, map, Mathf.Sqrt((float) this.DamageAmount) * 1f, 4f);
				else
					FleckMaker.Static(this.ExactPosition, map, FleckDefOf.ShotHit_Dirt);
			}

			if (!Rand.Chance(this.DamageDef.igniteCellChance))
				return;
			FireUtility.TryStartFireIn(this.Position, map, Rand.Range(0.55f, 0.85f), this.launcher);
		}
	}
	
	// Tries to attach a hungering shard to thing
	public void TryAttachShard(Thing t, BodyPartRecord bp, LogEntry_DamageResult log)
	{
		if (t.Destroyed)
			return;
		
		HungeringShard newThing = (HungeringShard) ThingMaker.MakeThing(ThingDef.Named("domom_HungeringShard"));
		newThing.launcher = this.launcher;
		if (t is Pawn)
		{
			// Will try to damage this when detonating
			newThing.bodyPart = bp;
			// Detonation damage will be linked to this to avoid clutter
			newThing.log = log;
			newThing.AttachTo(t);
		}
		else
			// Sets shard's non-pawn parent
			newThing.parentNoComp = t;
		// Spawn shard in the world
		GenSpawn.Spawn(newThing, t.Position, t.Map, Rot4.North);
	}

	public void TryGroundShard(IntVec3 position) {
		HungeringShard newThing = (HungeringShard) ThingMaker.MakeThing(ThingDef.Named("domom_HungeringShard"));
		newThing.launcher = this.launcher;
		GenSpawn.Spawn(newThing, position, this.intendedTarget.Thing.Map, Rot4.North);
	}
}