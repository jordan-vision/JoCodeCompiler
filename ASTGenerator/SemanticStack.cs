namespace ASTGenerator;

public class SemanticStack
{
    private static Stack<AST> semanticStack = new();

    public static bool PerformSemanticAction(string symbol, string lexeme)
    {
        if (AttributeGrammarSymbols.SemanticActions.ContainsKey(symbol))
        {
            AttributeGrammarSymbols.SemanticActions[symbol](semanticStack, lexeme);
            return true;
        }

        if (symbol.StartsWith("POP"))
        {
            var (prefix, suffix) = SeparateSymbol(symbol);
            AttributeGrammarSymbols.SemanticActions[prefix](semanticStack, suffix.ToLower());
            return true;
        }

        return false;
    }

    public static (string, string) SeparateSymbol(string symbol)
    {
        if (symbol.StartsWith("POP2") || symbol.StartsWith("POP3"))
        {
            return (symbol[..4], symbol.Length > 4 ? symbol[4..] : "");
        }

        if (symbol.StartsWith("POPUNTILEPSILON"))
        {
            return (symbol[..15], symbol.Length > 15 ? symbol[15..] : "");
        }

        return (symbol[..3], symbol.Length > 3 ? symbol[3..] : "");
    }
}
