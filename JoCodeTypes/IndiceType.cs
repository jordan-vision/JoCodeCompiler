namespace JoCodeTypes;

public class IndicedType(IJoCodeType baseType, int indice) : IJoCodeType
{
    private string label = baseType.Label + "[]";

    private IJoCodeType baseType = baseType;
    private int indice = indice;

    public string Label => label;

    public int Size => indice == 0 ? baseType.Size : baseType.Size * indice;

    public IJoCodeType BaseType => baseType;

    public IJoCodeType OriginalBaseType()
    {
        var type = baseType;

        while (type is IndicedType)
        {
            type = ((IndicedType)type).BaseType;
        }

        return type;
    }
}
