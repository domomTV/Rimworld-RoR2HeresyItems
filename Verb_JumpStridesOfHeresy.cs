using RimWorld;
using Verse;

public class Verb_JumpStridesOfHeresy : Verb_CastAbilityJump {
	protected override bool TryCastShot() {
		bool success = base.TryCastShot();
		if (this.CasterIsPawn)
		{
			CasterPawn.health.AddHediff(HediffDef.Named("domom_Shadowfade"));
		}
		return success;
	}
}