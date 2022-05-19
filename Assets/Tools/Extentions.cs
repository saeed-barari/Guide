using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Tools
{
    public static class Extentions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            if(list.Count <= 0)
                throw new Exception("List is empty");
            return list[Random.Range(0, list.Count)];
        }

        public static IEnumerable<TSource> Foreach<TSource>(this IEnumerable<TSource> enumerable, Action<TSource> action)
        {
            foreach (var item in enumerable) action(item);
            return enumerable;
        }
    }
}