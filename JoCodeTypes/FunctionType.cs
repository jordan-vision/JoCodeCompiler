namespace JoCodeTypes;

public class FunctionType : IJoCodeType
{
    private string label;
    private int size;

    private List<IJoCodeType> parameterList;
    private IJoCodeType returnType;

    public string Label => label;

    public int Size => size;

    public IJoCodeType ReturnType => returnType;

    public FunctionType(List<IJoCodeType> parameterList, IJoCodeType returnType)
    {
        this.parameterList = parameterList;
        this.returnType = returnType;

        label = $"({ParameterTypes()}):{returnType.Label}";
        size = 0;
    }

    public string ParameterTypes()
    {
        return parameterList.Count == 0 ? "" : String.Join(", ", parameterList.Select(p => p.Label));
    }
}
