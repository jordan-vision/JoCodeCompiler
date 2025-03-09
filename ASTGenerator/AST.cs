namespace ASTGenerator;

public abstract class AST
{
    private AST leftmostSibling;
    private AST? parent = null, rightSibling = null;
    protected AST? leftmostChild = null;

    public AST? RightSibling => rightSibling;

    /// <summary>
    /// Default constructor
    /// </summary>
    public AST()
    {
        leftmostSibling = this;
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
    public abstract string GetString(int indent);

    /// <summary>
    /// Create leaf node of given type
    /// </summary>
    /// <typeparam name="T">Type of the node to create</typeparam>
    /// <param name="value">Lexeme of the leaf node.</param>
    /// <returns></returns>
    public static T MakeNode<T>(string value = "epsilon") where T : LeafNode, new()
    {
        return new T()
        {
            Lexeme = value,
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
    public static CompositeNode MakeFamily(string parentName, List<AST> children)
    {
        var node = new CompositeNode(parentName);

        foreach (var child in children)
        {
            if (child == children[0])
            {
                continue;
            }

            children[0].MakeSiblings(child);
        }

        node.AdoptChildren(children[0]);
        return node;
    }
}

public abstract class LeafNode : AST
{
    private string lexeme = "";

    public string Lexeme { get { return lexeme; } set { lexeme = value; } }

    /// <summary>
    /// Write indents followed by the lexeme of this node
    /// </summary>
    /// <param name="indent">Number of whitespaces to add before writing lexeme</param>
    /// <returns>Text representation of this leaf node.</returns>
    public override string GetString(int indent)
    {
        var result = "";

        for (int i = 0; i < indent; i++)
        {
            result += " ";
        }
        result += lexeme;

        return result;
    }
}

public class EpsilonNode : LeafNode;
public class IdNode : LeafNode;
public class VisibilityNode : LeafNode;
public class TypeNode : LeafNode;
public class ReturnTypeNode : LeafNode;
public class IntLitNode : LeafNode;
public class FloatLitNode : LeafNode;
public class RelOpNode : LeafNode;
public class AddOpNode : LeafNode;
public class MultOpNode : LeafNode;
public class SignNode : LeafNode;
public class IdOrSelfNode : LeafNode;

public class CompositeNode(string name) : AST
{
    private string name = name;

    /// <summary>
    /// Recursively write text representation of this AST node with all its children
    /// </summary>
    /// <param name="indent">Number of whitespaces to add before writing the name of this node</param>
    /// <returns>Text representation of this AST</returns>
    public override string GetString(int indent)
    {
        var result = "";

        for (int i = 0; i < indent; i++)
        {
            result += " ";
        }
        result += name;

        var child = leftmostChild;

        while (child != null)
        {
            result += "\n";
            result += child.GetString(indent + 1);
            child = child.RightSibling;
        }

        return result;
    }
}