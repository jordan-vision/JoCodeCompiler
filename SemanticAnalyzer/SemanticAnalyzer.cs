using ASTGenerator;

namespace SemanticAnalyzer;

public class SemanticAnalyzer
{
    public static void TraverseAST()
    {
        SemanticStack.Traverse(new SymbolTableGeneratorVisitor());
    }
}
