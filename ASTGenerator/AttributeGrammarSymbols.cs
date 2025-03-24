namespace ASTGenerator;

public class AttributeGrammarSymbols
{
    private static Dictionary<string, Action<Stack<AST>, string, (int, int)>> semanticActions = [];
    private static Dictionary<string, Type> nameToType = [];

    public static Dictionary<string, Action<Stack<AST>, string, (int, int)>> SemanticActions => semanticActions;
    public static Dictionary<string, Type> NameToType => nameToType;

    /// <summary>
    /// Define all possible semantic actions
    /// </summary>
    public static void BuildSemanticActionsDictionary()
    {
        semanticActions = new()
        {
            { "PUSHEPSILON", (s, n, p) => { s.Push(AST.MakeNode<EpsilonNode>(n, p)); } },

            { "PUSHID", (s, n, p) => { s.Push(AST.MakeNode<IdNode>(n, p)); } },

            { "PUSHVISIBILITY", (s, n, p) => { s.Push(AST.MakeNode<VisibilityNode>(n, p)); } },

            { "PUSHTYPE", (s, n, p) => { s.Push(AST.MakeNode < TypeNode >(n, p)); } },

            { "PUSHRETURNTYPE", (s, n, p) => { s.Push(AST.MakeNode < ReturnTypeNode >(n, p)); } },

            { "PUSHINTLIT", (s, n, p) => { s.Push(AST.MakeNode < IntLitNode >(n, p)); } },

            { "PUSHFLOATLIT", (s, n, p) => { s.Push(AST.MakeNode < FloatLitNode >(n, p)); } },

            { "PUSHRELOP", (s, n, p) => { s.Push(AST.MakeNode < RelOpNode >(n, p)); } },

            { "PUSHADDOP", (s, n, p) => { s.Push(AST.MakeNode < AddOpNode >(n, p)); } },

            { "PUSHMULTOP", (s, n, p) => { s.Push(AST.MakeNode < MultOpNode >(n, p)); } },

            { "PUSHSIGN", (s, n, p) => { s.Push(AST.MakeNode < SignNode >(n, p)); } },

            { "PUSHIDORSELF", (s, n, p) => { s.Push(AST.MakeNode < IdOrSelfNode >(n, p)); } },

            { 
                "POPUNARY", (s, n, p) => 
                {
                    var child = s.Pop();
                    s.Push(AST.MakeFamily(s.Pop(), [child]));
                } 
            },

            { 
                "POPBINARY", (s, n, p) => 
                {
                    var child2 = s.Pop();
                    var parent = s.Pop();
                    var child1 = s.Pop();

                    s.Push(AST.MakeFamily(parent, [child1, child2]));
                } 
            },

            { 
                "POP", (s, n, p) => 
                {
                    s.Push(AST.MakeFamily(n, [s.Pop()])); 
                } 
            },

            {
                "POP2", (s, n, p) =>
                {
                    List<AST> children = [s.Pop(), s.Pop()];
                    children.Reverse();
                    s.Push(AST.MakeFamily(n, children));
                }
            },

            {
                "POP3", (s, n, p) =>
                {
                    List<AST> children = [s.Pop(), s.Pop(), s.Pop()];
                    children.Reverse();
                    s.Push(AST.MakeFamily(n, children));
                }
            },

            {
                "POPUNTILEPSILON", (s, n, p) =>
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

            { "PARAMS", typeof(ParamsNode) },

            { "INDICE", typeof(IndiceNode) },

            { "NOPARAMSORINDICES", typeof(NoParamsOrIndicesNode) },

            { "VAR", typeof(VarNode) },

            { "DOT", typeof(DotNode) },

            { "FPARAM", typeof(FParamNode) },

            { "ASSIGN", typeof(AssignNode) },

            { "EMPTYARRAYSIZE", typeof(EmptyArraySizeNode) },
        };
    }
}
