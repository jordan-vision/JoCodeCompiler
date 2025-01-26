
namespace LexicalAnalyzer;

public class TokenManager
{
    private static Token? currentToken = null;
    private static string[] reservedWords = ["or", "and", "not", "int", "float", "void", "class", "self", "isa", "implementation", "while", "if", "then", "else", "read", "write", "return", "local", "constructor", "attribute", "function", "public", "private"];

    public static void NewToken((int, int) position)
    {
        currentToken = new Token()
        {
            Location = position,
        };
    }

    public static void AddToLexeme(char character)
    {
        if (currentToken != null)
        {
            currentToken.Lexeme += character;
        }
    }

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
                currentToken.Type = "ZERO";
                break;

            case 11:
                currentToken.Type = "INT";
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
                currentToken.Type = "EQUALS";
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
                currentToken.Type = "INV_ID";
                break;

            case 21:
                currentToken.Type = "LCUB";
                break;

            case 22:
                currentToken.Type = "RCUB";
                break;

            case 25:
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
                currentToken.Type = "ASSIGN";
                break;

            case 30:
                currentToken.Type = "LAMBDA";
                break;

            case 31:
                currentToken.Type = "GEQ";
                break;

            case 33:
                currentToken.Type = "FLOAT_1";
                break;

            case 34:
                currentToken.Type = "INV_FLOAT_2";
                break;

            case 35:
                currentToken.Type = "INV_FLOAT_3";
                break;

            case 36:
                currentToken.Type = "INV_FLOAT_4";
                break;

            case 37:
                currentToken.Type = "FLOAT_2";
                break;

            case 38:
                currentToken.Type = "FLOAT_3";
                break;

            default:
                break;
        }
    }

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
