namespace ASTGenerator;

public abstract class AST
{
    private AST leftmostSibling;
    private AST? parent, rightSibling = null, leftmostChild;

    public AST()
    {
        leftmostSibling = this;
    }

    public static AST MakeNode<T>(string value) where T : LeafNode, new()
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

    public static AST MakeFamily(string parentName, List<AST> children)
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
}

public class IntLitNode : LeafNode;
public class FloatLitNode : LeafNode;
public class IdNode: LeafNode;
public class TypeNode : LeafNode;
public class VoidNode : LeafNode;

public class CompositeNode(string name) : AST
{
    private string name = name;
}