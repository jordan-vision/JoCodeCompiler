
namespace LexicalAnalyzer;

public class Token
{
    private string type, lexeme;
    private (int, int) location;

    public string Lexeme { get { return lexeme; } set { lexeme = value; } }
    public string Type { get { return type; } set { type = value; } }
    public (int, int) Location { get { return location; } set { location = value; } }

    public Token()
    {
        type = "SKIP";
        lexeme = "";
        location = (0, 0);
    }

    public override string ToString()
    {
        return $"[{Type}, {Lexeme}, {Location}]";
    }
}