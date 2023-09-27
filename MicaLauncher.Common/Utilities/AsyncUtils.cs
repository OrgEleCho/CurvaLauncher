namespace MicaLauncher.Utilities
{
    public static class AsyncUtils
    {
        public static async IAsyncEnumerable<T> EnumerableToAsync<T>(IEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();

            while (true)
            {
                bool hasNext = await Task.Run(enumerator.MoveNext);
                //bool hasNext = enumerator.MoveNext();

                if (!hasNext)
                    yield break;

                yield return enumerator.Current;
            }
        }
    }
}
