
namespace JoCodeCompiler;

public class Program
{
    public static void Main(string[] args)
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        var files = dir.GetFiles("*.src");

        if (files.Length == 0)
        {
            Console.WriteLine("No source file found.");
            return;
        }

        foreach (var file in files)
        {
            Console.WriteLine($"Reading from file {file}");
            LexicalAnalizer.OpenFile(file.FullName);

            while (LexicalAnalizer.NextToken() != null) { };
        }
    }
}
