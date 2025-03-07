namespace ASTGenerator;

public class SemanticStack
{
    private static Stack<AST> semanticStack = new();

    public static void PerformSemanticAction(string symbol, string name)
    {
        AttributeGrammarSymbols.SemanticActions[symbol](semanticStack, name);
    }
}
