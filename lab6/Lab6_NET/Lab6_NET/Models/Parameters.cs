namespace Lab6_NET.Models;

public record Parameters(int I, int? J = null, int? K = null, decimal? Value = null)
{ public override string ToString() => $"i= {I,2}, j= {J,2}, k= {K,2}"; }