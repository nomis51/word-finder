namespace WordFinder.Shared.Extensions;

public static class StringExtensions
{
    #region Public methods

    public static IEnumerable<string> ToPermutations(this IEnumerable<char> value)
    {
        List<string> results = new();
        Permute(results, string.Empty, value.ToList());
        return results;
    }
    
    #endregion
    
    #region Private methods

    private static void ArraySwap<T>(IList<T> array, int i, int j)
    {
        if (i == j || i > array.Count || j > array.Count || i < 0 || j < 0) return;
        (array[i], array[j]) = (array[j], array[i]);
    }

    private static void PermuteRec(ICollection<string> result, string str, IList<char> letters)
    {
        if (letters.Count == 0)
        {
            result.Add(str);
        }
        else
        {
            for (var i = 0; i < letters.Count; ++i)
            {
                ArraySwap(letters, 0, i);
                PermuteRec(result, str + letters.First(), letters.Skip(1).ToList());
                ArraySwap(letters, 0, i);
            }
        }
    }

    private static void Permute(ICollection<string> result, string str, List<char> letters)
    {
        while (true)
        {
            if (letters.Count == 0)
            {
                if (str.Length > 0) PermuteRec(result, string.Empty, str.ToCharArray().ToList());
                return;
            }

            Permute(result, str, letters.Skip(1).ToList());
            str += letters.First();
            letters = letters.Skip(1).ToList();
        }
    }

    #endregion
}