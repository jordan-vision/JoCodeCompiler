using ASTGenerator;

namespace SemanticAnalyzer;

public class SymbolTable(string name) : ISymbolTable
{
    private string name = name;
    private List<Entry> entries = new();

    public string Name { get { return name; } set { name = value; } }

    public List<Entry> Entries => entries;

    public void AddEntry(string name, string kind, string type, ISymbolTable? link)
    {
        if (DoesEntryExist(name, kind, type))
        {
            SemanticAnalyzer.WriteSemanticError($"Multiple definitions of the {kind} {name}");
            return;
        }

        entries.Add(new(name, kind, type, link));
    }

    public bool DoesEntryExist(string name, string kind, string type)
    {
        return entries.Any(e => e.Name.Equals(name) && e.Kind.Equals(kind) && e.Type.Equals(type));
    }

    public List<Entry> GetEntriesOfKind(string kind)
    {
        return [.. entries.Where(e => e.Kind.Equals(kind))];
    }

    public Entry GetEntry(string name, string kind, string type)
    {
        return entries.First(e => e.Name.Equals(name) && e.Kind.Equals(kind) && e.Type.Equals(type));
    }

    public void CreateLink(string name, string kind, string type, ISymbolTable link)
    {
        GetEntry(name, kind, type).Link = link;
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
            returnValue += prefix + $"| {entry.Name}\t{entry.Kind}\t{entry.Type}";

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
}
