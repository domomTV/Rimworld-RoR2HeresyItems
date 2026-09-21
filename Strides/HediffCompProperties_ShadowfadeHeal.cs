using Verse;

public class HediffCompProperties_ShadowfadeHeal : HediffCompProperties {
	public HediffCompProperties_ShadowfadeHeal() => this.compClass = typeof (HediffComp_ShadowfadeHeal);

	// Amount healed, random within a range
	public FloatRange severityPerHealRange;

	// Delay between heals, random within a range
	public IntRange ticksBetweenHealRange;
}