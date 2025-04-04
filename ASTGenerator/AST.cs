using JoCodeTypes;

namespace ASTGenerator;

public abstract class AST
{
    private AST leftmostSibling;
    private AST? parent = null, rightSibling = null;
    private (int, int) positionInFile;

    protected AST? leftmostChild = null;
    protected string label, kind = "";
    protected IJoCodeType? type = null;
    protected ISymbolTable? symbolTable = null;

    public AST? Parent => parent;

    public (int, int) Position => positionInFile;

    public string Label => label;

    public IJoCodeType? Type { get { return type; } set { type = value; } }

    public string Kind { get { return kind; } set { kind = value; } }

    public ISymbolTable? SymbolTable { get { return symbolTable; } set { symbolTable = value; } }

    public AST(string label, (int, int) positionInFile)
    {
        leftmostSibling = this;
        this.label = label;
        this.positionInFile = positionInFile;
    }

    public override string ToString()
    {
        return GetString(0);
    }

    /// <summary>
    /// Recursively write text representation of this AST node with all its children
    /// </summary>
    /// <param name="indent">Number of whitespaces before writing. Recursively increases</param>
    /// <returns>Text representation of the AST</returns>
    public string GetString(int indent)
    {
        var result = "";

        for (int i = 0; i < indent; i++)
        {
            result += " ";
        }
        result += label;

        foreach (var child in GetChildren())
        {
            result += "\n";
            result += child.GetString(indent + 1);
        }

        return result;
    }

    /// <summary>
    /// Create leaf node of given type
    /// </summary>
    /// <typeparam name="T">Type of the node to create</typeparam>
    /// <param name="value">Lexeme of the leaf node.</param>
    /// <returns></returns>
    public static T MakeNode<T>(string label, (int, int) positionInFile) where T : AST, new()
    {
        return new T()
        {
            label = label.ToLower(),
            positionInFile = positionInFile,
        };
    }

    /// <summary>
    /// Make <paramref name="y"/> a sibling of this node.
    /// </summary>
    /// <param name="y">Other AST node</param>
    /// <returns></returns>
    private AST MakeSiblings(AST y)
    {
        var xSiblings = this;

        while(xSiblings.rightSibling != null)
        {
            xSiblings = xSiblings.rightSibling;
        }

        var ySiblings = y.leftmostSibling;
        xSiblings.rightSibling = ySiblings;
        ySiblings.leftmostSibling = xSiblings.leftmostSibling;
        ySiblings.parent = xSiblings.parent;

        while (ySiblings.rightSibling != null)
        {
            ySiblings = ySiblings.rightSibling;
            ySiblings.leftmostSibling = xSiblings.leftmostSibling;
            ySiblings.parent = xSiblings.parent;
        }

        return ySiblings;
    }

    /// <summary>
    /// Make <paramref name="y"/> a child of this node.
    /// </summary>
    /// <param name="y">Other AST node</param>
    private void AdoptChildren(AST y)
    {
        if (leftmostChild != null)
        {
            leftmostChild.MakeSiblings(y);
        } 
        else
        {
            var ySiblings = y.leftmostSibling;
            leftmostChild = ySiblings;
            while (ySiblings != null)
            {
                ySiblings.parent = this;
                ySiblings = ySiblings.rightSibling;
            }
        }
    }

    /// <summary>
    /// Create composite node
    /// </summary>
    /// <param name="parentName">Name of the composite node</param>
    /// <param name="children">List of all of the composite node's children</param>
    /// <returns></returns>
    public static AST MakeFamily(string parent, List<AST> children)
    {
        var method = typeof(AST).GetMethod("MakeNode");
        var genericMethod = method?.MakeGenericMethod([AttributeGrammarSymbols.NameToType[parent]]);
        var node = (AST?)genericMethod?.Invoke(null, [parent.ToLower(), children[0].Position]);

        if (node == null)
        {
            return new EpsilonNode("ERROR", (0, 0));
        }

        return MakeFamily(node, children);
    }

    public static AST MakeFamily(AST parent, List<AST> children)
    {
        foreach (var child in children)
        {
            if (child == children[0])
            {
                continue;
            }

            children[0].MakeSiblings(child);
        }

        parent.AdoptChildren(children[0]);
        return parent;
    }

    public List<AST> GetChildren()
    {
        var list = new List<AST>();

        var child = leftmostChild;
        while (child != null)
        {
            list.Add(child);
            child = child.rightSibling;
        }

        return list;
    }

    public AST GetRootNode()
    {
        var node = this;

        while (node.parent != null)
        {
            node = node.parent;
        }

        return node;
    }

    public ISymbolTable? FindSmallestScope()
    {
        AST? node = this;
        
        while (node != null && node.SymbolTable == null)
        {
            node = node.Parent;
        }

        return node?.SymbolTable;
    }

    public abstract void Accept(IVisitor visitor);
}

