namespace JoCodeTypes;

public class IndicedType(IJoCodeType baseType, int indice) : IJoCodeType
{
    private string label = baseType.Label + "[]";
    private int size = indice == 0 ? baseType.Size : baseType.Size * indice;

    private IJoCodeType baseType = baseType;
    private int indice = indice;

    public string Label => label;

    public int Size => size;

    public IJoCodeType BaseType => baseType;
}
