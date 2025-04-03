namespace JoCodeTypes;

public class IndicedType : IJoCodeType
{
    private string label;
    private int size;

    private IJoCodeType baseType;
    private int indice;

    public string Label => label;

    public int Size => size;

    public IndicedType(IJoCodeType baseType, int indice)
    {
        this.baseType = baseType;
        this.indice = indice;

        label = baseType.Label + $"[{indice}]";
        size = baseType.Size * indice;
    }
}
