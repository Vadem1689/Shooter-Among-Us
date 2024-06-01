using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace BRAmongUS.Utils
{
    public static class MathExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRandomExcept<T>(this List<T> list, params T[] except)
        {
            T result = default;
            Random random = new Random();
            bool validResult = false;

            if (list.Count <= except.Length)
            {
                Debug.LogError(
                    $"Except list is bigger than the list of items to choose from. List count: [{list.Count}], except count: [{except.Length}]. Returned first item from the list.");
                return list[0];
            }

            while (!validResult)
            {
                result = list[random.Next(0, list.Count)];

                validResult = true;
                foreach (T item in except)
                {
                    if (result.Equals(item))
                    {
                        validResult = false;
                        break;
                    }
                }
            }
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FromPercent(this int value, in int percents)
        {
            return (value / 100) * percents;
        }
    }
}