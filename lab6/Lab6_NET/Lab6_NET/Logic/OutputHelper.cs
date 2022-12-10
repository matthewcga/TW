using System.Diagnostics;

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
    private static ConsoleColor NextColor => Colors[_colorIndex % Colors.Length];
    
    
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
    public void PrintAndSaveToFile(params string[] args)
    {
        Console.ForegroundColor = NextColor;
        foreach (var arg in args)
        {
            _writer.WriteLine(arg);
            Console.WriteLine(arg);
        }
        Console.ResetColor();
    }
    
    
    /// <summary>
    /// Zmiana koloru konsoli
    /// </summary>
    public static void ChangeSectionColor()
    { _colorIndex++; }
    
    
    /// <summary>
    /// Wypisuje argument w podanym kolorze.
    /// </summary>
    public static void Print(params string[] args)
    {
        Console.ForegroundColor = NextColor;
        foreach (var arg in args)
            Console.WriteLine(arg);
        Console.ResetColor();
    }
    
    
    /// <summary>
    /// Zakończenie zapisu do pliku.
    /// </summary>
    public void Dispose()
    {
        _writer.Close();
        _stream.Close();
    }
    
    
    /// <summary>
    /// Metoda uruchomi skrypt python'owy który wygeneruje graf zależności.
    /// (instancja OutputHelper musi być Disposed żeby można było odczytać plik z rezultatami)
    /// </summary>
    /// <param name="outFile">plik z rezultatami</param>
    /// <param name="fnf">postać normalna foaty</param>
    /// <exception cref="Exception">Błedy związane z uruchomieniem skryptu</exception>
    public static void GenerateFnfImage(string outFile, NormalForm fnf)
    {
        var tmpPath = Path.GetTempFileName();
        using(var sw = new StreamWriter(tmpPath))
            sw.Write(fnf.GetCsv());

        var assemblyLoc = System.Reflection.Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrEmpty(assemblyLoc))
            throw new Exception("Nie znaleziono lokalizacji pliku .exe!");
        
        var py = $"{Path.GetDirectoryName(assemblyLoc)}\\GraphGenerator.py";
        if (!File.Exists(py))
            throw new Exception("Nie znaleziono skryptu generującego grafy!");
        
        var process = new Process();
        process.StartInfo.FileName = "python.exe";
        process.StartInfo.Arguments = $"-u \"{py}\" \"{tmpPath}\" \"{outFile}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        
        process.Start();
        process.WaitForExit();
        Print(process.StandardOutput.ReadToEnd());
        
        File.Delete(tmpPath);
    }

    
    public static void Error(Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception.Message);
        Console.ResetColor();
    }
}