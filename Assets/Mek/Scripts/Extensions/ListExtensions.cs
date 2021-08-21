using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace Mek.Extensions
{
    public static class ListExtensions
    {
        private static System.Random Random = new System.Random();

        public static T RandomElement<T>(this IList<T> list)
        {
            var index = Random.Next(0, list.Count);
            return list[index];
        }
        public static T First<T>(this IList<T> list)
        {
            return list[0];
        }

        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count - 1];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = Random.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }
        }

        public static List<T> DrawFirstXElement<T>(this List<T> list, int count)
        {
            List<T> newList = new List<T>();

            if (count > list.Count)
            {
                Debug.LogError($"Could not draw {count} element");
                return newList;
            }

            for (int i = 0; i < count; i++)
            {
                newList.Add(list[i]);
            }

            return newList;
        }
    }
}
