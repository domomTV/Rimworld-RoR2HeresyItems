using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

public class CompHungeringShard : ThingComp {
	public CompProperties_HungeringShard Props => (CompProperties_HungeringShard) this.props;

	// Game ticks until shard detonates
	public int ticksUntilDetonation;

	public override void Initialize(CompProperties p) {
		base.Initialize(p);
		this.ticksUntilDetonation = Props.ticksUntilDetonation;
	}
	
	public override void CompTickInterval(int delta) 
	{
		// Destroys shard as soon as they're invalid to avoid glitched visuals
		if (!isParentPawnValid())
		{
			parent.Destroy();
			return;
		}
		
		if (this.ticksUntilDetonation <= 0)
			return;
		this.ticksUntilDetonation -= delta;
		if (this.ticksUntilDetonation <= 0)
			this.Detonate();
	}

	public void Detonate() {
		if (!(this.parent is HungeringShard shard) || shard.Destroyed)
			return;
		
		// Gets damage dealt from comp properties, or damage def as fallback
		int damage = this.Props.damageAmountBase != -1 ? this.Props.damageAmountBase : this.Props.damageType.defaultDamage;
		// Construct damage info from comp properties & projectile info
		DamageInfo damageInfo = new DamageInfo(this.Props.damageType, damage, this.Props.damageType.defaultArmorPenetration, instigator: shard.launcher, intendedTarget: shard.parent);
		damageInfo.SetWeaponHediff(HediffDef.Named("domom_VisionsOfHeresy"));
		// If parent isn't a pawn:
		if (shard.parentNoComp != null)
			shard.parentNoComp.TakeDamage(damageInfo);
		// If parent is a pawn:
		else if (shard.parent != null)
		{
			// Targets the same part the projectile hit
			damageInfo.SetHitPart(shard.bodyPart);
			// Link the damage to the projectile's log
			shard.parent.TakeDamage(damageInfo).AssociateWithLog(shard.log);
		}
		// If no parent:
		else
		{
			SoundDefOf.BulletImpact_Ground.PlayOneShot((SoundInfo) new TargetInfo(shard.Position, shard.Map));
		}
		
		// Shard is destroyed no matter what
		if (!shard.Destroyed)
			shard.Destroy();
	}

	// Checks if shard's parent is being carried or flying away
	private bool isParentPawnValid() {
		// Uses parent pawn's job driver to see if they are being carried or flying away.
		// Flying away seems to also use carried so we check for both
		JobDriver driver = ((Pawn) ((HungeringShard) this.parent)?.parent)?.jobs.curDriver;
		return !(driver is JobDriver_ExitMapFlying || driver is JobDriver_Carried);
	}
}