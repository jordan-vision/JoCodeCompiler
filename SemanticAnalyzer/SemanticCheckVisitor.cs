using ASTGenerator;
using JoCodeTypes;

namespace SemanticAnalyzer;

class SemanticCheckVisitor : IVisitor
{
    public void Visit(EpsilonNode node)
    {
        return;
    }

    public void Visit(IdNode node)
    {
        return;
    }

    public void Visit(VisibilityNode node)
    {
        return;
    }

    public void Visit(TypeNode node)
    {
        return;
    }

    public void Visit(ReturnTypeNode node)
    {
        return;
    }

    public void Visit(IntLitNode node)
    {
        node.Type = BaseType.Int;
        node.Kind = "constant";
    }

    public void Visit(FloatLitNode node)
    {
        node.Type = BaseType.Float;
        node.Kind = "constant";
    }

    public void Visit(RelOpNode node)
    {
        node.Type = BaseType.Bool;
        node.Kind = "operator";
    }

    public void Visit(AddOpNode node)
    {
        var operands = node.GetChildren();

        if (operands[0].Type == null || operands[1].Type == null)
        {
            return;
        }

        if (operands[0].Type!.Label != operands[1].Type!.Label)
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on types {operands[0].Type} and {operands[1].Type}.", node.Position);
        }
        else if (operands[0].Type!.Label == "int" || operands[0].Type!.Label == "float" || operands[0].Type!.Label == "bool")
        {
            node.Type = operands[0].Type;
            node.Kind = "operator";
        } 
        else
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on type {operands[0].Type}.", node.Position);
        }
    }

    public void Visit(MultOpNode node)
    {
        var operands = node.GetChildren();

        if (operands[0].Type == null || operands[1].Type == null)
        {
            return;
        }

        if (operands[0].Type!.Label != operands[1].Type!.Label)
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on types {operands[0].Type} and {operands[1].Type}.", node.Position);
        }
        else if (operands[0].Type!.Label == "int" || operands[0].Type!.Label == "float" || operands[0].Type!.Label == "bool")
        {
            node.Type = operands[0].Type;
            node.Kind = "operator";
        }
        else
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on type {operands[0].Type}.", node.Position);
        }
    }

    public void Visit(SignNode node)
    {
        var operand = node.GetChildren()[0];

        if (operand.Type == null)
        {
            return;
        }

        if (operand.Type.Label == "int" || operand.Type.Label == "float")
        {
            node.Type = operand.Type;
            node.Kind = operand.Kind;
        } 
        else if (operand.Type != null)
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on type {operand.Type.Label}", node.Position);
        }
    }

    public void Visit(IdOrSelfNode node)
    {
        if (node.Label != "self")
        {
            return;
        }

        AST? currentNode = node;

        while (currentNode is not ImplDefNode && currentNode is not ClassDeclNode)
        {
            if (currentNode == null)
            {
                SemanticAnalyzer.WriteSemanticError($"Symbol self may only be used in a class declaration or implementation.", node.Position);
                return;
            }

            currentNode = currentNode.Parent;
        }

        if (currentNode.SymbolTable == null)
        {
            return;
        }

        node.Type = BaseType.GetBaseType(currentNode.SymbolTable.Name);
        node.Kind = "class";
    }

    public void Visit(ProgramNode node)
    {
        return;
    }

    public void Visit(ParentsNode node)
    {
        return;
    }

    public void Visit(ClassMembersNode node)
    {
        return;
    }

    public void Visit(ClassMemberNode node)
    {
        return;
    }

    public void Visit(ClassDeclNode node)
    {
        return;
    }

    public void Visit(FuncDefsNode node)
    {
        return;
    }

    public void Visit(ImplDefNode node)
    {
        return;
    }

    public void Visit(FuncDefNode node)
    {
        return;
    }

    public void Visit(FParamsNode node)
    {
        return;
    }

    public void Visit(FuncHeadNode node)
    {
        return;
    }

    public void Visit(CParamsNode node)
    {
        return;
    }

    public void Visit(LocalVarDeclOrStatsNode node)
    {
        return;
    }

    public void Visit(ArraySizesNode node)
    {
        return;
    }

    public void Visit(VarDeclNode node)
    {
        return;
    }

    public void Visit(IfStatNode node)
    {
        return;
    }

    public void Visit(ReadStatNode node)
    {
        var variable = node.GetChildren()[0];

        if (variable.Kind != "localvar" && variable.Kind != "parameter")
        {
            SemanticAnalyzer.WriteSemanticError("Must read into a variable.", node.Position);
        }
    }

    public void Visit(ReturnStatNode node)
    {
        AST? currentNode = node;

        while (currentNode is not FuncDefNode)
        {
            if (currentNode == null)
            {
                SemanticAnalyzer.WriteSemanticError("return statement may only be used within a function body.", node.Position);
                return;
            }

            currentNode = currentNode.Parent;
        }

        if (currentNode.SymbolTable == null || currentNode.Parent == null)
        {
            return;
        }

        var parentNode = currentNode.Parent;

        while (parentNode?.SymbolTable == null)
        {
            if (parentNode == null)
            {
                return;
            }

            parentNode = parentNode.Parent;
        }

        var funcEntry = parentNode.SymbolTable.GetEntryWithLink(currentNode.SymbolTable);
        var funcType = (FunctionType?)(funcEntry?.Type);
        var funcReturnType = funcType?.ReturnType;
        var thisReturnType = node.GetChildren()[0].Type;

        if (funcReturnType == null || thisReturnType == null)
        {
            return;
        }

        if (funcReturnType.Label != thisReturnType.Label)
        {
            SemanticAnalyzer.WriteSemanticError($"Function should return a value of type {funcReturnType} but instead returns a value of type {thisReturnType}.", node.Position);
        }
    }

    public void Visit(WhileStatNode node)
    {
        return;
    }

    public void Visit(WriteStatNode node)
    {
        return;
    }

    public void Visit(StatementsNode node)
    {
        return;
    }

    public void Visit(EmptyBlockNode node)
    {
        return;
    }

    public void Visit(NotOpNode node)
    {
        var operand = node.GetChildren()[0];

        if (operand.Type == null)
        {
            return;
        }
        if (operand.Type.Label == "bool")
        {
            node.Type = operand.Type;
            node.Kind = operand.Kind;
        }
        else
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot use {node.Label} operator on type {operand.Type}", node.Position);
        }
    }

    public void Visit(ParamsNode node)
    {
        return;
    }

    public void Visit(IndiceNode node)
    {
        return;
    }

    public void Visit(NoParamsOrIndicesNode node)
    {
        return;
    }

    public void Visit(VarNode node)
    {
        // TODO get node type

        if (node.Type != null && node.Kind != "")
        {
            return;
        }
        
        var children = node.GetChildren();

        if (children[0].Kind == "class")
        {
            node.Type = children[0].Type;
            node.Kind = children[0].Kind;
        }

        else if (node.Parent is DotNode && node == node.Parent.GetChildren()[1])
        {
            return;
        }

        if (children[1] is IndiceNode paramsNode)
        {
            foreach (var expression in paramsNode.GetChildren())
            {
                if (expression.Type == null)
                {
                    continue;
                }

                if (expression is not EpsilonNode && expression.Type.Label != "int")
                {
                    SemanticAnalyzer.WriteSemanticError("Indice may only be of type int.", expression.Position);
                }
            }
        }

        node.FindTypeInScope(node);

        if (node.Type == null)
        {
            if (children[1] is ParamsNode)
            {
                SemanticAnalyzer.WriteSemanticError($"No function {children[0].Label} with given parameters was found.", node.Position);
            }

            else
            {
                SemanticAnalyzer.WriteSemanticError($"No identifier {children[0].Label} was found.", node.Position);
            }
        }
    }

    public void Visit(DotNode node)
    {
        var operands = node.GetChildren();
        var rootTable = node.GetRootNode().SymbolTable;

        if (rootTable == null)
        {
            return;
        }

        if (operands[0].Type == null)
        {
            return;
        }

        if (!rootTable.DoesEntryExist(operands[0].Type!.Label, "class"))
        {
            SemanticAnalyzer.WriteSemanticError($"Identifier {operands[0].GetChildren()[0].Label} does not represent a class object.", operands[0].Position);
        }

        var entry = rootTable.GetEntry(operands[0].Type!.Label, "class", null);
        var scope = entry?.Link?.ASTNode;

        if (scope == null)
        {
            return;
        }

        VarNode varNode = operands[1] is VarNode varNode1 ? varNode1 : (VarNode)operands[1].GetChildren()[0];
        var children = varNode.GetChildren();

        if (children[1] is IndiceNode paramsNode)
        {
            foreach (var expression in paramsNode.GetChildren())
            {
                if (expression.Type?.Label != "int")
                {
                    SemanticAnalyzer.WriteSemanticError("Indice may only be of type int.", expression.Position);
                }
            }
        }

        varNode.FindTypeInScope(scope);
        node.Type = operands[1].Type;

        if (varNode.Type == null)
        {
            if (children[1] is ParamsNode)
            {
                SemanticAnalyzer.WriteSemanticError($"No function {children[0].Label} with given parameters was found.", node.Position);
            }

            else
            {
                SemanticAnalyzer.WriteSemanticError($"No identifier {children[0].Label} was found.", node.Position);
            }
        }
    }

    public void Visit(FParamNode node)
    {
        return;
    }

    public void Visit(AssignNode node)
    {
        var operands = node.GetChildren();

        if (operands[0].Type == null || operands[1].Type == null)
        {
            return;
        }

        if (operands[0].Kind == "functioncall")
        {
            SemanticAnalyzer.WriteSemanticError("Cannot assign value to a fucntion call.", node.Position);
        }

        if (operands[0].Type != operands[1].Type)
        {
            SemanticAnalyzer.WriteSemanticError($"Cannot assign value of type {operands[1].Type} to a variable of type {operands[0].Type}.", node.Position);
        }
    }

    public void Visit(EmptyArraySizeNode node)
    {
        return;
    }
}
