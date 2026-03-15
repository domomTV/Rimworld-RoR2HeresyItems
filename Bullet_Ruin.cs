using RimWorld;
using Verse;

public class Bullet_Ruin: Bullet {
	private float damageScale = 1f;
	
	// Scales damage dealt on impact by damageScale
	public override int DamageAmount => (int) (base.DamageAmount * this.damageScale);

	protected override void Impact(Thing hitThing, bool blockedByShield = false) {
		if (hitThing is Pawn p)
		{
			// Searches for Ruin hediff
			Hediff hediff = p.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("domom_Ruin"));
			if (hediff != null && hediff is Hediff_Ruin ruin)
				// If found, scale damage by severity
				this.damageScale = ruin.Severity;
			else
				// Else damage dealt is zero
				this.damageScale = 0;
		}
		// Proceed to impact thing
		base.Impact(hitThing, blockedByShield);
	}
}
