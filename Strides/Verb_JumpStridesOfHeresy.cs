using RimWorld;
using Verse;

public class Verb_JumpStridesOfHeresy : Verb_CastAbilityJump {
	protected override bool TryCastShot() {
		// Checks if cast is valid
		bool success = base.TryCastShot();
		if (success && this.CasterIsPawn)
		{
			// Applies shadowfade to self
			CasterPawn.health.AddHediff(HediffDef.Named("domom_Shadowfade"));
		}
		
		return success;
	}
}