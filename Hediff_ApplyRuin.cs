using RimWorld;
using Verse;

public class Hediff_ApplyRuin : HediffWithComps {
	public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result) {
		if (result.totalDamageDealt <= 0 || !(thing is Pawn p) || p.health.ShouldBeDead()) 
			return;
		HediffDef hediffDef = HediffDef.Named("domom_Ruin");
		Hediff ruin;
		float severity = dinfo.Amount / 10;
		if (p.health.hediffSet.HasHediff(hediffDef))
		{
			ruin = p.health.hediffSet.GetFirstHediffOfDef(hediffDef);
			if (dinfo.Def.defName == "domom_Ruin")
			{
				p.health.RemoveHediff(ruin);
				return;
			}
			if (this.AllAbilitiesForReading[0].OnCooldown)
				return;
			ruin.Severity += severity;
			HediffComp_Disappears comp = ruin.TryGetComp<HediffComp_Disappears>();
			comp.ticksToDisappear = comp.disappearsAfterTicks;
		}
		else if(!this.AllAbilitiesForReading[0].OnCooldown)
		{
			ruin = HediffMaker.MakeHediff(hediffDef, p);
			ruin.Severity = severity;
			p.health.AddHediff(ruin);
		}
	}
}