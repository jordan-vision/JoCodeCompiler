using System.Diagnostics;

namespace ASTGenerator;

public class AttributeGrammarSymbols
{
    private static Dictionary<string, Action<Stack<AST>, string>> semanticActions = [];

    public static void BuildSemanticActionsDictionary()
    {
        semanticActions = new()
        {
            { "PUSHEPSILON", (s, n) => { s.Push(AST.MakeNode<EpsilonNode>(n)); } },

            { 
                "POPUNTILEPSILON", (s, n) => 
                {
                    var children = new List<AST>();
                    AST child = s.Pop();

                    while (child is not EpsilonNode)
                    {
                        children.Add(child);
                        child = s.Pop();
                    } 

                    s.Push(AST.MakeFamily(n, children));
                }
            },
        };
    }
}
