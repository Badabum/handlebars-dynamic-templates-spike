using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Spikes.Handlebars
{
    public static class EnumerableExtensions
    {
        public static List<ImmutableList<T>> Split<T>(this IEnumerable<T> enumerable, int size)
        {
            var list = enumerable.ToList();
            var chunks = new List<ImmutableList<T>>();
            int chunkCount = list.Count / size;

            if (list.Count % size > 0)
            {
                chunkCount++;
            }

            for (var chunkNumber = 0; chunkNumber < chunkCount; chunkNumber++)
            {
                chunks.Add(list.Skip(chunkNumber * size).Take(size).ToImmutableList());
            }

            return chunks;
        }
    }
}