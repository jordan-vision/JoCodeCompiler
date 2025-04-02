using ASTGenerator;

namespace SemanticAnalyzer;

public class SymbolTable(string name, AST astNode) : ISymbolTable
{
    private string name = name;
    private List<Entry> entries = [];
    private AST astNode = astNode;
    private int size, nextGeneratedEntryIndex = 1;

    public string Name { get { return name; } set { name = value; } }

    public List<Entry> Entries => entries;

    public AST ASTNode => astNode;

    public int Size => size;

    public void AddEntry(string name, string kind, string type, ISymbolTable? link)
    {
        if (IsEntryDuplicate(name, kind, type))
        {
            SemanticAnalyzer.WriteSemanticError($"Multiple definitions of the {kind} {name}.", astNode.Position);
            return;
        }

        var newEntry = new Entry(name, kind, type, link);

        
        if (kind == "inherited")
        {
            entries.Add(newEntry);
        }

        var baseSize = 4;
        var (typePrefix, typeSuffix) = newEntry.GetReturnTypePrefixAndSuffix();

        entries.Add(newEntry);
    }

    public bool DoesEntryExist(string name, string kind)
    {
        return entries.Any(e => e.Name.Equals(name) && e.Kind.Equals(kind));
    }

    private bool IsEntryDuplicate(string name, string kind, string type)
    {
        return entries.Any(e => e.Name.Equals(name) && e.Kind.Equals(kind) && e.Type.Equals(type));
    }

    public List<Entry> GetEntriesOfKind(string kind)
    {
        return [.. entries.Where(e => e.Kind.Equals(kind))];
    }

    public Entry? GetEntry(string name, string kind, string type)
    {
        var returnValue = entries.FirstOrDefault(e => e.Name.Equals(name) && e.Kind.Equals(kind) && e.Type.Equals(type));
        return returnValue == default(Entry) ? null : returnValue;
    }

    public Entry? GetEntryWithLink(ISymbolTable link)
    {
        var returnValue = entries.FirstOrDefault(e => e.Link == link);
        return returnValue == default(Entry) ? null : returnValue;
    }

    public List<Entry> GetEntriesWithName(string name)
    {
        return [.. entries.Where(e => e.Name == name)];
    }

    public void CreateLink(string name, string kind, string type, ISymbolTable link)
    {
        var entry = GetEntry(name, kind, type);

        if (entry != null)
        {
            entry.Link = link;
        }
    }

    public override string ToString()
    {
        return GetString(0);
    }

    public string GetString(int indent)
    {
        var returnValue = "";
        var prefix = "";

        for (var i = 0; i < indent; i++)
        {
            prefix += "\t";
        }

        returnValue += prefix + "====\n";
        returnValue += prefix + $"TABLE: {name}\n";
        returnValue += prefix + "----\n";

        foreach(var entry in entries)
        {
            returnValue += prefix + $"| {entry.Name}\t{entry.Kind}\t{entry.Type}\t{entry.Size}";

            if (entry.Link == null)
            {
                returnValue += "\n";
                continue;
            }

            returnValue += $"\t{entry.Link.Name}\n";
            returnValue += entry.Link.GetString(indent + 1);
        }

        returnValue += prefix + "====\n";

        return returnValue;
    }

    public void ComputeSize()
    {
        size = 0;

        foreach (var entry in entries.Where(e => !e.Type.Equals("inherited")))
        {
            size += entry.Size;
        }
    }

    public void GenerateEntry(string kind, string type)
    {
        AddEntry($"t{nextGeneratedEntryIndex++}", kind, type, null);
    }
}
