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
		// this.NotifyImpact(hitThing, map, position); <----- Caused some BS cause projectile is explosive I think?
		if (hitThing != null)
		{
			bool instigatorGuilty = !(this.launcher is Pawn launcher) || !launcher.Drafted;
			DamageInfo dinfo1 = new DamageInfo(this.DamageDef, this.DamageAmount, this.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, weapon: this.equipmentDef, intendedTarget: this.intendedTarget.Thing, instigatorGuilty: instigatorGuilty);
			dinfo1.SetWeaponQuality(this.equipmentQuality);
			DamageWorker.DamageResult result = hitThing.TakeDamage(dinfo1);
			result.AssociateWithLog(entryRangedImpact);
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
	
	// Tries to attach a hungering shard to hit thing
	public void TryAttachShard(Thing t, BodyPartRecord bp, LogEntry_DamageResult log)
	{
		// Does nothing if thing is destroyed
		if (t.Destroyed)
			return;
		// Initializes new hungering shard
		HungeringShard newThing = (HungeringShard) ThingMaker.MakeThing(ThingDef.Named("domom_HungeringShard"));
		// Sets shard's launcher
		newThing.launcher = this.launcher;
		// If thing is a pawn:
		if (t is Pawn)
		{
			// Set's shard's target body part
			newThing.bodyPart = bp;
			// Set's shard's log
			newThing.log = log;
			// Attaches to pawn's attachment comp
			newThing.AttachTo(t);
		}
		// If thing is not a pawn:
		else
			// Set's shard's compless parent
			newThing.parentNoComp = t;
		// Spawn shard in the world
		GenSpawn.Spawn(newThing, t.Position, t.Map, Rot4.North);
	}
}