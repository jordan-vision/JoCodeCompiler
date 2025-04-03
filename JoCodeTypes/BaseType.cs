namespace JoCodeTypes;

public class BaseType(string label) : IJoCodeType
{
    private string label = label;
    private int size = 4;

    public string Label => label;

    public int Size => size;

    public static BaseType Int = new("int");

    public static BaseType Float = new("float") { size = 8 };

    public static BaseType Bool = new("bool");
}
