using System.Diagnostics;

namespace ASTGenerator;

public class AttributeGrammarSymbols
{
    private static Dictionary<string, Action<Stack<AST>, string>> semanticActions = [];

    public static Dictionary<string, Action<Stack<AST>, string>> SemanticActions => semanticActions;

    public static void BuildSemanticActionsDictionary()
    {
        semanticActions = new()
        {
            { "PUSHEPSILON", (s, n) => { s.Push(AST.MakeNode<EpsilonNode>(n)); } },

            { "PUSHID", (s, n) => { s.Push(AST.MakeNode<IdNode>(n)); } },

            { "PUSHVISIBILITY", (s, n) => { s.Push(AST.MakeNode<VisibilityNode>(n)); } },

            { "PUSHTYPE", (s, n) => { s.Push(AST.MakeNode<TypeNode>(n)); } },

            { "PUSHRETURNTYPE", (s, n) => { s.Push(AST.MakeNode<ReturnTypeNode>(n)); } },

            { "PUSHINTLIT", (s, n) => { s.Push(AST.MakeNode<IntLitNode>(n)); } },

            { "PUSHFLOATLIT", (s, n) => { s.Push(AST.MakeNode<FloatLitNode>(n)); } },

            { "PUSHRELOP", (s, n) => { s.Push(AST.MakeNode<RelOpNode>(n)); } },

            { "PUSHADDOP", (s, n) => { s.Push(AST.MakeNode<AddOpNode>(n)); } },

            { "PUSHMULTOP", (s, n) => { s.Push(AST.MakeNode<MultOpNode>(n)); } },

            { "PUSHSIGN", (s, n) => { s.Push(AST.MakeNode<SignNode>(n)); } },

            { "PUSHIDORSELF", (s, n) => { s.Push(AST.MakeNode<IdOrSelfNode>(n)); } },

            {
                "POP", (s, n) => { s.Push(AST.MakeFamily(n, [s.Pop()])); }
            },

            {
                "POP2", (s, n) =>
                {
                    List<AST> children = [s.Pop(), s.Pop()];
                    children.Reverse();
                    s.Push(AST.MakeFamily(n, children));
                }
            },

            {
                "POP3", (s, n) =>
                {
                    List<AST> children = [s.Pop(), s.Pop(), s.Pop()];
                    children.Reverse();
                    s.Push(AST.MakeFamily(n, children));
                }
            },

            {
                "POPUNTILEPSILON", (s, n) =>
                {
                    var children = new List<AST>();
                    AST? child = null;

                    while (child is not EpsilonNode)
                    {
                        child = s.Pop();
                        children.Add(child);
                    }

                    children.Reverse();
                    s.Push(AST.MakeFamily(n, children));
                }
            },
        };
    }
}
