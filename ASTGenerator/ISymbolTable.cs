using JoCodeTypes;

namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; set; }

    List<Entry> Entries { get; }

    AST ASTNode { get; }

    int Size { get; }

    static AST? MainFunctionNode { get; set; }

    void AddEntry(string name, string kind, IJoCodeType? type, ISymbolTable? link, AST? node = null);

    bool DoesEntryExist(string name, string kind);

    List<Entry> GetEntriesOfKind(string kind);

    Entry? GetEntry(string name, string kind, IJoCodeType? type);

    Entry? GetEntryWithLink(ISymbolTable link);

    List<Entry> GetEntriesWithName(string name);

    void CreateLink(string name, string kind, IJoCodeType? type, ISymbolTable link);

    string ToString();

    string GetString(int indent);

    void ComputeSizeAndOffsets();

    void AddEntryFirst(string name, string kind, IJoCodeType? type, ISymbolTable? link);

    void GenerateEntry(string kind, IJoCodeType? type, ISymbolTable? link, AST? node = null);
}

public class Entry(string name, string kind, IJoCodeType? type, ISymbolTable? link)
{
    private string name = name, kind = kind;
    private IJoCodeType? type = type;
    private ISymbolTable? link = link;
    private int localOffset;

    public string Name => name;

    public string Kind => kind;

    public IJoCodeType? Type => type;

    public ISymbolTable? Link { get { return link; } set { link = value; } }

    public int LocalOffset { get { return localOffset; } set { localOffset = value; } }
}
