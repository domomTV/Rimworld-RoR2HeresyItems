using Verse;

public class MaelstromOrb : ThingWithComps {
	// Thing that launched the projectile that spawned this
	public Thing instigator;
	// Def of projectile
	public ThingDef projectileDef;
	
	public override void ExposeData() {
		base.ExposeData();
		Scribe_References.Look<Thing>(ref this.instigator, "instigator");
		Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
	}
}
