using RimWorld;
using Verse;

public class Verb_JumpStridesOfHeresy : Verb_CastAbilityJump {
	protected override bool TryCastShot() {
		// Checks if cast is valid
		bool success = base.TryCastShot();
		// If cast is valid & caster is a pawn:
		if (success && this.CasterIsPawn)
		{
			// Applies shadowfade to self
			CasterPawn.health.AddHediff(HediffDef.Named("domom_Shadowfade"));
		}
		// Returns validity check
		return success;
	}
}