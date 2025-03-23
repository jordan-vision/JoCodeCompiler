using ASTGenerator;

namespace SemanticAnalyzer;

class ImplementationAndInheritanceVisitor : IVisitor
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
        if (node.SymbolTable == null)
        {
            return;
        }

        foreach (var child in node.GetChildren())
        {
            // Linking method implementations to their respective classes
            if (child is ImplDefNode)
            {
                if (child.SymbolTable == null)
                {
                    continue;
                }

                var name = child.GetChildren()[0].Label;

                if (!node.SymbolTable.DoesEntryExist(name, "class", ""))
                {
                    SemanticAnalyzer.WriteSemanticError($"Implementing the non-existent class {name}.");
                    continue;
                }

                var classTable = node.SymbolTable.GetEntry(name, "class", "").Link;

                if (classTable == null)
                {
                    continue;
                }

                child.SymbolTable.AddEntry(classTable.Name, "class", "", classTable);

                foreach (var entry in child.SymbolTable.GetEntriesOfKind("method").Union(child.SymbolTable.GetEntriesOfKind("constructor")))
                {
                    if (!classTable.DoesEntryExist(entry.Name, entry.Kind, entry.Type))
                    {
                        SemanticAnalyzer.WriteSemanticError($"Implementing the non-existent class {entry.Kind} {classTable.Name}::{entry.Name} -> {entry.Type}.");
                        continue;
                    }

                    if (entry.Link == null)
                    {
                        continue;
                    }

                    classTable.CreateLink(entry.Name, entry.Kind, entry.Type, entry.Link);
                }
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

        var globalTable = node.GetRootNode().SymbolTable;

        if (globalTable == null || node.SymbolTable == null)
        {
            return;
        }

        foreach (var parent in classDeclaration[1].GetChildren())
        {
            if (parent is not IdNode)
            {
                continue;
            }

            var parentClassTable = globalTable.GetEntry(parent.Label, "class", "").Link;

            if (parentClassTable == null)
            {
                continue;
            }

            foreach (var entry in parentClassTable.Entries)
            {
                if (parentClassTable.DoesEntryExist(node.SymbolTable.Name, "inherited", ""))
                {
                    SemanticAnalyzer.WriteSemanticError($"Circular reference {node.SymbolTable.Name} -> {parentClassTable.Name} -> {node.SymbolTable.Name}");
                }
                else if (node.SymbolTable.DoesEntryExist(entry.Name, entry.Kind, entry.Type))
                {
                    SemanticAnalyzer.WriteWarning($"Class {entry.Kind} {node.SymbolTable.Name}::{entry.Name} -> {entry.Type} shadows {parentClassTable.Name}::{entry.Name} -> {entry.Type}");
                } 
                else
                {
                    node.SymbolTable.AddEntry(entry.Name, entry.Kind, entry.Type, entry.Link);
                }
            }
        }
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
