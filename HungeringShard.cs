using UnityEngine;
using Verse;

public class HungeringShard : AttachableThing {
	// Thing that launched the projectile that spawned this
	public Thing launcher;
	// Body part the projectile hit
	public BodyPartRecord bodyPart;
	// Attached parent, used for things without attachment comp
	public Thing parentNoComp;
	// Log from the projectile
	public LogEntry_DamageResult log;

	// Cached offset for draw position
	public Vector3 drawOffset = Vector3.zero;
	// If true, drawOffset will be used, otherwise drawOffset is chosen & set
	public bool cached = false;
	
	public override string InspectStringAddon { get; }

	public override Vector3 DrawPos
	{
		get
		{
			// Gets parent, with or without attachment comp
			Thing parent = this.parentNoComp != null ? this.parentNoComp : this.parent;
			// Gets base draw position
			Vector3 baseVal = base.DrawPos;
			// If no parent, use base position
			if (parent == null)
				return baseVal;
			// Gets parent's draw bound data
			Bounds bounds = parent.DrawBounds();
			// Use center of draw bounds as a starting point
			Vector3 ret = bounds.center;
			// Set y to base value
			ret.y = baseVal.y;
			// If offset isn't already cached:
			if (!this.cached)
			{
				// Gets thing's size
				Vector3 size = bounds.size;
				// Calculates draw offset based on thing's size
				Vector3 offset = new Vector3(size.x * Rand.Range(-0.5f, 0.5f), 0, size.z * Rand.Range(-0.5f, 0.5f));
				// Stores offset & marks it as cached
				this.drawOffset = offset;
				this.cached = true;
			}

			// Return draw position with offset
			return ret + this.drawOffset;
		}
		
	}
}