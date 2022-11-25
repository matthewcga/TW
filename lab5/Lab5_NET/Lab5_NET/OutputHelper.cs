using System.Diagnostics;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace Lab5_NET;

public class OutputHelper : IDisposable
{
    private readonly FileStream _ostrm;
    private readonly StreamWriter _writer;

    public OutputHelper(string path)
    {
        _ostrm = new FileStream (path, FileMode.Create, FileAccess.Write);
        _writer = new StreamWriter (_ostrm);
    }

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

    public static void Print(ConsoleColor consoleColor, string message)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Dispose()
    {
        _writer.Close();
        _ostrm.Close();
    }

    public static void GenerateImage(string inFile)
    {
        var assemblyLoc = System.Reflection.Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrEmpty(assemblyLoc))
            throw new Exception("Nie znaleziono lokalizacji pliku .exe!");
        
        var py = $"{Path.GetDirectoryName(assemblyLoc)}\\GraphGenerator.py";
        if (!File.Exists(py))
            throw new Exception("Nie znaleziono skryptu generującego grafy!");
        
        var process = new Process();
        process.StartInfo.FileName = $"python -u \"{py}\" \"{inFile}\"";
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        
        process.Start();
        process.WaitForExit();
        
        Print(ConsoleColor.Green, process.StandardOutput.ReadToEnd());
    }
}