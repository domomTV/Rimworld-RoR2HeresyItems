using Verse;

public class CompProperties_HungeringShard : CompProperties {
	public CompProperties_HungeringShard() => this.compClass = typeof (CompHungeringShard);

	// Damage type of explosion
	public DamageDef damageType;

	// Damage for the explosion, if not defined damage type's base damage is used
	public int damageAmountBase = -1;

	// Delay until explosion
	public int ticksUntilDetonation;

}