public class EpsilonNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public EpsilonNode() : this("epsilon", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class IdNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public IdNode() : this("id", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class VisibilityNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public VisibilityNode() : this("visibility", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class TypeNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public TypeNode() : this("type", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class ReturnTypeNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ReturnTypeNode() : this("returntype", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class IntLitNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public IntLitNode() : this("intlit", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class FloatLitNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FloatLitNode() : this("floatlit", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class RelOpNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public RelOpNode() : this("relop", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {

        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class AddOpNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public AddOpNode() : this("addop", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class MultOpNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public MultOpNode() : this("multop", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class SignNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public SignNode() : this("sign", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class IdOrSelfNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public IdOrSelfNode() : this("idorself", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ProgramNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ProgramNode() : this("program", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ParentsNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ParentsNode() : this("parents", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ClassMembersNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ClassMembersNode() : this("classmembers", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ClassMemberNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ClassMemberNode() : this("classmember", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ClassDeclNode (string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ClassDeclNode() : this("classdecl", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class FuncDefsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FuncDefsNode() : this("funcdefs", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ImplDefNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ImplDefNode() : this("impldef", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class FuncDefNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FuncDefNode() : this("funcdef", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class FParamsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FParamsNode() : this("fparams", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class FuncHeadNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FuncHeadNode() : this("funchead", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class CParamsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public CParamsNode() : this("cparams", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class LocalVarDeclOrStatsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public LocalVarDeclOrStatsNode() : this("localvardeclorstats", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ArraySizesNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ArraySizesNode() : this("arraysizes", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class VarDeclNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public VarDeclNode() : this("vardecl", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class IfStatNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public IfStatNode() : this("ifstat", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ReadStatNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ReadStatNode() : this("readstat", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ReturnStatNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ReturnStatNode() : this("returnstat", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class WhileStatNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public WhileStatNode() : this("whilestat", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class WriteStatNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public WriteStatNode() : this("writestat", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class StatementsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public StatementsNode() : this("statements", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class EmptyBlockNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public EmptyBlockNode() : this("emptyblock", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class NotOpNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public NotOpNode() : this("notop", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class ParamsNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public ParamsNode() : this("params", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class IndiceNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public IndiceNode() : this("indice", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class NoParamsOrIndicesNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public NoParamsOrIndicesNode() : this("noparamsorindices", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class VarNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public VarNode() : this("var", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }

    public void FindTypeInScope(AST scope)
    {
        var children = GetChildren();

        if (children[1] is ParamsNode paramsNode)
        {
            kind = "functioncall";

            var typePrefix = "";
            foreach (var param in paramsNode.GetChildren())
            {
                if (param is EpsilonNode)
                {
                    continue;
                }

                typePrefix += typePrefix == "" ? $"{param.Type}" : $", {param.Type}";
            }

            AST? currentNode = scope;

            while (currentNode != null)
            {
                if (currentNode.SymbolTable != null)
                {
                    var relevantEntries = currentNode.SymbolTable.GetEntriesWithName(children[0].Label);
                    relevantEntries = [.. relevantEntries.Where(e => e.Kind == "function" || e.Kind == "method" || e.Kind == "constructor")];
                    var mainEntry = relevantEntries.FirstOrDefault(e => e.Type is FunctionType f && f.ParameterTypes() == typePrefix);

                    if (mainEntry != default(Entry))
                    {
                        type = ((FunctionType)mainEntry.Type!).ReturnType;
                        return;
                    }
                }

                if (currentNode is ImplDefNode)
                {
                    currentNode = currentNode.SymbolTable?.GetEntry(currentNode.GetChildren()[0].Label, "class", null)?.Link?.ASTNode;
                }
                else
                {
                    currentNode = currentNode.Parent;
                }
            }
        }

        else
        {
            var indices = 0;

            if (children[1] is IndiceNode indice)
            {
                foreach (var expression in indice.GetChildren())
                {
                    if (expression is EpsilonNode)
                    {
                        continue;
                    }

                    indices++;
                }
            }

            AST? currentNode = scope;

            while (currentNode != null)
            {
                if (currentNode.SymbolTable != null)
                {
                    var relevantEntries = currentNode.SymbolTable.GetEntriesWithName(children[0].Label);
                    relevantEntries = [.. relevantEntries.Where(e => e.Kind == "localvar" || e.Kind == "parameter" || e.Kind == "attribute")];
                    var mainEntry = relevantEntries.FirstOrDefault();

                    if (mainEntry != default(Entry))
                    {
                        type = mainEntry.Type;
                        kind = mainEntry.Kind;
                        return;
                    }
                }

                if (currentNode is ImplDefNode)
                {
                    currentNode = currentNode.SymbolTable?.GetEntry(currentNode.GetChildren()[0].Label, "class", null)?.Link?.ASTNode;
                }
                else
                {
                    currentNode = currentNode.Parent;
                }
            }
        }
    }
}

public class DotNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public DotNode() : this("dot", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class FParamNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public FParamNode() : this("fparam", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class AssignNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public AssignNode() : this("assign", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}

public class EmptyArraySizeNode(string label, (int, int) positionInFile) : AST(label, positionInFile)
{
    public EmptyArraySizeNode() : this("emptyarraysize", (0, 0)) { }

    public override void Accept(IVisitor visitor)
    {
        foreach (var child in GetChildren())
        {
            child.Accept(visitor);
        }

        visitor.Visit(this);
    }
}