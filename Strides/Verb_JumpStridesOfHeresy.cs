using RimWorld;
using Verse;

// Applies shadowfade before jumping
public class Verb_JumpStridesOfHeresy : Verb_CastAbilityJump {
	protected override bool TryCastShot() {
		bool success = base.TryCastShot();
		if (success && this.CasterIsPawn)
		{
			string defName = "domom_Shadowfade";
			// Checks if cast ability is enhanced version
			if (this.ability.def.defName == "domom_StridesOfHeresy_Enhanced")
			{
				defName = "domom_Shadowfade_Enhanced";
			}
			
			// Applies shadowfade to self
			this.CasterPawn.health.AddHediff(HediffDef.Named(defName));
		}
		
		return success;
	}
}