using Verse;

public class Hediff_Ruin : HediffWithComps {
	
	// Display severity as a decimal number to represent damage multiplier
	public override string SeverityLabel => this.Severity == 0 ? (string) null : "x" + (this.Severity).ToString("F2");
	
}