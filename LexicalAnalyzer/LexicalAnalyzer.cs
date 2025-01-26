
namespace LexicalAnalyzer;

public class LexicalAnalyzer
{
    private static StreamReader? inFile = null;
    private static int? backedUpCharacter = null;
    private static (int, int) currentPositionInFile = (1, 1);
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
        if (inFile == null || inFile.EndOfStream)
        {
            inFile?.Close();
            tokenWriter?.Close();
            errorWriter?.Close();

            return null;
        }

        int character;
        var nextPosition = currentPositionInFile;

        do
        {
            currentPositionInFile = nextPosition;
            nextPosition = (currentPositionInFile.Item1, currentPositionInFile.Item2 + 1);

            if (backedUpCharacter == null)
            {
                character = inFile.Read();
            }

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

        if (token != null && token.Type != "SKIP")
        {
            tokenWriter?.Write(token.ToString());
        }

        return token;
    }

    public static void BackupCharacter(int character)
    {
        backedUpCharacter = character;
    }
}
