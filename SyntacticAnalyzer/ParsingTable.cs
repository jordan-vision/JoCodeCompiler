
namespace SyntacticAnalyzer;

public class ParsingTable
{
    private static Stack<string> stack = new Stack<string>();
    private static Dictionary<(int, int), string> parsingTable = [];
    private static List<string> derivation = [GrammarSymbols.START];

    public static void BuildParsingTable()
    {
        ResetStack();
        GrammarSymbols.BuildDictionary();

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
                    // Apply production to stack
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

                    // Write derivation
                    var firstIndexOfReplacement = derivation.IndexOf(topElement);
                    var newDerivation = derivation.GetRange(0, firstIndexOfReplacement) ?? [];
                    newDerivation.AddRange(toPush.AsEnumerable().Reverse());
                    newDerivation.AddRange(derivation.GetRange(firstIndexOfReplacement + 1, derivation.Count - firstIndexOfReplacement - 1));
                    derivation = newDerivation;

                    string derivationString = "=> ";

                    foreach (var word in derivation)
                    {
                        if (word.Equals("EPSILON"))
                        {
                            continue;
                        }

                        derivationString += (derivationString == "=> ") ? word : " " + word;
                    }

                    Parser.WriteDerivation(derivationString);
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

    public static void ResetStack()
    {
        stack = new();

        stack.Push(GrammarSymbols.END);
        stack.Push(GrammarSymbols.START);

        derivation = [GrammarSymbols.START];
    }
}
