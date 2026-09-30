using UnityEngine;
using Verse;

public class HungeringShard : AttachableThing {
	// Thing that launched the projectile that spawned this
	public Thing launcher;
	
	// Body part the projectile hit, and that the explosion will try to hit
	public BodyPartRecord bodyPart;
	
	// Attached parent, used for things without attachment comp
	public Thing parentNoComp;
	
	// Log from the projectile
	public LogEntry_DamageResult log;

	// Cached offset for draw position
	// Needed to keep a consistent visual offset
	public Vector3 drawOffset = Vector3.zero;
	public bool cached = false;
	
	public override string InspectStringAddon { get; }

	public override Vector3 DrawPos
	{
		get
		{
			Thing parentThing = this.parentNoComp ?? this.parent;
			// Use thing's position as a starting point
			Vector3 baseVal = base.DrawPos;
			Vector3 ret = baseVal;
			Vector3 offsetScale = new Vector3(1, 0, 1);
			if (parentThing != null)
			{
				// Use center of parent's draw bounds as a starting point
				// Needed for larger parents
				Bounds bounds = parentThing.DrawBounds();
				ret = bounds.center;
				// Keeps draw layer consistent
				ret.y = baseVal.y;
				// Calculates draw offset based on thing's size
				// Allows larger parents to have more spread out shards
				offsetScale = bounds.size;
			}
			
			if (!this.cached)
			{
				Vector3 offset = new Vector3(offsetScale.x * Rand.Range(-0.5f, 0.5f), 0, offsetScale.z * Rand.Range(-0.5f, 0.5f));
				this.drawOffset = offset;
				this.cached = true;
			}

			// Return draw position with cached offset
			return ret + this.drawOffset;
		}
		
	}
	
	public override void ExposeData() {
		base.ExposeData();
		Scribe_References.Look<Thing>(ref this.launcher, "launcher");
		Scribe_BodyParts.Look(ref this.bodyPart, "bodyPart");
		Scribe_References.Look<Thing>(ref this.parentNoComp, "parentNoComp");
		Scribe_References.Look<LogEntry_DamageResult>(ref this.log, "log");
		
	}
}