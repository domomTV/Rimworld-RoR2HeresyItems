using UnityEngine;
using Verse;

public class HungeringShard : AttachableThing {
	public Thing launcher;
	public BodyPartRecord bodyPart;
	public Thing parentNoComp;
	public LogEntry_DamageResult log;

	public Vector3 drawOffset = Vector3.zero;
	public bool cached = false;
	
	public override string InspectStringAddon { get; }

	public override Vector3 DrawPos
	{
		get
		{
			Thing parent = this.parentNoComp != null ? this.parentNoComp : this.parent;
			Vector3 baseVal = base.DrawPos;
			if (parent == null)
				return baseVal;
			Bounds bounds = parent.DrawBounds();
			Vector3 ret = bounds.center;
			ret.y = baseVal.y;
			if (!this.cached)
			{
				Vector3 size = bounds.size;
				Vector3 offset = new Vector3(size.x * Rand.Range(-0.5f, 0.5f), 0, size.z * Rand.Range(-0.5f, 0.5f));
				this.drawOffset = offset;
				this.cached = true;
			}

			return ret + this.drawOffset;
		}
		
	}
}