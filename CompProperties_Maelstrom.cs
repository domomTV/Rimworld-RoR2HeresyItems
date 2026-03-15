using RimWorld;
using Verse;

public class CompProperties_Maelstrom : CompProperties_Explosive {
	public CompProperties_Maelstrom() => this.compClass = typeof (CompMaelstrom);
	
	// Explosion radius for minor detonations
	public float explosiveRadiusMinor;

	// Damage type for minor detonations
	public DamageDef explosiveDamageTypeMinor;

	// Number of minor detonations before the final detonation
	public int minorDetonationsNumber = 1;

	// Delay between detonations
	public int ticksBetweenDetonations = 60;

	// If true, thing will immediately detonate when spawned
	public bool minorDetonationOnSpawn;
}