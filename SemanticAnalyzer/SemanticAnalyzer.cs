using ASTGenerator;

namespace SemanticAnalyzer;

public class SemanticAnalyzer
{
    private static FileStream? symbolTableStream, semanticErrorsStream;
    private static StreamWriter? symbolTableWriter, semanticErrorsWriter;

    public static void OpenSourceFile(string filename)
    {
        symbolTableWriter?.Close();
        semanticErrorsWriter?.Close();

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outsymboltableFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outSymbolTable";
        var outsemanticerrorFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outSemanticErrors";

        symbolTableStream = File.Create(Path.Combine(outputDirectory, outsymboltableFilename));
        semanticErrorsStream = File.Create(Path.Combine(outputDirectory, outsemanticerrorFilename));

        symbolTableWriter = new(symbolTableStream);
        semanticErrorsWriter = new(semanticErrorsStream);
    }

    public static void TraverseAST()
    {
        SemanticStack.Traverse(new SymbolTableGeneratorVisitor());
        SemanticStack.Traverse(new SemanticCheckVisitor());
        Console.Write(SemanticStack.WriteSymbolTable());
    }

    public static void WriteSemanticError(string message)
    {
        semanticErrorsWriter?.WriteLine($"Semantic error. {message}");
    }

    public static void WriteWarning(string message)
    {
        semanticErrorsWriter?.WriteLine($"Warning. {message}");
    }
}
