
namespace LexicalAnalyzer;

public class TransitionTable
{
    private static int currentState = 1;
    private static Dictionary<(int, int), int> table = [];
    private static int[] ignoreTokensForStates = [1, 23, 24, 32];

    public static int CurrentState => currentState;

    /// <summary>
    /// Initializes transition table
    /// </summary>
    public static void BuildTransitionTable()
    {
        // From start
        table[(1, 1)] = 1;
        table[(1, 2)] = 1;
        table[(1, 3)] = 2;
        table[(1, 4)] = 3;
        table[(1, 5)] = 4;
        table[(1, 6)] = 5;
        table[(1, 7)] = 6;
        table[(1, 8)] = 7;
        table[(1, 9)] = 8;
        table[(1, 10)] = 9;
        table[(1, 11)] = 10;
        table[(1, 12)] = 11;
        table[(1, 13)] = 12;
        table[(1, 14)] = 13;
        table[(1, 15)] = 14;
        table[(1, 16)] = 15;
        table[(1, 17)] = 16;
        table[(1, 18)] = 17;
        table[(1, 19)] = 18;
        table[(1, 20)] = 18;
        table[(1, 21)] = 19;
        table[(1, 22)] = 20;
        table[(1, 23)] = 21;
        table[(1, 24)] = 22;

        // From /
        table[(9, 5)] = 23;
        table[(9, 10)] = 24;

        // From 0
        table[(10, 9)] = 25;

        // From 1..9
        table[(11, 9)] = 25;
        table[(11, 11)] = 11;
        table[(11, 12)] = 11;

        // From :
        table[(12, 16)] = 26;

        // From <
        table[(14, 16)] = 27;
        table[(14, 17)] = 28;

        // From =
        table[(15, 16)] = 29;
        table[(15, 17)] = 30;

        // From >
        table[(16, 16)] = 31;

        // From letter alphanum*
        table[(17, 11)] = 17;
        table[(17, 12)] = 17;
        table[(17, 18)] = 17;
        table[(17, 19)] = 17;
        table[(17, 22)] = 17;

        // From CMT
        table[(23, 1)] = 23;
        table[(23, 2)] = 23;
        table[(23, 3)] = 23;
        table[(23, 4)] = 23;
        table[(23, 5)] = 32;
        table[(23, 6)] = 23;
        table[(23, 7)] = 23;
        table[(23, 8)] = 23;
        table[(23, 9)] = 23;
        table[(23, 10)] = 23;
        table[(23, 11)] = 23;
        table[(23, 12)] = 23;
        table[(23, 13)] = 23;
        table[(23, 14)] = 23;
        table[(23, 15)] = 23;
        table[(23, 16)] = 23;
        table[(23, 17)] = 23;
        table[(23, 18)] = 23;
        table[(23, 19)] = 23;
        table[(23, 20)] = 23;
        table[(23, 21)] = 23;
        table[(23, 22)] = 23;
        table[(23, 23)] = 23;
        table[(23, 24)] = 23;

        // From INLINE
        table[(24, 1)] = 1;
        table[(24, 2)] = 24;
        table[(24, 3)] = 24;
        table[(24, 4)] = 24;
        table[(24, 5)] = 24;
        table[(24, 6)] = 24;
        table[(24, 7)] = 24;
        table[(24, 8)] = 24;
        table[(24, 9)] = 24;
        table[(24, 10)] = 24;
        table[(24, 11)] = 24;
        table[(24, 12)] = 24;
        table[(24, 13)] = 24;
        table[(24, 14)] = 24;
        table[(24, 15)] = 24;
        table[(24, 16)] = 24;
        table[(24, 17)] = 24;
        table[(24, 18)] = 24;
        table[(24, 19)] = 24;
        table[(24, 20)] = 24;
        table[(24, 21)] = 24;
        table[(24, 22)] = 24;
        table[(24, 23)] = 24;
        table[(24, 24)] = 24;

        // From integer.
        table[(25, 11)] = 33;
        table[(25, 12)] = 33;

        // From CMT_CLOSE
        table[(32, 1)] = 23;
        table[(32, 2)] = 23;
        table[(32, 3)] = 23;
        table[(32, 4)] = 23;
        table[(32, 5)] = 32;
        table[(32, 6)] = 23;
        table[(32, 7)] = 23;
        table[(32, 8)] = 23;
        table[(32, 9)] = 23;
        table[(32, 10)] = 1;
        table[(32, 11)] = 23;
        table[(32, 12)] = 23;
        table[(32, 13)] = 23;
        table[(32, 14)] = 23;
        table[(32, 15)] = 23;
        table[(32, 16)] = 23;
        table[(32, 17)] = 23;
        table[(32, 18)] = 23;
        table[(32, 19)] = 23;
        table[(32, 20)] = 23;
        table[(32, 21)] = 23;
        table[(32, 22)] = 23;
        table[(32, 23)] = 23;
        table[(32, 24)] = 23;

        // From integer fraction
        table[(33, 11)] = 34;
        table[(33, 12)] = 33;
        table[(33, 19)] = 35;

        // From integer fraction 0*
        table[(34, 11)] = 34;
        table[(34, 12)] = 33;

        // From integer fraction e
        table[(35, 6)] = 36;
        table[(35, 8)] = 36;
        table[(35, 11)] = 37;
        table[(35, 12)] = 38;

        // from integer fraction e(+|-)
        table[(36, 11)] = 37;
        table[(36, 12)] = 38;

        // from integer fraction e[+|-] integer
        table[(38, 11)] = 38;
        table[(38, 12)] = 38;
    }

