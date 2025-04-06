using ASTGenerator;
using JoCodeTypes;

namespace CodeGenerator;

public class CodeGeneratorVisitor : IVisitor
{
    private Stack<string> availableRegisters = new();

    public CodeGeneratorVisitor()
    {
        for (var i = 12; i > 0; i--)
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

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [register, "r0", node.Label], $"{node.Position}: Storing literal {node.SymbolTableEntry.Name}={node.Label}");
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
        foreach (var child in node.GetChildren())
        {
            node.MoonCode += child.MoonCode;
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
        var functionName = head.GetChildren()[0].Label;

        if (functionName == "main")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "entry", [], "Start here");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r0", "topaddr"], "");
            node.MoonCode += "\n";
        }

        node.MoonCode += $"%====FUNCTION START: {functionName}====\n";

        foreach (var child in body.GetChildren())
        {
            node.MoonCode += child.MoonCode;
        }

        node.MoonCode += $"%====FUNCTION END: {functionName}====\n\n";

        if (head.GetChildren()[0].Label == "main")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "hlt", [], "End here");
            node.MoonCode += CodeGenerator.MoonCodeLine("buf", "res", ["20"], "");
            node.MoonCode += "\n";
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
        var varNode = node.GetChildren()[0];
        var scope = node.FindSmallestScope();

        if (varNode.SymbolTableEntry == null || scope == null)
        {
            return;
        }

        node.MoonCode += varNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", (-scope.Size).ToString()], $"{node.Position}: Adding to stack frame to call getstr function");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", ["-8(r14)", "r13"], $"{node.Position}: Setting parameter for getstr function.");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "jl", ["r15", "getstr"], $"{node.Position}: Calling getstr function.");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", scope.Size.ToString()], "");
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
        var exprNode = node.GetChildren()[0];
        if (exprNode.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += exprNode.MoonCode;
        // TODO: finish this later
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

        foreach (var indice in node.GetChildren()[1].GetChildren())
        {
            node.MoonCode += indice.MoonCode;
        }

        var varOffset = node.SymbolTableEntry.LocalOffset;
        var currentType = node.SymbolTableEntry.Type;

        var sizeRegister = availableRegisters.Pop();
        var indiceRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "add", ["r13", "r0", "r14"], $"{node.Position}: Finding adress of {node.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r13", "r13", $"{varOffset}"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [sizeRegister, "r0", "1"], "");

        foreach (var indice in node.GetChildren()[1].GetChildren())
        {
            if (indice.SymbolTableEntry == null || currentType is not IndicedType)
            {
                continue;
            }

            currentType = ((IndicedType)currentType).BaseType;

            node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [indiceRegister, $"{indice.SymbolTableEntry.LocalOffset}(r14)"], $"{indice.Position}: Computing offset [{indice.SymbolTableEntry.Name}]");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [sizeRegister, indiceRegister, currentType.Size.ToString()], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", ["r13", "r13", sizeRegister], "");
        }

        availableRegisters.Push(sizeRegister);
        availableRegisters.Push(indiceRegister);
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

        node.MoonCode += varNode.MoonCode;
        node.MoonCode += exprNode.MoonCode;

        var exprOffset = exprNode.SymbolTableEntry.LocalOffset;
        var exprRegister = availableRegisters.Pop();

        var varName = varNode.SymbolTableEntry.Name;
        foreach (var indice in varNode.GetChildren()[0].GetChildren())
        {
            if (indice is IndiceNode)
            {
                varName += "[]";
            }
        }

        // TODO: do this for all elements in array
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [$"{exprRegister}", $"{exprNode.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Performing assignment {varName}:={exprNode.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{varNode.SymbolTableEntry.LocalOffset}(r13)", $"{exprRegister}"], "");

        availableRegisters.Push(exprRegister);
    }

    public void Visit(EmptyArraySizeNode node)
    {
        return;
    }
}
