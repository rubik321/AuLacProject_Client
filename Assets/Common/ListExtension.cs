using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Rubik.Common
{
    public static class ListExtension
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            // Random rng = new Random(DateTime.UtcNow.Millisecond);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RubikRandomHelper.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            // Random rng = new Random(DateTime.UtcNow.Millisecond);
            // var index = rng.Next(list.Count);
            return list[RubikRandomHelper.Next(list.Count)];
        }

        public static T Front<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T Back<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static T RemoveFront<T>(this IList<T> list)
        {
            T front = list.Front();
            list.RemoveAt(0);
            return front;
        }

        public static T RemoveBack<T>(this IList<T> list)
        {
            T back = list.Back();
            list.RemoveAt(list.Count - 1);
            return back;
        }

        public static void AddFront<T>(this IList<T> list, T item)
        {
            list.Insert(0, item);
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }

        public static bool IsEmpty<T>(this Queue<T> queue)
        {
            return queue.Count == 0;
        }

        public static bool IsEmpty<T>(this Stack<T> stack)
        {
            return stack.Count == 0;
        }




        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

    }
}