    /// <summary>
    /// Transitions between states in the state machine using the transition table
    /// </summary>
    /// <param name="character">The symbol that is read</param>
    /// <returns>True if execution of state machine should be interrupted, True if a new token is available</returns>
    public static bool Transition(char character)
    {
        var state = currentState;
        int symbol;

        switch (character)
        {
            case '\n':
            case '\r':
                symbol = 1;
                break;

            case ' ':
            case '\t':
                symbol = 2;
                break;

            case '(':
                symbol = 3;
                break;

            case ')':
                symbol = 4;
                break;

            case '*':
                symbol = 5;
                break;

            case '+':
                symbol = 6;
                break;

            case ',':
                symbol = 7;
                break;

            case '-':
                symbol = 8;
                break;

            case '.':
                symbol = 9;
                break;

            case '/':
                symbol = 10;
                break;

            case '0':
                symbol = 11;
                break;

            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                symbol = 12;
                break;

            case ':':
                symbol = 13;
                break;

            case ';':
                symbol = 14;
                break;

            case '<':
                symbol = 15;
                break;

            case '=':
                symbol = 16;
                break;

            case '>':
                symbol = 17;
                break;

            case 'e':
                symbol = 19;
                break;

            case '[':
                symbol = 20;
                break;

            case ']':
                symbol = 21;
                break;

            case '_':
                symbol = 22;
                break;

            case '{':
                symbol = 23;
                break;

            case '}':
                symbol = 24;
                break;

            default:
                symbol = 0;
                break;
        }

        if (character >= 'A' && character <= 'Z'
            || character >= 'a' && character <= 'd'
            || character >= 'f' && character <= 'z')
        {
            symbol = 18;
        }

        // Invalid character
        if (symbol == 0)
        {  
            TokenManager.ResolveToken(currentState);
            currentState = 1;
            return true;
        }

        // Invalid transition
        if (!table.ContainsKey((state, symbol)))
        {
            // Back up chatacter
            LexicalAnalyzer.BackupCharacter(character);
            TokenManager.ResolveToken(currentState);
            currentState = 1;

            return true;
        }

        var previousState = state;
        currentState = table[(state, symbol)];

        // If start state or comment, ignore token
        if (ignoreTokensForStates.Contains(currentState))
        {
            return false;
        }

        // If going out of start state
        if (previousState == 1)
        {
            TokenManager.NewToken(LexicalAnalyzer.CurrentPositionInFile);
        }

        TokenManager.AddToLexeme(character);

        return false;
    }
}
