namespace JoCodeTypes;

public class IndicedType(IJoCodeType baseType, int indice) : IJoCodeType
{
    private string label = baseType.Label + $"[{indice}]";
    private int size = baseType.Size * indice;

    private IJoCodeType baseType = baseType;
    private int indice = indice;

    public string Label => label;

    public int Size => size;
}
