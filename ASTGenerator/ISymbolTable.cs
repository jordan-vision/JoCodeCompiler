namespace ASTGenerator;

public interface ISymbolTable
{
    string Name { get; set; }

    List<Entry> Entries { get; }

    AST ASTNode { get; }

    void AddEntry(string name, string kind, string type, ISymbolTable? link);

    bool DoesEntryExist(string name, string kind);

    List<Entry> GetEntriesOfKind(string kind);

    Entry? GetEntry(string name, string kind, string type);

    Entry? GetEntryWithLink(ISymbolTable link);

    List<Entry> GetEntriesWithName(string name);

    void CreateLink(string name, string kind, string type, ISymbolTable link);

    string ToString();

    string GetString(int indent);

    void ComputeSize();

    void GenerateEntry(string kind, string type);
}

public class Entry(string name, string kind, string type, ISymbolTable? link)
{
    private string name = name, kind = kind, type = type;
    private ISymbolTable? link = link;
    private int size;

    public string Name => name;

    public string Kind => kind;

    public string Type => type;

    public ISymbolTable? Link { get { return link; } set { link = value; } }

    public int Size { get { return size; } set { size = value; } }

    public (string, List<int>) GetReturnTypePrefixAndSuffix()
    {
        var returnString = "";
        var returnInts = new List<int>();

        var prefix = type.Contains(':') ? type[1..(type.IndexOf(':') - 1)] : type;
        bool inIndex = false;
        string currentIndiceString = "";

        for (int i = 0; i < prefix.Length - 1; i++)
        {
            if (inIndex)
            {
                if (prefix[i] == ']')
                {
                    inIndex = false;
                    returnInts.Add(int.Parse(currentIndiceString));
                    currentIndiceString = "";
                } 
                else if (prefix[i] >= '0' && prefix[i] <= '9')
                {
                    currentIndiceString += prefix[i];
                }
            }

            if (prefix[i] == '[' && prefix[i + 1] != ']')
            {
                inIndex = true;
            }

            returnString += inIndex ? "" : prefix[i];
        }

        returnString += prefix[^1];
        return (returnString, returnInts);
    }
}
