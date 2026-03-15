using Verse;

public class CompHungeringShard : ThingComp {
	public CompProperties_HungeringShard Props => (CompProperties_HungeringShard) this.props;

	// Game ticks until shard detonates
	public int ticksUntilDetonation;

	public override void Initialize(CompProperties p) {
		base.Initialize(p);
		// Set ticks until detonation from property defined in comp properties
		this.ticksUntilDetonation = Props.ticksUntilDetonation;
	}
	
	// If ticksUntilDetonation <= 0, detonate
	public override void CompTickInterval(int delta) 
	{
		if (this.ticksUntilDetonation <= 0)
			return;
		this.ticksUntilDetonation -= delta;
		if (this.ticksUntilDetonation <= 0)
			this.Detonate();
	}

	public void Detonate() {
		// Does nothing if parent isn't HunderingShard or shard is already destroyed
		if (!(this.parent is HungeringShard shard) || shard.Destroyed)
			return;
		// Gets damage dealt from comp properties, or damage def as fallback
		int damage = this.Props.damageAmountBase != -1 ? this.Props.damageAmountBase : this.Props.damageType.defaultDamage;
		// Construct damage info from comp properties & projectile info
		DamageInfo damageInfo = new DamageInfo(this.Props.damageType, damage, this.Props.damageType.defaultArmorPenetration, instigator: shard.launcher, intendedTarget: shard.parent);
		// If parent isn't a pawn:
		if (shard.parentNoComp != null)
			// Simply damage the thing
			shard.parentNoComp.TakeDamage(damageInfo);
		// If parent is a pawn:
		else if (shard.parent != null)
		{
			// Targets the same part the projectile hit
			damageInfo.SetHitPart(shard.bodyPart);
			// Damage the thing & link it to the projectile's log
			shard.parent.TakeDamage(damageInfo).AssociateWithLog(shard.log);
		}
		
		// Ensure this shard is destroyed
		if (!shard.Destroyed)
			shard.Destroy();
	}
}