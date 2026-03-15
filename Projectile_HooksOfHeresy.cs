using Verse;

// Stolen & edited from Projectile_SpawnsThing
public class Projectile_HooksOfHeresy: Projectile {
	protected override void Impact(Thing hitThing, bool blockedByShield = false)
	{
		Map map = this.Map;
		base.Impact(hitThing, blockedByShield);
		IntVec3 loc = this.Position;
		if (this.def.projectile.tryAdjacentFreeSpaces && this.Position.GetFirstBuilding(map) != null)
		{
			foreach (IntVec3 c in GenAdjFast.AdjacentCells8Way(this.Position))
			{
				if (c.GetFirstBuilding(map) == null && c.Standable(map))
				{
					loc = c;
					break;
				}
			}
		}
		Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.def.projectile.spawnsThingDef), loc, map);
		// If thing to spawn is MaelstromOrb:
		if (thing is MaelstromOrb orb)
		{
			// Sets launcher
			orb.instigator = this.launcher;
			// Sets projectile def
			orb.projectileDef = this.def;
		}
		if (!thing.def.CanHaveFaction)
			return;
		thing.SetFaction(this.Launcher.Faction);
	}
}