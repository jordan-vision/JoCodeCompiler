using System.Diagnostics;
using Lex = LexicalAnalyzer.LexicalAnalyzer;

namespace SyntacticAnalyzer;

public class Parser
{
    private static FileStream? derivationStream, syntaxErrorStream;
    private static StreamWriter? derivationWriter, syntaxErrorWriter;

    /// <summary>
    /// Opens file in lexical analyzer
    /// </summary>
    /// <param name="filename">Name of file to open</param>
    public static void OpenSourceFile(string filename)
    {
        ParsingTable.ResetStack();
        
        derivationWriter?.Close();
        syntaxErrorWriter?.Close();

        Lex.OpenSourceFile(filename);

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outDerivationFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outderivation";
        //var outSyntaxErrorsErrorsFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outsyntaxerrors";

        derivationStream = File.Create(Path.Combine(outputDirectory, outDerivationFilename));
        //syntaxErrorStream = File.Create(Path.Combine(outputDirectory, outSyntaxErrorsErrorsFilename));

        derivationWriter = new(derivationStream);
        //syntaxErrorWriter = new(syntaxErrorStream);
    }

    /// <summary>
    /// Parses tokens received from lexical analyzer
    /// </summary>
    public static void Parse()
    {
        LexicalAnalyzer.Token? token;
        
        do
        {
            bool isTokenValid;

            do
            {
                token = Lex.NextToken();
                isTokenValid = true;

                if (token == null)
                {
                    Debug.WriteLine("ERROR. Source file does not exist");
                    return;
                }

                if (!GrammarSymbols.TERMINALS.Contains(token.Type))
                {
                    isTokenValid = false;
                }

            } while (!isTokenValid);

            ParsingTable.Derive(token.Type);

        } while (!token.Type.Equals(GrammarSymbols.END));
    }

    public static void WriteDerivation(string derivation)
    {
        derivationWriter?.WriteLine(derivation);
        derivationWriter?.WriteLine();

        Console.WriteLine(derivation);
        Console.WriteLine();
    }
}
