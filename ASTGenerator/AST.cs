namespace ASTGenerator;

public abstract class AST
{
    private AST leftmostSibling;
    private AST? parent = null, rightSibling = null;
    protected AST? leftmostChild = null;

    public AST? RightSibling => rightSibling;

    public AST()
    {
        leftmostSibling = this;
    }

    public override string ToString()
    {
        return GetString(0);
    }

    public abstract string GetString(int indent);

    public static T MakeNode<T>(string value = "epsilon") where T : LeafNode, new()
    {
        return new T()
        {
            Lexeme = value,
        };
    }

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