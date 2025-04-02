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
        node.SymbolTable = new SymbolTable("global", node);

        foreach (var child in node.GetChildren())
        {
            // Linking all classes in global scope
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

                var funcHead = child.GetChildren()[0];

                if (funcHead is not FuncHeadNode)
                {
                    continue;
                }

                if (node.SymbolTable.DoesEntryExist(child.SymbolTable.Name, "function"))
                {
                    SemanticAnalyzer.WriteWarning($"Multiple functions {child.SymbolTable.Name} declared.", funcHead.Position);
                }

                var kind = (child.SymbolTable.Name == "main") ? "main function" : "function";
                node.SymbolTable.AddEntry(child.SymbolTable.Name, kind, ((FuncHeadNode)funcHead).GetTypeString(), child.SymbolTable);
            }
        }

        var mainFunctions = node.SymbolTable.GetEntriesOfKind("main function").Count;
        if (mainFunctions == 0)
        {
            SemanticAnalyzer.WriteSemanticError("No main function.", node.Position);
        } 
        else if (mainFunctions > 1)
        {
            SemanticAnalyzer.WriteSemanticError("Multiple main functions.", node.Position);
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
        node.SymbolTable = new SymbolTable(classDeclaration[0].Label, node);

        foreach (var parent in classDeclaration[1].GetChildren())
        {
            if (parent is not IdNode)
            {
                continue;
            }

            node.SymbolTable.AddEntry(parent.Label, "inherited", "", null);
        }

        foreach (var member in classDeclaration[2].GetChildren())
        {
            if (member is not ClassMemberNode)
            {
                continue;
            }

            var decl = member.GetChildren()[1];

            if (decl is VarDeclNode)
            {
                var varDecl = decl.GetChildren();
                var type = varDecl[1].ToString() + ((ArraySizesNode)varDecl[2]).GetSizesString();
                node.SymbolTable.AddEntry(varDecl[0].Label, "attribute", type, null);
            }

            if (decl is FuncHeadNode funcHead)
            {
                var name = decl.GetChildren()[0].Label;
                if (node.SymbolTable.DoesEntryExist(name, "method"))
                {
                    SemanticAnalyzer.WriteWarning($"Multiple class methods {node.SymbolTable.Name}::{name} declared.", funcHead.Position);
                }

                node.SymbolTable.AddEntry(name, "method", funcHead.GetTypeString(), null);
            }

            if (decl is CParamsNode cParams)
            {
                node.SymbolTable.AddEntry(node.SymbolTable.Name, "constructor", cParams.GetTypeString(), null);
            }
        }
    }

    public void Visit(FuncDefsNode node)
    {
        return;
    }

    public void Visit(ImplDefNode node)
    {
        var implDef = node.GetChildren();
        node.SymbolTable = new SymbolTable(implDef[0].Label, node);

        foreach (var funcDef in implDef[1].GetChildren())
        {
            if (funcDef.SymbolTable == null)
            {
                continue;
            }

            var funcHead = funcDef.GetChildren()[0];
            
            if (funcHead is FuncHeadNode funcHeadNode)
            {
                var funcHeadChildren = funcHead.GetChildren();
                node.SymbolTable.AddEntry(funcHeadChildren[0].Label, "method", funcHeadNode.GetTypeString(), funcDef.SymbolTable);
            }

            if (funcHead is CParamsNode cParams)
            {
                funcDef.SymbolTable.Name = node.SymbolTable.Name;
                node.SymbolTable.AddEntry(node.SymbolTable.Name, "constructor", cParams.GetTypeString(), funcDef.SymbolTable);
            }
        }
    }

    public void Visit(FuncDefNode node)
    {
        var funcDef = node.GetChildren();
        var funcHead = funcDef[0];

        var name = "";
        var parameters = funcHead;

        if (funcHead is FuncHeadNode)
        {
            name = funcHead.GetChildren()[0].Label;
            parameters = funcHead.GetChildren()[1];
        }

        node.SymbolTable = new SymbolTable(name, node);
       
        foreach (var param in parameters.GetChildren())
        {
            if (param is not FParamNode)
            {
                continue;
            }

            var parameter = param.GetChildren();
            var type = parameter[1].Label + ((ArraySizesNode)parameter[2]).GetSizesString();
            node.SymbolTable.AddEntry(parameter[0].Label, "parameter", type, null);
        }

        foreach (var varDecl in funcDef[1].GetChildren())
        {
            if (varDecl is not VarDeclNode)
            {
                continue;
            }

            var varDeclChildren = varDecl.GetChildren();
            var type = varDeclChildren[1].Label + ((ArraySizesNode)varDeclChildren[2]).GetSizesString();
            node.SymbolTable.AddEntry(varDeclChildren[0].Label, "localvar", type, null);
        }
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
