namespace ASTGenerator;

public class AttributeGrammarSymbols
{
    private static Dictionary<string, Action<Stack<AST>, string>> semanticActions = [];
    private static Dictionary<string, Type> nameToType = [];

    public static Dictionary<string, Action<Stack<AST>, string>> SemanticActions => semanticActions;
    public static Dictionary<string, Type> NameToType => nameToType;

    /// <summary>
    /// Define all possible semantic actions
    /// </summary>
    public static void BuildSemanticActionsDictionary()
    {
        semanticActions = new()
        {
            { "PUSHEPSILON", (s, n) => { s.Push(AST.MakeNode<EpsilonNode>()); } },

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
                "POPUNARY", (s, n) => 
                {
                    var child = s.Pop();
                    s.Push(AST.MakeFamily(s.Pop(), [child]));
                } 
            },

            { 
                "POPBINARY", (s, n) => 
                {
                    var child2 = s.Pop();
                    var parent = s.Pop();
                    var child1 = s.Pop();

                    s.Push(AST.MakeFamily(parent, [child1, child2]));
                } 
            },

            { 
                "POP", (s, n) => 
                {
                    s.Push(AST.MakeFamily(n, [s.Pop()])); 
                } 
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

        nameToType = new()
        {
            { "PROGRAM", typeof(ProgramNode) },

            { "PARENTS", typeof(ParentsNode) },

            { "CLASSMEMBERS", typeof(ClassMembersNode) },

            { "CLASSMEMBER", typeof(ClassMemberNode) },

            { "CLASSDECL", typeof(ClassDeclNode) },

            { "FUNCDEFS", typeof(FuncDefsNode) },

            { "IMPLDEF", typeof(ImplDefNode) },

            { "FUNCDEF", typeof(FuncDefNode) },

            { "FPARAMS", typeof(FParamsNode) },

            { "FUNCHEAD", typeof(FuncHeadNode) },

            { "CPARAMS", typeof(CParamsNode) },

            { "LOCALVARDECLORSTATS", typeof(LocalVarDeclOrStatsNode) },

            { "ARRAYSIZES", typeof(ArraySizesNode) },

            { "VARDECL", typeof(VarDeclNode) },

            { "IFSTAT", typeof(IfStatNode) },

            { "READSTAT", typeof(ReadStatNode) },

            { "RETURNSTAT", typeof(ReturnStatNode) },

            { "WHILESTAT", typeof(WhileStatNode) },

            { "WRITESTAT", typeof(WriteStatNode) },

            { "STATEMENTS", typeof(StatementsNode) },

            { "EMPTYBLOCK", typeof(EmptyBlockNode) },

            { "NOTOP", typeof(NotOpNode) },

            { "PARAMSORINDICES", typeof(ParamsOrIndicesNode) },

            { "VAR", typeof(VarNode) },

            { "DOT", typeof(DotNode) },

            { "FPARAM", typeof(FParamNode) },

            { "ASSIGN", typeof(AssignNode) },

            { "EMPTYARRAYSIZE", typeof(EmptyArraySizeNode) },
        };
    }
}
