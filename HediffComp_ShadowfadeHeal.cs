using System;
using System.Linq;
using RimWorld;
using Verse;

// Stolen & Changed from HediffComp_HealPermanentWounds
public class HediffComp_ShadowfadeHeal : HediffComp {
	public HediffCompProperties_ShadowfadeHeal Props => (HediffCompProperties_ShadowfadeHeal) this.props;

	// Game ticks until the next heal
	private int ticksToHeal;
	
	public override void CompPostMake()
	{
		base.CompPostMake();
		this.ResetTicksToHeal();
	}

	// Changed from HediffComp_HealPermanentWounds
	// Resets ticks until next heal to a random value based on comp properties
	private void ResetTicksToHeal() => this.ticksToHeal = this.Props.ticksBetweenHealRange.RandomInRange;

	public override void CompPostTickInterval(ref float severityAdjustment, int delta)
	{
		// Checks if it is time to heal
		this.ticksToHeal -= delta;
		if (this.ticksToHeal > 0)
			return;
		this.TryHealRandomWound(this.Pawn);
		this.ResetTicksToHeal();
	}

	// Changed from HediffComp_HealPermanentWounds (note: is no longer static)
	// Tries to heal a random wound
	public void TryHealRandomWound(Pawn pawn) {
		// Stores hediff to try & heal
		Hediff result;
		// Finds a random injury that isn't chronic or permanent, otherwise does nothing 
		if (!pawn.health.hediffSet.hediffs.Where(hd => hd.def.hediffClass == typeof(Hediff_Injury) && !hd.IsPermanent() && !hd.def.chronic).TryRandomElement(out result))
			return;
		// Reduces injury's severity by random value, based on comp properties
		HealthUtility.AdjustSeverity(pawn, result.def, this.Props.severityPerHealRange.RandomInRange);
	}

	public override void CompExposeData()
	{
		Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal");
	}

	public override string CompDebugString() => "ticksToHeal: " + this.ticksToHeal.ToString();
}