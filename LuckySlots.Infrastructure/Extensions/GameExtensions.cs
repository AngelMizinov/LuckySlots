namespace LuckySlots.Infrastructure.Extensions
{
    public static class GameExtensions
    {
        public static T[] GetRow<T>(this T[,] matrix, int row)
        {
            var length = matrix.GetLength(1);
            var array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = matrix[row, i];
            }

            return array;
        }
    }
}
