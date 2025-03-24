using System.Diagnostics;
using Lex = LexicalAnalyzer.LexicalAnalyzer;
using ASTGen = ASTGenerator.ASTGenerator;
using Sem = SemanticAnalyzer.SemanticAnalyzer;

namespace SyntacticAnalyzer;

public class Parser
{
    private static FileStream? derivationStream, syntaxErrorStream;
    private static StreamWriter? derivationWriter, syntaxErrorWriter;
    private static LexicalAnalyzer.Token? token;
    private static bool nextTokenFlag = false, isProgramSyntacticallyCorrect = true;

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
        ASTGen.OpenSourceFile(filename);
        Sem.OpenSourceFile(filename);

        nextTokenFlag = false;
        isProgramSyntacticallyCorrect = true;

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outDerivationFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outDerivation";
        var outSyntaxErrorsErrorsFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outSyntaxErrors";

        derivationStream = File.Create(Path.Combine(outputDirectory, outDerivationFilename));
        syntaxErrorStream = File.Create(Path.Combine(outputDirectory, outSyntaxErrorsErrorsFilename));

        derivationWriter = new(derivationStream);
        syntaxErrorWriter = new(syntaxErrorStream);
    }

    /// <summary>
    /// Parses tokens received from lexical analyzer
    /// </summary>
    public static void Parse()
    {
        do
        {
            if (!nextTokenFlag)
            {
                token = NextToken();
            } 
            else
            {
                nextTokenFlag = true;
            }

            if (token == null)
            {
                Console.WriteLine("ERROR! Token stream invalid");
                return;
            }

            ParsingTable.Derive(token.Type, token.Lexeme, token.Location);

        } while (!token.Type.Equals(GrammarSymbols.END));

        if (!isProgramSyntacticallyCorrect || !ParsingTable.IsStackEmpty())
        {
            WriteSyntaxError(ParsingTable.TopOfStack(), false);
            syntaxErrorWriter?.WriteLine("Analysis concluded with syntax errors. See above.");
        }

        ASTGen.WriteAST();
        Sem.TraverseAST();

        derivationWriter?.Close();
        syntaxErrorWriter?.Close();
    }

    /// <summary>
    /// Writes derivation in outDerivation file
    /// </summary>
    /// <param name="derivation">String representing derivation</param>
    public static void WriteDerivation(string derivation)
    {
        derivationWriter?.WriteLine(derivation);
        derivationWriter?.WriteLine();
        derivationWriter?.Flush();
    }

    /// <summary>
    /// Obtain next valid token
    /// </summary>
    /// <param name="raiseNextTokenFlag">If function is called during a scan error, do not get next token on the next loop execution</param>
    /// <returns></returns>
    public static LexicalAnalyzer.Token? NextToken(bool raiseNextTokenFlag = false)
    {
        bool isTokenValid;

        do
        {
            isTokenValid = true;

            token = Lex.NextToken();

            if (token == null)
            {
                Debug.WriteLine("ERROR. Source file does not exist!");
                return null;
            }

            if (!GrammarSymbols.TERMINALS.Contains(token.Type))
            {
                isTokenValid = false;
            }

        } while (!isTokenValid);

        nextTokenFlag = raiseNextTokenFlag;
        return token;
    }

    /// <summary>
    /// Write syntax error in outSyntaxErrors file
    /// </summary>
    /// <param name="topOfStack">Top of stack in parsing table. Allows for more details about the error</param>
    /// <param name="includeToken">Include lexeme and location of current token</param>
    public static void WriteSyntaxError(string topOfStack, bool includeToken)
    {
        var message = "Syntax error. ";

        if (includeToken)
        {
            message += $"Unexpected symbol \"{token?.Lexeme}\" on line {token?.Location.Item1}, column {token?.Location.Item2}. ";
        }

        var details = GrammarSymbols.TERMINALS.Contains(topOfStack) ? $"Expected a token of type {topOfStack}" : $"{GrammarSymbols.nonTerminalToErrorDetails[topOfStack]}";
        message += details;

        syntaxErrorWriter?.WriteLine(message);
        syntaxErrorStream?.Flush();
    }

    /// <summary>
    /// Call when a syntax error is encountered. Signals that the current program is invalid.
    /// </summary>
    public static void InvalidProgram()
    {
        isProgramSyntacticallyCorrect = false;
    }
}
