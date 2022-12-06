namespace Lab6_NET.Models;

public record Operation(Delegate Delegate, Parameters Parameters)
{ public override string ToString() => $"{Delegate.Method.Name}: {Parameters}"; }