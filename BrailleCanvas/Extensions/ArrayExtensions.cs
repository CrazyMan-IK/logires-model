namespace BrailleCanvas.Extensions;

public static class ArrayExtensions
{
    public static IEnumerable<T> GetRow<T>(this T[,] matrix, int rowIndex)
    {
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            yield return matrix[rowIndex, i];
        }
    }

    public static IEnumerable<T[]> GetRows<T>(this T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            yield return matrix.GetRow(i).ToArray();
        }
    }
}