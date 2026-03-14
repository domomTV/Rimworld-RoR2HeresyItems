using Verse;

public class CompHungeringShard : ThingComp{
	public CompProperties_HungeringShard Props => (CompProperties_HungeringShard) this.props;

	public int ticksUntilDetonation;

	public override void Initialize(CompProperties props) {
		base.Initialize(props);
		this.ticksUntilDetonation = Props.ticksUntilDetonation;
	}
	
	public override void CompTickInterval(int delta) 
	{
		if (this.ticksUntilDetonation <= 0)
			return;
		this.ticksUntilDetonation -= delta;
		if (this.ticksUntilDetonation <= 0)
			this.Detonate();
	}

	public void Detonate() {
		if (!(this.parent is HungeringShard shard) || shard.Destroyed)
			return;
		int damage = this.Props.damageAmountBase != -1 ? this.Props.damageAmountBase : this.Props.damageType.defaultDamage;
		DamageInfo damageInfo = new DamageInfo(this.Props.damageType, damage, this.Props.damageType.defaultArmorPenetration, instigator: shard.launcher, intendedTarget: shard.parent);
		if (shard.parentNoComp != null)
			shard.parentNoComp.TakeDamage(damageInfo);
		else if (shard.parent != null)
		{
			damageInfo.SetHitPart(shard.bodyPart);
			shard.parent.TakeDamage(damageInfo).AssociateWithLog(shard.log);
		}
		
		if (!shard.Destroyed)
			shard.Destroy();
	}
}