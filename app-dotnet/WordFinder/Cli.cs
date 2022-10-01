using System.Diagnostics;
using Newtonsoft.Json;
using WordFinder.Shared.Extensions;
using WordFinder.Shared.Models;

namespace WordFinder;

public class Cli
{
    #region Constants

    private const string FrenchWordsFilePath = "./assets/french-words.json";

    #endregion

    #region Members

    private Dictionary<string, byte> _frenchWords = new();

    #endregion

    #region Constructors

    public Cli()
    {
        Console.WriteLine("Loading french words...");
        LoadFrenchWords();
    }

    #endregion

    #region Public methods

    public void Execute()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Specify letters followed by the word length or a word template.");
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) continue;

            var args = line.Split(" ");

            var options = ParseArgs(args);

            Console.Clear();
            Console.WriteLine($"Letters: {string.Join("", options.Letters)}");
            Console.WriteLine($"Word length: {options.WordLength}");
            Console.WriteLine($"Word template: {options.TemplateStr}");
            Console.WriteLine();

            var words = FindWords(options)
                .ToList();

            Console.WriteLine();

            if (words.Count == 0)
            {
                Console.WriteLine("0 word found");
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            Console.WriteLine($"{words.Count} word{(words.Count > 1 ? 's' : string.Empty)} found: ");

            foreach (var word in words.OrderBy(f => f))
            {
                Console.WriteLine($"  {word}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    #endregion

    #region Private methods

    private void LoadFrenchWords()
    {
        var data = File.ReadAllText(FrenchWordsFilePath);
        var words = JsonConvert.DeserializeObject<IEnumerable<string>>(data);
        if (words is null) throw new NullReferenceException("Unable to acquire french words");

        foreach (var word in words)
        {
            _frenchWords.Add(word, 0);
        }
    }

    private IEnumerable<string> FindWords(Options options)
    {
        Console.WriteLine("Finding all possible permutations...");
        var timer = Stopwatch.StartNew();
        var possibleWords = options.Letters.ToPermutations();
        timer.Stop();
        Console.WriteLine($"Done in {timer.ElapsedMilliseconds}ms");

        Console.WriteLine("Searching for french words...");
        timer.Restart();
        List<string> foundWords = new();
        object foundWordsLock = new();

        Parallel.ForEach(possibleWords, possibleWord =>
        {
            if (possibleWord.Length != options.WordLength) return;

            if (options.Template != null)
            {
                var wordMatchTemplate = true;
                foreach (var (index, character) in options.Template)
                {
                    if (index < possibleWord.Length && possibleWord[index] == character) continue;

                    wordMatchTemplate = false;
                    break;
                }

                if (!wordMatchTemplate) return;
            }

            if (!_frenchWords.ContainsKey(possibleWord)) return;
            if (foundWords.Contains(possibleWord)) return;

            lock (foundWordsLock)
            {
                foundWords.Add(possibleWord);
            }
        });
        timer.Stop();
        Console.WriteLine($"Done in {timer.ElapsedMilliseconds}ms");

        return foundWords;
    }

    private Options ParseArgs(IReadOnlyList<string> args)
    {
        return int.TryParse(args[1], out var length) ? new Options(args[0], length, string.Empty) : new Options(args[0], 0, args[1]);
    }

    #endregion
}