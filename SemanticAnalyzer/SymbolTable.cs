using ASTGenerator;

namespace SemanticAnalyzer;

public class SymbolTable(string name) : ISymbolTable
{
    private string name = name;
    private List<Entry> entries = new();

    public string Name { get { return name; } }

    public void AddEntry(string name, string kind, string type, ISymbolTable? link)
    {
        entries.Add(new(name, kind, type, link));
    }
}

public class Entry(string name, string kind, string type, ISymbolTable? link)
{
    private string name = name, kind = kind, type = type;
    private ISymbolTable? link = link;
}
