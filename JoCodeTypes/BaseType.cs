namespace JoCodeTypes;

public class BaseType : IJoCodeType
{
    private string label = "";
    private int size = 4;

    public string Label => label;

    public int Size => size;

    public static BaseType Int = new() { label = "int" };

    public static BaseType Float = new() { label = "float", size = 8 };

    public static BaseType Bool = new() { label = "bool" };

    public static Dictionary<string, BaseType> ExistingTypes = new() { { "int", Int }, { "float", Float }, { "bool", Bool } };

    public static BaseType GetBaseType(string label)
    {
        if (ExistingTypes.TryGetValue(label, out var type))
        {
            return type;
        }
        
        else
        {
            var newType = new BaseType() { label = label };
            ExistingTypes.Add(label, newType);
            return newType;
        }
    }

    public void ChangeSize(int newSize)
    {
        size = newSize;
    }
}
