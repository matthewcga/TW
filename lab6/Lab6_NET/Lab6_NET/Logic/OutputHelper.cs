namespace Lab6_NET.Logic;

/// <summary>
/// Klasa pomagająca wypisywać i zapisywać rezultaty
/// </summary>
public class OutputHelper : IDisposable
{
    private readonly FileStream _stream;
    private readonly StreamWriter _writer;
    private static int _colorIndex;
    private static readonly ConsoleColor[] Colors =
        { ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Magenta };
    private static ConsoleColor ConsoleColor => Colors[_colorIndex % Colors.Length];
    
    
    /// <summary>
    /// Konstruktor przygotowuje się do zapisu do pliku.
    /// </summary>
    /// <param name="path">ścieżka do pliku wyjściowego</param>
    public OutputHelper(string path)
    {
        _stream = new FileStream (path, FileMode.Create, FileAccess.Write);
        _writer = new StreamWriter (_stream);
    }
    

    /// <summary>
    /// Wypisuje i zapisuje argumenty w podanym kolorze.
    /// </summary>
    /// <param name="args">argumenty do wypisania</param>
    public void PrintAndWriteToFile(params string[] args)
    {
        Console.ForegroundColor = ConsoleColor;
        foreach (var arg in args)
        {
            _writer.WriteLine(arg);
            Console.WriteLine(arg);
        }
        Console.ResetColor();
    }
    
    public void WriteToFile(params string[] args)
    {
        foreach (var arg in args)
            _writer.WriteLine(arg);
    }
    
    
    /// <summary>
    /// Zmiana koloru konsoli
    /// </summary>
    public static void ChangeSectionColor()
    { _colorIndex++; }
    
    
    /// <summary>
    /// Zakończenie zapisu do pliku.
    /// </summary>
    public void Dispose()
    {
        _writer.Close();
        _stream.Close();
    }
    
    
    /// <summary>
    /// Wypisuje argument w podanym kolorze.
    /// </summary>
    public static void Print(params string[] args)
    {
        Console.ForegroundColor = ConsoleColor;
        foreach (var arg in args)
            Console.WriteLine(arg);
        Console.ResetColor();
    }
    
    
    public static void Error(Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception.Message);
        Console.ResetColor();
    }

    public static (string outFile, string imgFile, string slnFile) GetFilePaths(string inFile)
    {
        var fileBase = $"{Path.GetDirectoryName(inFile)}\\{Path.GetFileNameWithoutExtension(inFile)}";
        return ($"{fileBase}_results.txt", $"{fileBase}_graph.png", $"{fileBase}_solution.txt");
    }
}