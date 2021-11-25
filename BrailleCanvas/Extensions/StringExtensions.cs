using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class StringExtensions
{
    public static string ReplaceAt(this string value, int index, string substring)
    {
        return value.Remove(index, 1).Insert(index, substring);
    }

    public static string ReplaceAt(this string value, int index, char chr)
    {
        return value.ReplaceAt(index, chr.ToString());
    }
}