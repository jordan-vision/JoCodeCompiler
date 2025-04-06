namespace CodeGenerator;

public class CodeGenerator
{
    private static FileStream? moonStream;
    private static StreamWriter? moonWriter;

    public static void OpenSourceFile(string filename)
    {
        moonWriter?.Close();

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outastFilename = $"{Path.GetFileNameWithoutExtension(filename)}.moon";
        moonStream = File.Create(Path.Combine(outputDirectory, outastFilename));
        moonWriter = new(moonStream);
    }

    public static string MoonCodeLine(string symbol, string opCode, List<string> operands, string comment)
    {
        var line = "";
        line += symbol == "" ? "" : symbol;
        line += $"\t{opCode}";
        line += operands.Count == 0 ? "\t\t" : $"\t\t{String.Join(",", operands)}";
        line += comment == "" ? "\t\t" : $"\t\t%{comment}";
        line += "\n";

        return line;
    }

    public static void WriteMoonCode(string code)
    {
        moonWriter?.Write(code);
        moonWriter?.Flush();
    }
}