namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; set; }

    List<Entry> Entries { get; }

    AST ASTNode { get; }

    void AddEntry(string name, string kind, string type, ISymbolTable? link);

    bool DoesEntryExist(string name, string kind, string type);

    List<Entry> GetEntriesOfKind(string kind);

    Entry? GetEntry(string name, string kind, string type);

    Entry? GetEntryWithLink(ISymbolTable link);

    List<Entry> GetEntriesWithName(string name);

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

    public string TypePrefix()
    {
        var returnValue = "";

        var prefix = type[1..(type.IndexOf(':') - 1)];
        bool skipThisCharacter = false;

        for (int i = 0; i < prefix.Length - 1; i++)
        {
            if (skipThisCharacter && prefix[i] == ']')
            {
                skipThisCharacter = false;
            }

            if (prefix[i] == '[' && prefix[i + 1] != ']')
            {
                skipThisCharacter = true;
            }

            returnValue += skipThisCharacter ? "" : prefix[i];
        }

        returnValue += prefix[^1];
        return returnValue;
    }
}
