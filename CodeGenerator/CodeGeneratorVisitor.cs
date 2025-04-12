using ASTGenerator;
using JoCodeTypes;
using static System.Formats.Asn1.AsnWriter;

namespace CodeGenerator;

public class CodeGeneratorVisitor : IVisitor
{
    private Stack<string> availableRegisters = new();
    private int nextIfElse = 1, nextWhile = 1;

    public CodeGeneratorVisitor()
    {
        for (var i = 12; i > 0; i--)
        {
            availableRegisters.Push($"r{i}");
        }
    }

    public (string, string) NewIfElse()
    {
        return ($"else{nextIfElse}", $"endif{nextIfElse++}");
    }

    public (string, string) NewWhile()
    {
        return ($"whilestart{nextWhile}", $"whileend{nextWhile++}");
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

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [register, "r0", node.Label], $"{node.Position}: Storing literal {node.SymbolTableEntry.Name} = {node.Label}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{offset}(r14)", register], "");

        availableRegisters.Push(register);
    }

    public void Visit(FloatLitNode node)
    {
        return;
    }

    public void Visit(RelOpNode node)
    {
        var leftOperand = node.GetChildren()[0];
        var rightOperand = node.GetChildren()[1];

        if (node.SymbolTableEntry == null || leftOperand.SymbolTableEntry == null || rightOperand.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += leftOperand.MoonCode;
        node.MoonCode += rightOperand.MoonCode;

        var leftRegister = availableRegisters.Pop();
        var rightRegister = availableRegisters.Pop();
        var resultRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [leftRegister, $"{leftOperand.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Evaluating {node.SymbolTableEntry.Name} = {leftOperand.SymbolTableEntry.Name} {node.Label} {rightOperand.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [rightRegister, $"{rightOperand.SymbolTableEntry.LocalOffset}(r14)"], "");

        var comparisonOpCode = "";

        if (node.Label == "<")
        {
            comparisonOpCode = "clt";
        }
        else if (node.Label == "<=")
        {
            comparisonOpCode = "cle";
        }
        else if (node.Label == "<>")
        {
            comparisonOpCode = "cne";
        }
        else if (node.Label == "==")
        {
            comparisonOpCode = "ceq";
        } 
        else if (node.Label == ">")
        {
            comparisonOpCode = "cgt";
        }
        else if (node.Label == ">=")
        {
            comparisonOpCode = "cge";
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", comparisonOpCode, [resultRegister, leftRegister, rightRegister], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{node.SymbolTableEntry.LocalOffset}(r14)", resultRegister], "");

        availableRegisters.Push(resultRegister);
        availableRegisters.Push(rightRegister);
        availableRegisters.Push(leftRegister);
    }

    public void Visit(AddOpNode node)
    {
        var leftOperand = node.GetChildren()[0];
        var rightOperand = node.GetChildren()[1];

        if (node.SymbolTableEntry == null || leftOperand.SymbolTableEntry == null || rightOperand.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += leftOperand.MoonCode;
        node.MoonCode += rightOperand.MoonCode;

        var leftRegister = availableRegisters.Pop();
        var rightRegister = availableRegisters.Pop();
        var resultRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [leftRegister, $"{leftOperand.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Evaluating {node.SymbolTableEntry.Name} = {leftOperand.SymbolTableEntry.Name} {node.Label} {rightOperand.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [rightRegister, $"{rightOperand.SymbolTableEntry.LocalOffset}(r14)"], "");

        if (node.Label == "+" || node.Label == "or")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [resultRegister, leftRegister, rightRegister], "");
        } 
        else if (node.Label == "-")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", [resultRegister, leftRegister, rightRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{node.SymbolTableEntry.LocalOffset}(r14)", resultRegister], "");

        availableRegisters.Push(resultRegister);
        availableRegisters.Push(rightRegister);
        availableRegisters.Push(leftRegister);
    }

    public void Visit(MultOpNode node)
    {
        var leftOperand = node.GetChildren()[0];
        var rightOperand = node.GetChildren()[1];

        if (node.SymbolTableEntry == null || leftOperand.SymbolTableEntry == null || rightOperand.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += leftOperand.MoonCode;
        node.MoonCode += rightOperand.MoonCode;

        var leftRegister = availableRegisters.Pop();
        var rightRegister = availableRegisters.Pop();
        var resultRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [leftRegister, $"{leftOperand.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Evaluating {node.SymbolTableEntry.Name} = {leftOperand.SymbolTableEntry.Name} {node.Label} {rightOperand.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [rightRegister, $"{rightOperand.SymbolTableEntry.LocalOffset}(r14)"], "");

        if (node.Label == "*" || node.Label == "and")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "mul", [resultRegister, leftRegister, rightRegister], "");
        }
        else if (node.Label == "/")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "div", [resultRegister, leftRegister, rightRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{node.SymbolTableEntry.LocalOffset}(r14)", resultRegister], "");

        availableRegisters.Push(resultRegister);
        availableRegisters.Push(rightRegister);
        availableRegisters.Push(leftRegister);
    }

    public void Visit(SignNode node)
    {
        var operand = node.GetChildren()[0];

        if (node.SymbolTableEntry == null || operand.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += operand.MoonCode;

        var register = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [register, $"{operand.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Evaluating {node.SymbolTableEntry.Name} = {node.Label}{operand.SymbolTableEntry.Name}");
        if (node.Label == "-")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [register, register, "-1"], "");
        }
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{node.SymbolTableEntry.LocalOffset}(r14)", register], "");

        availableRegisters.Push(register);
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

            node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r1", "r0", "10"], "Setting value of newline character");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", ["newline(r0)", "r1"], "");

            node.MoonCode += "\n";
        }

        node.MoonCode += $"%====FUNCTION START: {functionName}====\n";

        foreach (var child in body.GetChildren())
        {
            node.MoonCode += child.MoonCode;
        }

        node.MoonCode += $"%====FUNCTION END: {functionName}====\n\n";

        if (functionName == "main")
        {
            node.MoonCode += CodeGenerator.MoonCodeLine("", "hlt", [], "End here");
            node.MoonCode += CodeGenerator.MoonCodeLine("buf", "res", ["20"], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("newline", "res", ["4"], "");

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
        var relExprNode = node.GetChildren()[0];
        var thenStatNode = node.GetChildren()[1];
        var elseStatNode = node.GetChildren()[2];

        if (relExprNode.SymbolTableEntry == null)
        {
            return;
        }

        node.MoonCode += relExprNode.MoonCode;

        var register = availableRegisters.Pop();
        var (elsePointer, endIfPointer) = NewIfElse();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [register, $"{relExprNode.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Processing if({relExprNode.SymbolTableEntry.Name}) statement");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "bz", [register, elsePointer], "");
        node.MoonCode += thenStatNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine("", "j", [endIfPointer], "");
        node.MoonCode += CodeGenerator.MoonCodeLine(elsePointer, "", [], "");
        node.MoonCode += elseStatNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine(endIfPointer, "", [], "");

        availableRegisters.Push(register);
    }

    public void Visit(ReadStatNode node)
    {
        var varNode = node.GetChildren()[0];
        var scope = node.FindSmallestScope();

        if (varNode is not VarNode || varNode.SymbolTableEntry == null || scope == null)
        {
            return;
        }

        foreach (var indice in varNode.GetChildren()[1].GetChildren())
        {
            node.MoonCode += indice.MoonCode;
        }

        var varOffset = ((VarNode)varNode).Reference!.LocalOffset;
        var currentType = ((VarNode)varNode).Reference!.Type;

        var readValueRegister = availableRegisters.Pop(); // getint stores result in r1
        var offsetRegister = availableRegisters.Pop();
        var sizeRegister = availableRegisters.Pop();
        var indiceRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", (-scope.Size).ToString()], $"{node.Position}: Adding to stack frame to call getint function");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "jl", ["r15", "getint"], $"{node.Position}: Calling getint function");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", scope.Size.ToString()], "");

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [offsetRegister, "r0", $"{varOffset}"], $"{node.Position}: Finding address of {((VarNode)varNode).Reference!.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [offsetRegister, offsetRegister, "r14"], "");

        foreach (var indice in varNode.GetChildren()[1].GetChildren())
        {
            if (indice.SymbolTableEntry == null || currentType is not IndicedType)
            {
                continue;
            }

            currentType = ((IndicedType)currentType).BaseType;

            node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [indiceRegister, $"{indice.SymbolTableEntry.LocalOffset}(r14)"], $"{indice.Position}: Computing offset [{indice.SymbolTableEntry.Name}]");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [sizeRegister, indiceRegister, currentType.Size.ToString()], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", [offsetRegister, offsetRegister, sizeRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"0({offsetRegister})", "r1"], $"Storing read value in {((VarNode)varNode).Reference!.Name}");

        availableRegisters.Push(indiceRegister);
        availableRegisters.Push(sizeRegister);
        availableRegisters.Push(offsetRegister);
        availableRegisters.Push(readValueRegister);
    }

    public void Visit(ReturnStatNode node)
    {
        return;
    }

    public void Visit(WhileStatNode node)
    {
        var relExprNode = node.GetChildren()[0];
        var whileBlockNode = node.GetChildren()[1];

        if (relExprNode.SymbolTableEntry == null)
        {
            return;
        }

        var register = availableRegisters.Pop();
        var (whileStart, whileEnd) = NewWhile();

        node.MoonCode += CodeGenerator.MoonCodeLine(whileStart, "", [], $"{node.Position}: Processing while({relExprNode.SymbolTableEntry.Name}) statement");
        node.MoonCode += relExprNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [register, $"{relExprNode.SymbolTableEntry.LocalOffset}(r14)"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "bz", [register, whileEnd], "");
        node.MoonCode += whileBlockNode.MoonCode;
        node.MoonCode += CodeGenerator.MoonCodeLine("", "j", [whileStart], "");
        node.MoonCode += CodeGenerator.MoonCodeLine(whileEnd, "", [], "");

        availableRegisters.Push(register);
    }

    public void Visit(WriteStatNode node)
    {
        var exprNode = node.GetChildren()[0];
        var scope = node.FindSmallestScope();

        if (exprNode.SymbolTableEntry == null || scope == null)
        {
            return;
        }

        node.MoonCode += exprNode.MoonCode;

        var register = availableRegisters.Pop(); // r1

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [register, $"{exprNode.SymbolTableEntry.LocalOffset}(r14)"], $"{node.Position}: Calling putint function");

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", (-scope.Size).ToString()], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "jl", ["r15", "putint"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", scope.Size.ToString()], "");

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [register, "r0", "newline"], "");

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", (-scope.Size).ToString()], $"{node.Position}: writing new line");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "jl", ["r15", "putstr"], "");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", ["r14", "r14", scope.Size.ToString()], "");

        availableRegisters.Push(register);
    }

    public void Visit(StatementsNode node)
    {
        foreach (var child in node.GetChildren())
        {
            node.MoonCode += child.MoonCode;
        }
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
        if (node.SymbolTableEntry == null || node.Reference == null)
        {
            return;
        }

        foreach (var indice in node.GetChildren()[1].GetChildren())
        {
            node.MoonCode += indice.MoonCode;
        }

        var varOffset = node.Reference.LocalOffset;
        var currentType = node.Reference.Type;

        var offsetRegister = availableRegisters.Pop();
        var sizeRegister = availableRegisters.Pop();
        var indiceRegister = availableRegisters.Pop();
        var resultRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [offsetRegister, "r0", $"{varOffset}"], $"{node.Position}: Getting value of {node.Reference.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [offsetRegister, offsetRegister, "r14"], "");

        foreach (var indice in node.GetChildren()[1].GetChildren())
        {
            if (indice.SymbolTableEntry == null || currentType is not IndicedType)
            {
                continue;
            }

            currentType = ((IndicedType)currentType).BaseType;


            node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [indiceRegister, $"{indice.SymbolTableEntry.LocalOffset}(r14)"], $"{indice.Position}: Computing offset [{indice.SymbolTableEntry.Name}]");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [sizeRegister, indiceRegister, currentType.Size.ToString()], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", [offsetRegister, offsetRegister, sizeRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [resultRegister, $"0({offsetRegister})"], $"{node.Position}: Storing result of get {node.Reference.Name} in {node.SymbolTableEntry.Name}.");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"{node.SymbolTableEntry.LocalOffset}(r14)", resultRegister], "");

        availableRegisters.Push(resultRegister);
        availableRegisters.Push(indiceRegister);
        availableRegisters.Push(sizeRegister);
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

        if (varNode.SymbolTableEntry == null || exprNode.SymbolTableEntry == null || varNode is not VarNode || ((VarNode)varNode).Reference == null)
        {
            return;
        }

        node.MoonCode += exprNode.MoonCode;

        foreach (var indice in varNode.GetChildren()[1].GetChildren())
        {
            node.MoonCode += indice.MoonCode;
        }

        var exprOffset = exprNode.SymbolTableEntry.LocalOffset;
        var varOffset = ((VarNode)varNode).Reference!.LocalOffset;
        var currentType = ((VarNode)varNode).Reference!.Type;

        var exprRegister = availableRegisters.Pop();
        var offsetRegister = availableRegisters.Pop();
        var sizeRegister = availableRegisters.Pop();
        var indiceRegister = availableRegisters.Pop();

        node.MoonCode += CodeGenerator.MoonCodeLine("", "addi", [offsetRegister, "r0", $"{varOffset}"], $"{node.Position}: Finding address of {((VarNode)varNode).Reference!.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "add", [offsetRegister, offsetRegister, "r14"], "");

        foreach (var indice in varNode.GetChildren()[1].GetChildren())
        {
            if (indice.SymbolTableEntry == null || currentType is not IndicedType)
            {
                continue;
            }

            currentType = ((IndicedType)currentType).BaseType;

            node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [indiceRegister, $"{indice.SymbolTableEntry.LocalOffset}(r14)"], $"{indice.Position}: Computing offset [{indice.SymbolTableEntry.Name}]");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "muli", [sizeRegister, indiceRegister, currentType.Size.ToString()], "");
            node.MoonCode += CodeGenerator.MoonCodeLine("", "sub", [offsetRegister, offsetRegister, sizeRegister], "");
        }

        node.MoonCode += CodeGenerator.MoonCodeLine("", "lw", [exprRegister, $"{exprOffset}(r14)"], $"{node.Position}: Performing assignment {((VarNode)varNode).Reference!.Name} := {exprNode.SymbolTableEntry.Name}");
        node.MoonCode += CodeGenerator.MoonCodeLine("", "sw", [$"0({offsetRegister})", exprRegister], "");

        availableRegisters.Push(indiceRegister);
        availableRegisters.Push(sizeRegister);
        availableRegisters.Push(offsetRegister);
        availableRegisters.Push(exprRegister);
    }

    public void Visit(EmptyArraySizeNode node)
    {
        return;
    }
}
