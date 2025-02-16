
namespace SyntacticAnalyzer;

public class ParsingTable
{
    private static Stack<string> stack = new Stack<string>();
    private static Dictionary<(int, int), string> parsingTable = [];

    public static void BuildParsingTable()
    {
        GrammarSymbols.BuildDictionary();

        stack.Push(GrammarSymbols.END);
        stack.Push(GrammarSymbols.START);

        var parsingTableFilePath = Path.Combine(Directory.GetCurrentDirectory(), "parsingtable.tsv");

        var sr = new StreamReader(parsingTableFilePath);
        var (row, col) = (0, 0);

        sr.ReadLine(); // Skip first row (headers)

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();

            if (line == null)
            {
                break;
            }

            var splitLine = line.Split('\t');

            for (int i = 1; i < splitLine?.Length; i++)
            {
                if (splitLine[i] != "")
                {
                    parsingTable[(row, col)] = TranslateToGrammarSymbols(splitLine[i]);
                }
                col++;
            }

            row++;
            col = 0;
        }
    }

    public static bool EndOfProgramReached()
    {
        return stack.Peek() == GrammarSymbols.END;
    }

    public static void Derive(string thisToken)
    {
        string topElement;

        do
        {
            topElement = stack.Peek();
            if (GrammarSymbols.TERMINALS.Contains(topElement))
            {
                if (topElement == thisToken)
                {
                    stack.Pop();
                    return;
                }
            }
            else
            {
                if (parsingTable.TryGetValue((
                    Array.IndexOf(GrammarSymbols.NONTERMINALS, topElement),
                    Array.IndexOf(GrammarSymbols.TERMINALS, thisToken)
                    ), out var production))
                {
                    stack.Pop();
                    var toPush = production.Split(" ").Reverse().ToList();
                    foreach (var word in toPush)
                    {
                        if (word.Equals("EPSILON"))
                        {
                            continue;
                        }

                        stack.Push(word);
                    }

                    Console.WriteLine(topElement + " -> " + production.ToString());
                }

            }
        } while (!GrammarSymbols.TERMINALS.Contains(topElement));
    }

    private static string TranslateToGrammarSymbols(string text)
    {
        text = text.ToUpper();
        var splitText = text.Split(' ');
        var translated = "";

        foreach (var word in splitText)
        {
            var append = GrammarSymbols.lexemeToType.TryGetValue(word, out string? value) ? value : word;
            translated += (translated == "") ? append : " " + append;
        }

        return translated;
    }
}
