
namespace LexicalAnalyzer;

public class TokenManager
{
    private static Token? currentToken = null;
    private static string[] reservedWords = ["or", "and", "not", "int", "float", "void", "class", "self", "isa", "implementation", "while", "if", "then", "else", "read", "write", "return", "local", "constructor", "attribute", "function", "public", "private"];

    /// <summary>
    /// Create token with given location
    /// </summary>
    /// <param name="position">Coordinates of the first character of the token</param>
    public static void NewToken((int, int) position)
    {
        currentToken = new Token()
        {
            Location = position,
        };
    }

    /// <summary>
    /// Add character to current token's lexeme
    /// </summary>
    /// <param name="character">Character to add</param>
    public static void AddToLexeme(char character)
    {
        if (currentToken != null)
        {
            currentToken.Lexeme += character;
        }
    }

    /// <summary>
    /// Determines current token's type based on current state
    /// </summary>
    /// <param name="currentState">State of the state machine in the transition table</param>
    public static void ResolveToken(int currentState)
    {
        if (currentToken == null)
        {
            return;
        }

        switch (currentState)
        {
            case 2:
                currentToken.Type = "LPAREN";
                break;

            case 3:
                currentToken.Type = "RPAREN";
                break;

            case 4:
                currentToken.Type = "AST";
                break;

            case 5:
                currentToken.Type = "PLUS";
                break;

            case 6:
                currentToken.Type = "COMMA";
                break;

            case 7:
                currentToken.Type = "MINUS";
                break;

            case 8:
                currentToken.Type = "PERIOD";
                break;

            case 9:
                currentToken.Type = "SOL";
                break;

            case 10:
                currentToken.Type = "ZERO_VAL";
                break;

            case 11:
                currentToken.Type = "INT_VAL";
                break;

            case 12:
                currentToken.Type = "COLON";
                break;

            case 13:
                currentToken.Type = "SEMI";
                break;

            case 14:
                currentToken.Type = "LT";
                break;

            case 15:
                currentToken.Type = "ASSIGN";
                break;

            case 16:
                currentToken.Type = "RT";
                break;

            case 17:
                if (reservedWords.Contains(currentToken.Lexeme.ToLower()))
                {
                    currentToken.Type = currentToken.Lexeme.ToUpper();
                } else
                {
                    currentToken.Type = "ID";
                }
                break;

            case 18:
                currentToken.Type = "LSQB";
                break;

            case 19:
                currentToken.Type = "RSQB";
                break;

            case 20:
                LexicalAnalyzer.WriteLexicalError("Invalid identifier", currentToken.Lexeme, currentToken.Location.Item1, currentToken.Location.Item2);
                currentToken.Type = "INV_ID";
                break;

            case 21:
                currentToken.Type = "LCUB";
                break;

            case 22:
                currentToken.Type = "RCUB";
                break;

            case 25:
                LexicalAnalyzer.WriteLexicalError("Invalid number. Include at least one digit after the floating point", currentToken.Lexeme, currentToken.Location.Item1, currentToken.Location.Item2);
                currentToken.Type = "INV_FLOAT_1";
                break;

            case 26:
                currentToken.Type = "EQUIV";
                break;

            case 27:
                currentToken.Type = "LEQ";
                break;

            case 28:
                currentToken.Type = "TRI";
                break;

            case 29:
                currentToken.Type = "EQUALS";
                break;

            case 30:
                currentToken.Type = "LAMBDA";
                break;

            case 31:
                currentToken.Type = "GEQ";
                break;

            case 33:
            case 37:
            case 38:
                currentToken.Type = "FLOAT_VAL";
                break;

            case 34:
                LexicalAnalyzer.WriteLexicalError("Invalid number. Float may not end with a zero", currentToken.Lexeme, currentToken.Location.Item1, currentToken.Location.Item2);
                currentToken.Type = "INV_FLOAT_2";
                break;

            case 35:
                LexicalAnalyzer.WriteLexicalError("Invalid number. No value after e", currentToken.Lexeme, currentToken.Location.Item1, currentToken.Location.Item2);
                currentToken.Type = "INV_FLOAT_3";
                break;

            case 36:
                LexicalAnalyzer.WriteLexicalError("Invalid number. No value after e", currentToken.Lexeme, currentToken.Location.Item1, currentToken.Location.Item2);
                currentToken.Type = "INV_FLOAT_4";
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Accessor for current token. Consumes token
    /// </summary>
    /// <returns></returns>
    public static Token? GetCurrentToken()
    {
        if (currentToken == null)
        {
            return new Token()
            {
                Type = "SKIP",
            };
        }

        var returnToken = currentToken;
        currentToken = null;
        return returnToken;
    }
}
