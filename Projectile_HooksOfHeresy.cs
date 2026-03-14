
using Verse;

public class Projectile_HooksOfHeresy: Projectile {
	// Stolen from Projectile_SpawnsThing
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
		if (thing is MaelstromOrb orb)
		{
			orb.instigator = this.launcher;
			orb.projectileDef = this.def;
		}
		if (!thing.def.CanHaveFaction)
			return;
		thing.SetFaction(this.Launcher.Faction);
	}
}