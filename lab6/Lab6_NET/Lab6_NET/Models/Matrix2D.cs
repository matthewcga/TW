using System.Text;

namespace Lab6_NET.Models;

public class Matrix2D
{
    public int Size { get; }
    public decimal[][] Matrix  { get; set; }
    public List<Operation> Operations { get; } = new();
    
    
    public Matrix2D(int size)
    {
        Size = size;
        Matrix = new decimal[Size][];
        for (var i = 0; i < Size; i++)
            Matrix[i] = new decimal[Size + 1];
    }

    
    #region Setters, Getters, Misc
    public decimal this[int row, int col]
    { get => Matrix[row][col]; set => Matrix[row][col] = value; }
    

    public override string ToString()
    {
        StringBuilder sb = new();
        for (var i = 0; i < Size; i++)
            sb.AppendLine(
                $"[ {string.Concat(Matrix[i][..Size].Select(x => $"{x, 7:##.0#}"))} |" +
                $"{Matrix[i][Size], 7:##.0#} ]");
        return sb.ToString();
    }
    
    private void SwapRows(int rowA, int rowB)
    { (Matrix[rowA], Matrix[rowB]) = (Matrix[rowB], Matrix[rowA]); }
    
    private decimal Call(Func<Parameters, decimal> operation, Parameters parameters)
    {
        Operations.Add(new Operation(operation, parameters));
        return operation(parameters);
    }
    
    private void Call(Action<Parameters> operation, Parameters parameters)
    {
        Operations.Add(new Operation(operation, parameters));
        operation(parameters);
    }

    #endregion
    
    
    #region Operations
    private decimal A(Parameters parameters)//int rowI, int rowK)
    { return Matrix[parameters.K!.Value][parameters.I] / Matrix[parameters.I][parameters.I]; }

    private void B(Parameters parameters)
    { Matrix[parameters.I][parameters.J!.Value] *= parameters.Value!.Value; }
    
    private void C(Parameters parameters)
    { Matrix[parameters.K!.Value][parameters.J!.Value] -= Matrix[parameters.I][parameters.J.Value]; }
    
    private void D(Parameters parameters) // colJ ???, assign Matrix[rowI][rowI] to var?
    {
        Matrix[parameters.I][Size] /= Matrix[parameters.I][parameters.I];  // on vector
        Matrix[parameters.I][parameters.I] /= Matrix[parameters.I][parameters.I]; // always = 1m; // TODO different op ???
    }
    
    private void E(Parameters parameters) // colJ, assign (Matrix[rowK][rowI] * Matrix[rowI][Size]) to var?
    {
        Matrix[parameters.K!.Value][Size] -= Matrix[parameters.K.Value][parameters.I]; // on vector
        Matrix[parameters.K.Value][parameters.I] -= Matrix[parameters.K.Value][parameters.I]; // = 0m;  // TODO different op ???
    }

    #endregion

    public void TrySolve()
    {
        for (var i = 0; i < Size - 1; i++)
        for (var k = i + 1; k < Size; k++)
        {
            var multiplayer = Call(A, new Parameters(I:i, K:k));
            for (var j = 0; j < Size + 1; j++)
            {
                Call(B, new Parameters(I:i, K:k, J:j, Value:multiplayer));
                Call(C, new Parameters(I:i, K:k, J:j));
            }
        }

        for (var i = Size - 1; i >= 0; i--)
        {
            Call(D, new Parameters(I:i));
            for (var k = i - 1; k >= 0; k--)
                Call(E, new Parameters(I:i, K:k));
        }
    }
}