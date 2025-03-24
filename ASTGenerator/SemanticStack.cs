namespace ASTGenerator;

public class SemanticStack
{
    private static Stack<AST> semanticStack = new();

    /// <summary>
    /// Performs semantic action described by given symbol.
    /// </summary>
    /// <param name="symbol">Semantic action</param>
    /// <param name="lexeme">If this is a PUSH action, the resulting leaf node should save this lexeme</param>
    /// <returns>True if the semantic action was succesful, false otherwise</returns>
    public static bool PerformSemanticAction(string symbol, string lexeme, (int, int) position)
    {
        if (AttributeGrammarSymbols.SemanticActions.TryGetValue(symbol, out var value))
        {
            value(semanticStack, lexeme, position);
            return true;
        }

        if (symbol.StartsWith("POP"))
        {
            var (prefix, suffix) = SeparateSymbol(symbol);
            AttributeGrammarSymbols.SemanticActions[prefix](semanticStack, suffix, position);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets prefix and suffix for POP symbol
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns>(prefix of symbol, suffix of symbol)</returns>
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

    public static void ResetStack()
    {
        semanticStack = new();
    }

    /// <summary>
    /// Write AST in outast file.
    /// </summary>
    /// <returns>Text representation of top of semantic stack</returns>
    public static string WriteTree()
    {
        return semanticStack.Peek().ToString();
    }

    public static void Traverse(IVisitor visitor)
    {
        semanticStack.Peek().Accept(visitor);
    }

    public static string? WriteSymbolTable()
    {
        return semanticStack.Peek().SymbolTable?.ToString();
    }
}
