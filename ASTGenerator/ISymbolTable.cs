using JoCodeTypes;

namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; set; }

    List<Entry> Entries { get; }

    AST ASTNode { get; }

    void AddEntry(string name, string kind, IJoCodeType? type, ISymbolTable? link);

    bool DoesEntryExist(string name, string kind);

    List<Entry> GetEntriesOfKind(string kind);

    Entry? GetEntry(string name, string kind, IJoCodeType? type);

    Entry? GetEntryWithLink(ISymbolTable link);

    List<Entry> GetEntriesWithName(string name);

    void CreateLink(string name, string kind, IJoCodeType? type, ISymbolTable link);

    string ToString();

    string GetString(int indent);

    void ComputeSize();
}

public class Entry(string name, string kind, IJoCodeType? type, ISymbolTable? link)
{
    private string name = name, kind = kind;
    private IJoCodeType? type = type;
    private ISymbolTable? link = link;

    public string Name => name;

    public string Kind => kind;

    public IJoCodeType? Type => type;

    public ISymbolTable? Link { get { return link; } set { link = value; } }
}
