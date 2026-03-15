using RimWorld;
using Verse;

public class Hediff_ApplyRuin : HediffWithComps {
	// Called when the pawn this hediff is applied to damages a thing
	public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result) {
		// Does nothing if no damage was dealt, thing wasn't a pawn, or damaged pawn will die
		if (result.totalDamageDealt <= 0 || !(thing is Pawn p) || p.health.ShouldBeDead()) 
			return;
		// Preparing hediff def of ruin
		HediffDef hediffDef = HediffDef.Named("domom_Ruin");
		// Stores ruin hediff
		Hediff ruin;
		// Severity of ruin applied is 1/10th damage dealt
		float severity = dinfo.Amount / 10;
		// If thing already has ruin:
		if (p.health.hediffSet.HasHediff(hediffDef))
		{
			// Get their ruin hediff
			ruin = p.health.hediffSet.GetFirstHediffOfDef(hediffDef);
			// If the damage dealt is of type ruin (different from the hediff):
			if (dinfo.Def.defName == "domom_Ruin")
			{
				// Remove the hediff
				p.health.RemoveHediff(ruin);
				return;
			}
			// If this hediff's ability is on cooldown, do nothing
			if (this.AllAbilitiesForReading[0].OnCooldown)
				return;
			// Increment severity
			ruin.Severity += severity;
			// Try to refresh ruin's duration
			HediffComp_Disappears comp = ruin.TryGetComp<HediffComp_Disappears>();
			comp.ticksToDisappear = comp.disappearsAfterTicks;
		}
		// If thing does not have ruin:
		else if(!this.AllAbilitiesForReading[0].OnCooldown)
		{
			// Make the ruin hediff
			ruin = HediffMaker.MakeHediff(hediffDef, p);
			// Set severity
			ruin.Severity = severity;
			// Add hediff to thing
			p.health.AddHediff(ruin);
		}
	}
}