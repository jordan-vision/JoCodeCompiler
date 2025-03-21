namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; }

    void AddEntry(string name, string kind, string type, ISymbolTable link);
}
