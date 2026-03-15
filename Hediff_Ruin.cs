using Verse;

public class Hediff_Ruin : HediffWithComps {
	
	// Display severity as a decimal number
	public override string SeverityLabel => this.Severity == 0 ? (string) null : (this.Severity).ToString("F2");
	
}