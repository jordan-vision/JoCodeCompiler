namespace ASTGenerator;

public class ASTGenerator
{
    private static FileStream? astStream;
    private static StreamWriter? astWriter;

    public static void OpenSourceFile(string filename)
    {
        SemanticStack.ResetStack();
        astWriter?.Close();

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outastFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outast";
        astStream = File.Create(Path.Combine(outputDirectory, outastFilename));
        astWriter = new(astStream);
    }

    public static void WriteAST()
    {
        astWriter?.WriteLine(SemanticStack.WriteTree());
        astWriter?.Flush();
    }
}
