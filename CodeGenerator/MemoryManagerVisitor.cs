using ASTGenerator;
using JoCodeTypes;

namespace CodeGenerator;

public class MemoryManagerVisitor : IVisitor
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
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "intlit", BaseType.Int, null);
    }

    public void Visit(FloatLitNode node)
    {
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "floatlit", BaseType.Float, null);
    }

    public void Visit(RelOpNode node)
    {
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "bool", BaseType.Bool, null);
    }

    public void Visit(AddOpNode node)
    {
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "tempvar", node.GetChildren()[0].Type, null);
    }

    public void Visit(MultOpNode node)
    {
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "tempvar", node.GetChildren()[0].Type, null);
    }

    public void Visit(SignNode node)
    {
        var scopeSymbolTable = node.FindSmallestScope();
        scopeSymbolTable?.AddEntry(TagManager.NextVariable(), "tempvar", node.GetChildren()[0].Type, null);
    }

    public void Visit(IdOrSelfNode node)
    {
        return;
    }

    public void Visit(ProgramNode node)
    {
        node.SymbolTable?.ComputeEntrySizesAndOffsets();
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
        node.SymbolTable?.ComputeEntrySizesAndOffsets();
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
        node.SymbolTable?.ComputeEntrySizesAndOffsets();
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
