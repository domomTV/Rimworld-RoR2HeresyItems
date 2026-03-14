
using System.Collections.Generic;
using RimWorld;
using Verse;

public class CompMaelstrom : ThingComp {
	public CompProperties_Maelstrom Props => (CompProperties_Maelstrom) this.props;

	public int minorDetsLeft;
	public float ticksToNextDetonation = 1;

	public override void Initialize(CompProperties p) {
		base.Initialize(p);
		if (!Props.minorDetonationOnSpawn)
			this.ticksToNextDetonation = Props.ticksBetweenDetonations;
		this.minorDetsLeft = Props.minorDetonationsNumber;
	}

	public override void CompTickInterval(int delta) 
	{
		if (this.ticksToNextDetonation <= 0)
			return;
		this.ticksToNextDetonation -= delta;
		if (this.ticksToNextDetonation > 0)
			return;
		
		if (this.minorDetsLeft > 0)
		{
			--this.minorDetsLeft;
			this.ticksToNextDetonation += Props.ticksBetweenDetonations;
			this.Maelstrom_Detonate(false);
		}
		else
		{
			this.Maelstrom_Detonate(true);
			this.parent.Destroy(DestroyMode.Vanish);
		}
	}

	// Stolen and edited from Projectile_Explosive
	protected void Maelstrom_Detonate(bool isFinal) {
		Map map1 = this.parent.Map;
		if (this.Props.explosionEffect != null)
		{
			Effecter eff = this.Props.explosionEffect.Spawn();
			if (this.Props.explosionEffect.maintainTicks != 0)
			{
				map1.effecterMaintainer.AddEffecterToMaintain(eff, this.parent.Position.ToVector3().ToIntVec3(),
					this.Props.explosionEffect.maintainTicks);
			}
			else
			{
				eff.Trigger(new TargetInfo(this.parent.Position, map1), new TargetInfo(this.parent.Position, map1));
				eff.Cleanup();
			}
		}
		
		Thing instigator = this.parent;
		ThingDef projectileDef = null;
		if (this.parent is MaelstromOrb orb)
		{
			instigator = orb.instigator;
			projectileDef = orb.projectileDef;
		}

		IntVec3 position = this.parent.Position;
		double explosionRadius = isFinal ? (double) this.Props.explosiveRadius : this.Props.explosiveRadiusMinor;
		DamageDef damageDef = isFinal ?  this.Props.explosiveDamageType : this.Props.explosiveDamageTypeMinor;
		int damageAmount = this.Props.damageAmountBase;
		double armorPenetration = (double) this.Props.armorPenetrationBase;
		SoundDef soundExplode = this.Props.explosionSound;
		ThingDef postExplosionSpawnThingDef = this.Props.postExplosionSpawnThingDef;
		double explosionSpawnChance1 = (double) this.Props.postExplosionSpawnChance;
		int explosionSpawnThingCount1 = this.Props.postExplosionSpawnThingCount;
		GasType? explosionGasType = this.Props.postExplosionGasType;
		bool explosionCellsNeighbors = this.Props.applyDamageToExplosionCellsNeighbors;
		ThingDef preExplosionSpawnThingDef = this.Props.preExplosionSpawnThingDef;
		float preExplosionSpawnChance = this.Props.preExplosionSpawnChance;
		int preExplosionSpawnThingCount = this.Props.preExplosionSpawnThingCount;
		float chanceToStartFire2 = this.Props.chanceToStartFire;
		bool explosionDamageFalloff = this.Props.damageFalloff;
		bool doExplosionVfx = this.Props.doVisualEffects;
		float propagationSpeed1 = damageDef.expolosionPropagationSpeed;
		float screenShakeFactor = 0;
		ThingDef postExplosionSpawnSingleThingDef = this.Props.preExplosionSpawnSingleThingDef;
		ThingDef preExplosionSpawnSingleThingDef = this.Props.postExplosionSpawnSingleThingDef;
		GenExplosion.DoExplosion(
			position, 
			map1, 
			(float) explosionRadius, 
			damageDef, 
			instigator, 
			damageAmount, 
			(float) armorPenetration, 
			soundExplode, 
			null, 
			projectileDef, 
			null, 
			postExplosionSpawnThingDef, 
			(float) explosionSpawnChance1, 
			explosionSpawnThingCount1, 
			explosionGasType, 
			null, 
			applyDamageToExplosionCellsNeighbors: explosionCellsNeighbors, 
			preExplosionSpawnThingDef: preExplosionSpawnThingDef, 
			preExplosionSpawnChance: (float) preExplosionSpawnChance, 
			preExplosionSpawnThingCount: preExplosionSpawnThingCount, 
			chanceToStartFire: (float) chanceToStartFire2, 
			damageFalloff: explosionDamageFalloff, 
			direction: null, 
			affectedAngle: null, 
			doVisualEffects: doExplosionVfx, 
			propagationSpeed: (float) propagationSpeed1, 
			postExplosionSpawnThingDefWater: null, 
			screenShakeFactor: screenShakeFactor, 
			postExplosionSpawnSingleThingDef: postExplosionSpawnSingleThingDef, 
			preExplosionSpawnSingleThingDef: preExplosionSpawnSingleThingDef);
	}
}