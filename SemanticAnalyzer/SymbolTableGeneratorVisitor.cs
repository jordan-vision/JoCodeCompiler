using ASTGenerator;

namespace SemanticAnalyzer;

public class SymbolTableGeneratorVisitor : IVisitor
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
        return;
    }

    public void Visit(FloatLitNode node)
    {
        return;
    }

    public void Visit(RelOpNode node)
    {
        return;
    }

    public void Visit(AddOpNode node)
    {
        return;
    }

    public void Visit(MultOpNode node)
    {
        return;
    }

    public void Visit(SignNode node)
    {
        return;
    }

    public void Visit(IdOrSelfNode node)
    {
        return;
    }

    public void Visit(ProgramNode node)
    {
        node.SymbolTable = new SymbolTable("global");

        foreach (var child in node.GetChildren())
        {
            if (child is ClassDeclNode)
            {
                if (child.SymbolTable == null)
                {
                    continue;
                }

                node.SymbolTable.AddEntry(child.SymbolTable.Name, "class", "", child.SymbolTable);
            }

            if (child is FuncDefNode)
            {
                if (child.SymbolTable == null)
                {
                    continue;
                }

                var funcHead = child.LeftmostChild;

                if (funcHead is not FuncHeadNode)
                {
                    continue;
                }

                var parameters = funcHead.GetChildren()[1];
                string parameterTypeList = "";

                foreach (var parameter in parameters.GetChildren())
                {
                    if (parameter is not FParamNode)
                    {
                        continue;
                    }

                    var parameterDefinition = parameter.GetChildren();
                    parameterTypeList += parameterTypeList == "" ? parameterDefinition[1] : $", {parameterDefinition[1]}";

                    foreach (var arraySize in parameterDefinition[2].GetChildren())
                    {
                        if (arraySize is EmptyArraySizeNode)
                        {
                            parameterTypeList += "[]";
                        }

                        if (arraySize is IntLitNode)
                        {
                            parameterTypeList += $"[{arraySize.Label}]";
                        }
                    }
                }

                node.SymbolTable.AddEntry(child.SymbolTable.Name, "function", $"{funcHead.GetChildren()[2]}:{parameterTypeList}", child.SymbolTable);
            }
        }
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
        var classDeclaration = node.GetChildren();
        node.SymbolTable = new SymbolTable(classDeclaration[0].Label);
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
        return;
    }

    public void Visit(ReturnStatNode node)
    {
        return;
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
        return;
    }

    public void Visit(ParamsOrIndicesNode node)
    {
        return;
    }

    public void Visit(VarNode node)
    {
        return;
    }

    public void Visit(DotNode node)
    {
        return;
    }

    public void Visit(FParamNode node)
    {
        return;
    }

    public void Visit(AssignNode node)
    {
        return;
    }

    public void Visit(EmptyArraySizeNode node)
    {
        return;
    }
}
