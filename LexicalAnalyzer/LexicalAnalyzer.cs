
namespace LexicalAnalyzer;

public class LexicalAnalyzer
{
    private static StreamReader? file = null;
    private static int? backedUpCharacter = null; 

    /// <summary>
    /// Sets and opens source file to start reading
    /// </summary>
    /// <param name="filename">Name of file to open</param>
    public static void OpenFile(string filename)
    {
        file?.Close();
        file = new StreamReader(filename);
    }

    /// <summary>
    /// Get and write next token in out file
    /// </summary>
    /// <returns>The proper token data structure </returns>
    public static Token? NextToken()
    {
        if (file == null || file.EndOfStream)
        {
            return null;
        }

        int character;
        do
        {
            if (backedUpCharacter == null)
            {
                character = file.Read();
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

        } while (!TransitionTable.Transition((char)character));

        return TokenManager.GetCurrentToken();
    }

    public static void BackupCharacter(int character)
    {
        backedUpCharacter = character;
    }
}
