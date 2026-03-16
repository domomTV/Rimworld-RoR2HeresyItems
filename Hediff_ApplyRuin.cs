using RimWorld;
using Verse;

public class Hediff_ApplyRuin : Hediff_AddedPart {
	// Called when the pawn this hediff is applied to damages a thing
	public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result) {
		// Does nothing if no damage was dealt, thing wasn't a pawn, or damaged pawn will die
		// Prevents ruin from being refreshed or consumed if 0 damage was dealt
		if (result.totalDamageDealt <= 0 || !(thing is Pawn p) || p.health.ShouldBeDead()) 
			return;
		
		HediffDef hediffDef = HediffDef.Named("domom_Ruin");
		Hediff ruin;
		// Severity of ruin applied is 1/10th damage dealt
		float severity = dinfo.Amount / 10;
		// If thing already has ruin:
		if (p.health.hediffSet.HasHediff(hediffDef))
		{
			// Get existing hediff
			ruin = p.health.hediffSet.GetFirstHediffOfDef(hediffDef);
			// If damage dealt is of type ruin (different from the hediff), consume the hediff & return
			// It's okay to remove because ruin damage mult happens before this
			if (dinfo.Def.defName == "domom_Ruin")
			{
				p.health.RemoveHediff(ruin);
				return;
			}
			
			// If this ruin ability is on cooldown, do nothing
			if (this.AllAbilitiesForReading[0].OnCooldown)
				return;
			
			ruin.Severity += severity;
			// Try to refresh ruin's duration
			HediffComp_Disappears comp = ruin.TryGetComp<HediffComp_Disappears>();
			comp.ticksToDisappear = comp.disappearsAfterTicks;
		}
		// If thing does not have ruin & ruin ability is off cooldown, create a new hediff to give to thing
		else if (!this.AllAbilitiesForReading[0].OnCooldown)
		{
			ruin = HediffMaker.MakeHediff(hediffDef, p);
			ruin.Severity = severity;
			p.health.AddHediff(ruin);
		}
	}
}