using System.Diagnostics;

namespace ASTGenerator;

public class AttributeGrammarSymbols
{
    private static Dictionary<string, Action<Stack<AST>, string>> semanticActions = [];

    public static void BuildSemanticActionsDictionary()
    {
        semanticActions = new()
        {
            { "A", (s, n) => { s.Push(AST.MakeNode<IdNode>(n)); } }
        };
    }
}
