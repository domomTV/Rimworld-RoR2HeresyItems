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
			Thing parent = this.parentNoComp ?? this.parent;
			Vector3 baseVal = base.DrawPos;
			// Uses base drawPos if no parent attached, 
			if (parent == null)
				return baseVal;
			// Use center of parent's draw bounds as a starting point
			// Needed for larget parents
			Bounds bounds = parent.DrawBounds();
			Vector3 ret = bounds.center;
			// Set y to base value
			// Keeps draw layer consistent
			ret.y = baseVal.y;
			if (!this.cached)
			{
				Vector3 size = bounds.size;
				// Calculates draw offset based on thing's size
				// Allows larger parents to have more spread out shards
				Vector3 offset = new Vector3(size.x * Rand.Range(-0.5f, 0.5f), 0, size.z * Rand.Range(-0.5f, 0.5f));
				// Stores offset & marks it as cached
				this.drawOffset = offset;
				this.cached = true;
			}

			// Return draw position with cached offset
			return ret + this.drawOffset;
		}
		
	}
}