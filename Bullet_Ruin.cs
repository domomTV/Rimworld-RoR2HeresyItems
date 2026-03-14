using RimWorld;
using Verse;

public class Bullet_Ruin: Bullet {
	private float damageScale = 1f;
	
	public override int DamageAmount => (int) (base.DamageAmount * this.damageScale);

	protected override void Impact(Thing hitThing, bool blockedByShield = false) {
		if (hitThing is Pawn p)
		{
			Hediff hediff = p.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("domom_Ruin"));
			if (hediff != null && hediff is Hediff_Ruin ruin)
				this.damageScale = ruin.Severity;
			else
				this.damageScale = 0;
		}
		base.Impact(hitThing, blockedByShield);
	}
}
