namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; set; }

    List<Entry> Entries { get; }

    void AddEntry(string name, string kind, string type, ISymbolTable? link);

    bool DoesEntryExist(string name, string kind, string type);

    List<Entry> GetEntriesOfKind(string kind);

    Entry GetEntry(string name, string kind, string type);

    void CreateLink(string name, string kind, string type, ISymbolTable link);

    string ToString();

    string GetString(int indent);
}

public class Entry(string name, string kind, string type, ISymbolTable? link)
{
    private string name = name, kind = kind, type = type;
    private ISymbolTable? link = link;

    public string Name => name;

    public string Kind => kind;

    public string Type => type;

    public ISymbolTable? Link { get { return link; } set { link = value; } }
}
