namespace CodeGenerator;

public class TagManager
{
    private static int nextTag = 1;

    public static string NextVariable()
    {
        return $"t{nextTag++}";
    }

    public static void Reset()
    {
        nextTag = 1;
    }
}
