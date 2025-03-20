using System.Xml.Linq;

namespace ASTGenerator;

public abstract class AST
{
    private AST leftmostSibling;
    private AST? parent = null, rightSibling = null;
    protected AST? leftmostChild = null;
    protected string label;

    public AST? RightSibling => rightSibling;

    public AST(string label)
    {
        leftmostSibling = this;
        this.label = label;
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

        var child = leftmostChild;

        while (child != null)
        {
            result += "\n";
            result += child.GetString(indent + 1);
            child = child.RightSibling;
        }

        return result;
    }

    /// <summary>
    /// Create leaf node of given type
    /// </summary>
    /// <typeparam name="T">Type of the node to create</typeparam>
    /// <param name="value">Lexeme of the leaf node.</param>
    /// <returns></returns>
    public static T MakeNode<T>(string label = "epsilon") where T : AST, new()
    {
        return new T()
        {
            label = label.ToLower(),
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
        var node = (AST?)genericMethod?.Invoke(null, [parent.ToLower()]);

        if (node == null)
        {
            return new EpsilonNode("ERROR");
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
}

public class EpsilonNode(string label) : AST(label)
{
    public EpsilonNode() : this("epsilon") { }
}

public class IdNode(string label) : AST(label)
{
    public IdNode() : this("id") { }
}

public class VisibilityNode (string label) : AST(label)
{
    public VisibilityNode() : this("visibility") { }
}

public class TypeNode (string label) : AST(label)
{
    public TypeNode() : this("type") { }
}

public class ReturnTypeNode (string label) : AST(label)
{
    public ReturnTypeNode() : this("returntype") { }
}

public class IntLitNode (string label) : AST(label)
{
    public IntLitNode() : this("intlit") { }
}

public class FloatLitNode (string label) : AST(label)
{
    public FloatLitNode() : this("floatlit") { }
}

public class RelOpNode (string label) : AST(label)
{
    public RelOpNode() : this("relop") { }
}

public class AddOpNode (string label) : AST(label)
{
    public AddOpNode() : this("addop") { }
}

public class MultOpNode (string label) : AST(label)
{
    public MultOpNode() : this("multop") { }
}

public class SignNode (string label) : AST(label)
{
    public SignNode() : this("sign") { }
}

public class IdOrSelfNode (string label) : AST(label)
{
    public IdOrSelfNode() : this("idorself") { }
}

public class ProgramNode (string label) : AST(label)
{
    public ProgramNode() : this("program") { }
}

public class ParentsNode (string label) : AST(label)
{
    public ParentsNode() : this("parents") { }
}

public class ClassMembersNode (string label) : AST(label)
{
    public ClassMembersNode() : this("classmembers") { }
}

public class ClassMemberNode(string label) : AST(label)
{
    public ClassMemberNode() : this("classmember") { }
}

public class ClassDeclNode (string label) : AST(label)
{
    public ClassDeclNode() : this("classdecl") { }
}

public class FuncDefsNode(string label) : AST(label)
{
    public FuncDefsNode() : this("funcdefs") { }
}

public class ImplDefNode(string label) : AST(label)
{
    public ImplDefNode() : this("impldef") { }
}

public class FuncDefNode(string label) : AST(label)
{
    public FuncDefNode() : this("funcdef") { }
}

public class FParamsNode(string label) : AST(label)
{
    public FParamsNode() : this("fparams") { }
}

public class FuncHeadNode(string label) : AST(label)
{
    public FuncHeadNode() : this("funchead") { }
}

public class CParamsNode(string label) : AST(label)
{
    public CParamsNode() : this("cparams") { }
}

public class LocalVarDeclOrStatsNode(string label) : AST(label)
{
    public LocalVarDeclOrStatsNode() : this("localvardeclorstats") { }
}

public class ArraySizesNode(string label) : AST(label)
{
    public ArraySizesNode() : this("arraysizes") { }
}

public class VarDeclNode(string label) : AST(label)
{
    public VarDeclNode() : this("vardecl") { }
}

public class IfStatNode(string label) : AST(label)
{
    public IfStatNode() : this("ifstat") { }
}

public class ReadStatNode(string label) : AST(label)
{
    public ReadStatNode() : this("readstat") { }
}

public class ReturnStatNode(string label) : AST(label)
{
    public ReturnStatNode() : this("returnstat") { }
}

public class WhileStatNode(string label) : AST(label)
{
    public WhileStatNode() : this("whilestat") { }
}

public class WriteStatNode(string label) : AST(label)
{
    public WriteStatNode() : this("writestat") { }
}

public class StatementsNode(string label) : AST(label)
{
    public StatementsNode() : this("statements") { }
}

public class EmptyBlockNode(string label) : AST(label)
{
    public EmptyBlockNode() : this("emptyblock") { }
}

public class NotOpNode(string label) : AST(label)
{
    public NotOpNode() : this("notop") { }
}

public class ParamsOrIndicesNode(string label) : AST(label)
{
    public ParamsOrIndicesNode() : this("paramsorindices") { }
}

public class VarNode(string label) : AST(label)
{
    public VarNode() : this("var") { }
}

public class DotNode(string label) : AST(label)
{
    public DotNode() : this("dot") { }
}

public class FParamNode(string label) : AST(label)
{
    public FParamNode() : this("fparam") { }
}

public class AssignNode(string label) : AST(label)
{
    public AssignNode() : this("assign") { }
}

public class EmptyArraySizeNode(string label) : AST(label)
{
    public EmptyArraySizeNode() : this("emptyarraysize") { }
}