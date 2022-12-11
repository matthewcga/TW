using System.Diagnostics;

namespace Lab6_NET.Logic;

public static class GraphHelper
{
    /// <summary>
    /// Metoda uruchomi skrypt python który wygeneruje graf zależności.
    /// (instancja OutputHelper musi być Disposed żeby można było odczytać plik z rezultatami)
    /// </summary>
    /// <param name="outFile">plik z rezultatami</param>
    /// <param name="fnf">postać normalna Foaty</param>
    /// <exception cref="Exception">Błędy związane z uruchomieniem skryptu</exception>
    public static void GenerateFnfImage(string outFile, NormalForm fnf)
    {
        var tmpFile = Path.GetTempFileName();
        using(var sw = new StreamWriter(tmpFile))
            sw.Write(fnf.GetCsv());

        var assemblyLoc = System.Reflection.Assembly.GetEntryAssembly()?.Location;
        if (string.IsNullOrEmpty(assemblyLoc))
            throw new Exception("Nie znaleziono lokalizacji pliku .exe!");
        
        var py = $"{Path.GetDirectoryName(assemblyLoc)}\\GraphGenerator.py";
        if (!File.Exists(py))
            throw new Exception("Nie znaleziono skryptu generującego grafy!");
        
        var process = new Process();
        process.StartInfo.FileName = "python.exe";
        process.StartInfo.Arguments = $"-u \"{py}\" \"{tmpFile}\" \"{outFile}\"";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        
        process.Start();
        process.WaitForExit();
        OutputHelper.Print(process.StandardOutput.ReadToEnd());
        
        File.Delete(tmpFile);
    }
}