using System;
using System.Linq;
using RimWorld;
using Verse;

// Stolen & Changed from HediffComp_HealPermanentWounds
public class HediffComp_ShadowfadeHeal : HediffComp {
	private int ticksToHeal;

	public HediffCompProperties_ShadowfadeHeal Props => (HediffCompProperties_ShadowfadeHeal) this.props;

	public override void CompPostMake()
	{
		base.CompPostMake();
		this.ResetTicksToHeal();
	}

	// Changed from HediffComp_HealPermanentWounds
	private void ResetTicksToHeal() => this.ticksToHeal = this.Props.ticksBetweenHealRange.RandomInRange;

	public override void CompPostTickInterval(ref float severityAdjustment, int delta)
	{
		this.ticksToHeal -= delta;
		if (this.ticksToHeal > 0)
			return;
		this.TryHealRandomWound(this.Pawn, this.parent.LabelCap);
		this.ResetTicksToHeal();
	}

	// Changed from HediffComp_HealPermanentWounds (note: is no longer static)
	public void TryHealRandomWound(Pawn pawn, string cause) {
		Hediff result;
		if (!pawn.health.hediffSet.hediffs.Where(hd => hd.def.hediffClass == typeof(Hediff_Injury) && !hd.IsPermanent() && !hd.def.chronic).TryRandomElement(out result))
			return;
		HealthUtility.AdjustSeverity(pawn, result.def, this.Props.severityPerHealRange.RandomInRange);
		// if (!PawnUtility.ShouldSendNotificationAbout(pawn))
		// 	return;
		// Messages.Message((string) "MessagePermanentWoundHealed".Translate((NamedArgument) cause, (NamedArgument) pawn.LabelShort, (NamedArgument) result.Label, pawn.Named("PAWN")), (LookTargets) (Thing) pawn, MessageTypeDefOf.PositiveEvent);
	}

	public override void CompExposeData()
	{
		Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal");
	}

	public override string CompDebugString() => "ticksToHeal: " + this.ticksToHeal.ToString();
}