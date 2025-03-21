namespace ASTGenerator;

public interface IVisitor
{
    void Visit(EpsilonNode node);

    void Visit(IdNode node);

    void Visit(VisibilityNode node);

    void Visit(TypeNode node);

    void Visit(ReturnTypeNode node);

    void Visit(IntLitNode node);

    void Visit(FloatLitNode node);

    void Visit(RelOpNode node);

    void Visit(AddOpNode node);

    void Visit(MultOpNode node);

    void Visit(SignNode node);

    void Visit(IdOrSelfNode node);

    void Visit(ProgramNode node);

    void Visit(ParentsNode node);

    void Visit(ClassMembersNode node);

    void Visit(ClassMemberNode node);

    void Visit(ClassDeclNode node);

    void Visit(FuncDefsNode node);

    void Visit(ImplDefNode node);

    void Visit(FuncDefNode node);

    void Visit(FParamsNode node);

    void Visit(FuncHeadNode node);

    void Visit(CParamsNode node);

    void Visit(LocalVarDeclOrStatsNode node);

    void Visit(ArraySizesNode node);

    void Visit(VarDeclNode node);

    void Visit(IfStatNode node);

    void Visit(ReadStatNode node);

    void Visit(ReturnStatNode node);

    void Visit(WhileStatNode node);

    void Visit(WriteStatNode node);

    void Visit(StatementsNode node);

    void Visit(EmptyBlockNode node);

    void Visit(NotOpNode node);

    void Visit(ParamsOrIndicesNode node);

    void Visit(VarNode node);

    void Visit(DotNode node);

    void Visit(FParamNode node);

    void Visit(AssignNode node);

    void Visit(EmptyArraySizeNode node);
}
