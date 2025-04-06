using ASTGenerator;
using JoCodeTypes;

namespace CodeGenerator;

public class CodeGeneratorVisitor : IVisitor
{
    private Stack<string> availableRegisters = new();

    public CodeGeneratorVisitor()
    {
        for (var i = 13; i > 0; i--)
        {
            availableRegisters.Push($"r{i}");
        }
    }

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
        if (node.SymbolTableEntry == null)
        {
            return;
        }

        var offset = node.SymbolTableEntry.LocalOffset;
        var register = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [register, "r0", node.Label], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{offset}(r14)", register], "");

        availableRegisters.Push(register);
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
        var (head, body) = (node.GetChildren()[0], node.GetChildren()[1]);
        if (head.GetChildren()[0].Label == "main")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "entry", [], "Start here");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r0", "topaddr"], "");
        }

        foreach (var child in body.GetChildren())
        {
            node.MoonCode += child.MoonCode;
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
        if (node.SymbolTableEntry == null)
        {
            return;
        }

        var offset = node.SymbolTableEntry.LocalOffset;
        var currentType = node.SymbolTableEntry.Type;

        var offsetRegister = availableRegisters.Pop();
        var indiceRegister = availableRegisters.Pop();
        var sizeRegister = availableRegisters.Pop();
        var adressRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [offsetRegister, "r0", $"{offset}"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [offsetRegister, offsetRegister, "r14"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [sizeRegister, "r0", "1"], "");

        foreach (var indice in node.GetChildren()[1].GetChildren())
        {
            node.MoonCode += indice.MoonCode;

            if (indice.SymbolTableEntry == null || currentType is not IndicedType)
            {
                continue;
            }

            currentType = ((IndicedType)currentType).BaseType;

            node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [adressRegister, $"{indice.SymbolTableEntry.LocalOffset}(r14)"], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [indiceRegister, "r0", adressRegister], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [sizeRegister, indiceRegister, currentType.Size.ToString()], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", [offsetRegister, offsetRegister, sizeRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{offset}(r14)", offsetRegister], "");

        availableRegisters.Push(adressRegister);
        availableRegisters.Push(sizeRegister);
        availableRegisters.Push(indiceRegister);
        availableRegisters.Push(offsetRegister);
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
        var varNode = node.GetChildren()[0];
        var exprNode = node.GetChildren()[1];

        if (varNode.SymbolTableEntry == null || exprNode.SymbolTableEntry == null)
        {
            return;
        }

        var exprRegister = availableRegisters.Pop();
        var exprOffset = exprNode.SymbolTableEntry.LocalOffset;

        node.MoonCode += exprNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [exprRegister, $"{exprOffset}(r14)"], "");
        node.MoonCode += varNode.MoonCode;

        availableRegisters.Push(exprRegister);
        return;
    }

    public void Visit(EmptyArraySizeNode node)
    {
        return;
    }
}
