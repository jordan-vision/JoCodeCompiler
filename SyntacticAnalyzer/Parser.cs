using System.Diagnostics;
using Lex = LexicalAnalyzer.LexicalAnalyzer;

namespace SyntacticAnalyzer;

public class Parser
{
    /// <summary>
    /// Opens file in lexical analyzer
    /// </summary>
    /// <param name="filename">Name of file to open</param>
    public static void OpenSourceFile(string filename)
    {
        Lex.OpenSourceFile(filename);
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
                    Debug.WriteLine("end of file reached");
                    return;
                }

                if (!GrammarSymbols.TERMINALS.Contains(token.Type))
                {
                    isTokenValid = false;
                }

            } while (!isTokenValid);

            ParsingTable.Derive(token.Type);

        } while (token != null);
    }
}
