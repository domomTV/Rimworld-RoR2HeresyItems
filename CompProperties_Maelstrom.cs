using RimWorld;
using Verse;

public class CompProperties_Maelstrom : CompProperties_Explosive {
	public CompProperties_Maelstrom() => this.compClass = typeof (CompMaelstrom);
	
	public float explosiveRadiusMinor;

	public DamageDef explosiveDamageTypeMinor;

	public int minorDetonationsNumber = 1;

	public int ticksBetweenDetonations = 60;

	public bool minorDetonationOnSpawn;
}