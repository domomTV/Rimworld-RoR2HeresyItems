using Verse;

public class HediffCompProperties_ShadowfadeHeal : HediffCompProperties{
	public HediffCompProperties_ShadowfadeHeal()
	{
		this.compClass = typeof (HediffComp_ShadowfadeHeal);
	}

	public FloatRange severityPerHealRange;

	public IntRange ticksBetweenHealRange;
}