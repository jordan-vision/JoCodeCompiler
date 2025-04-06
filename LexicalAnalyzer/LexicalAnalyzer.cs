
namespace LexicalAnalyzer;

public class LexicalAnalyzer
{
    private static StreamReader? inFile = null;
    private static int? backedUpCharacter = null;
    private static (int, int) currentPositionInFile = (1, 1), nextPosition = (1, 1);
    private static FileStream? tokenStream, errorStream;
    private static StreamWriter? tokenWriter, errorWriter;

    public static (int, int) CurrentPositionInFile => currentPositionInFile;

    /// <summary>
    /// Sets and opens source file to start reading
    /// </summary>
    /// <param name="filename">Name of file to open</param>
    public static void OpenSourceFile(string filename)
    {
        inFile?.Close();
        tokenWriter?.Close();
        errorWriter?.Close();

        inFile = new StreamReader(filename);
        backedUpCharacter = null;
        currentPositionInFile = (1, 1);
        nextPosition = (1, 1);

        var outputDirectory = Path.GetDirectoryName(filename);

        if (outputDirectory == null)
        {
            Console.WriteLine("IO Error.");
            return;
        }

        var outLexTokensFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outLexTokens";
        var outLexErrorsFilename = $"{Path.GetFileNameWithoutExtension(filename)}.outLexErrors";

        tokenStream = File.Create(Path.Combine(outputDirectory, outLexTokensFilename));
        errorStream = File.Create(Path.Combine(outputDirectory, outLexErrorsFilename));

        tokenWriter = new(tokenStream);
        errorWriter = new(errorStream);
    }

    /// <summary>
    /// Get and write next token in out file
    /// </summary>
    /// <returns>The proper token data structure </returns>
    public static Token? NextToken()
    {
        if (inFile == null || (inFile.EndOfStream && backedUpCharacter == null))
        {
            inFile?.Close();
            tokenWriter?.Close();
            errorWriter?.Close();

            if (inFile == null) 
            {
                return null;
            }

            return new Token { Type = "$" };
        }

        int character;

        do
        {
            // Read from file if there is no backed up character
            if (backedUpCharacter == null)
            {
                character = inFile.Read();
                currentPositionInFile = nextPosition;
                nextPosition = (currentPositionInFile.Item1, currentPositionInFile.Item2 + 1);
            }

            // Otherwise use backed up character and consume it
            else
            {
                character = backedUpCharacter.Value;
                backedUpCharacter = null;
            }

            if (character == -1)
            {
                // End of file, resolve last token
                TokenManager.ResolveToken(TransitionTable.CurrentState);
                break;
            }

            // If new line, reset column number and add to line number
            if ((char)character == '\n')
            {
                nextPosition = (currentPositionInFile.Item1 + 1, 1);
                tokenWriter?.WriteLine();
            }

        } while (!TransitionTable.Transition((char)character));

        var token = TokenManager.GetCurrentToken();

        // Ignore token if it is SKIP type
        if (token != null && token.Type != "SKIP")
        {
            tokenWriter?.Write(token.ToString());
        }

        return token;
    }

    /// <summary>
    /// For the next iteration of NextToken, use this character instead of reading from file
    /// </summary>
    /// <param name="character">Character to back up</param>
    public static void BackupCharacter(int character)
    {
        backedUpCharacter = character;
    }

    /// <summary>
    /// Format and write lexical error in outlexerror file
    /// </summary>
    /// <param name="message">Reason for the error</param>
    /// <param name="lexeme">Problematic string</param>
    /// <param name="line">line of first character</param>
    /// <param name="col">column of first character</param>
    public static void WriteLexicalError(string message, string lexeme, int line, int col) 
    {
        errorWriter?.WriteLine($"Lexical error. {message}: \"{lexeme}\" line {line}, column {col}");
    }
}
