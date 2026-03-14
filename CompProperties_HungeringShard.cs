using Verse;

public class CompProperties_HungeringShard : CompProperties {
	public CompProperties_HungeringShard() => this.compClass = typeof (CompHungeringShard);

	public DamageDef damageType;

	public int damageAmountBase = -1;

	public int ticksUntilDetonation;

}