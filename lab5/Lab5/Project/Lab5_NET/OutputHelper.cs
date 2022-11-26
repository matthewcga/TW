using System.Diagnostics;

namespace Lab5_NET;

/// <summary>
/// Klasa pomagająca wypisywać i zapisywać rezultaty
/// </summary>
public class OutputHelper : IDisposable
{
    private readonly FileStream _stream;
    private readonly StreamWriter _writer;

    
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
    /// <param name="consoleColor">kolor konsoli</param>
    /// <param name="args">argumenty do wypisania</param>
    public void PrintAndSaveToFile(ConsoleColor consoleColor, params string[] args)
    {
        Console.ForegroundColor = consoleColor;
        foreach (var arg in args)
        {
            _writer.WriteLine(arg);
            Console.WriteLine(arg);
        }
        Console.ResetColor();
    }


    /// <summary>
    /// Wypisuje argument w podanym kolorze.
    /// </summary>
    /// <param name="consoleColor">kolor konsoli</param>
    /// <param name="message">argument do wypisania</param>
    public static void Print(ConsoleColor consoleColor, string message)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message);
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
    /// <param name="outFile">plik z rezultatami w którym wpisane są już rezultaty</param>
    /// <exception cref="Exception">Błedy związane z uruchomieniem skryptu</exception>
    public static void GenerateImage(string outFile)
    {
        var assemblyLoc = System.Reflection.Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrEmpty(assemblyLoc))
            throw new Exception("Nie znaleziono lokalizacji pliku .exe!");
        
        var py = $"{Path.GetDirectoryName(assemblyLoc)}\\GraphGenerator.py";
        if (!File.Exists(py))
            throw new Exception("Nie znaleziono skryptu generującego grafy!");
        
        var process = new Process();
        process.StartInfo.FileName = "python.exe";
        process.StartInfo.Arguments = $"-u \"{py}\" \"{outFile}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        
        process.Start();
        process.WaitForExit();
        
        Print(ConsoleColor.Green, process.StandardOutput.ReadToEnd());
    }
}