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
}