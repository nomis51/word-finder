using System.Net;
using System.Runtime.InteropServices;

namespace WordFinder.Shared.Models;

public class Options
{
    #region Constants

    private const char EmptyCharacter = '-';

    #endregion

    #region Members

    private readonly string _letters;
    private readonly int _wordLength;
    private readonly string _template;

    #endregion

    #region Props

    public char[] Letters => _letters.ToCharArray();
    public int WordLength => _wordLength != 0 ? _wordLength : string.IsNullOrEmpty(_template) ? _letters.Length : _template.Length;

    public string TemplateStr => _template;
    public IEnumerable<Tuple<int, char>>? Template
    {
        get
        {
            for (var i = 0; i < _template.Length; ++i)
            {
                if (_template[i] == EmptyCharacter) continue;
                yield return new Tuple<int, char>(i, _template[i]);
            }
        }
    }

    #endregion

    #region Constructors

    public Options(string letters, int wordLength, string template)
    {
        _letters = letters;
        _wordLength = wordLength;
        _template = template;
    }

    #endregion
}