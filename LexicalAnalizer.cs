
namespace JoCodeCompiler;

public class LexicalAnalizer
{
    private static StreamReader? file = null;

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
        if (file == null)
        {
            return null;
        }

        var character = file.Read();
        if (character == -1)
        {
            file.Close();
            return null;
        }

        return new Token();
    }
}
