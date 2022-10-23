using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace BrewedInk.CardCheat.Core
{
    public static class Extensions
    {
        public static IEnumerable<Vector2Int> GetNeighbors(this Vector2Int self)
        {
            yield return self + Vector2Int.down;
            yield return self + Vector2Int.left;
            yield return self + Vector2Int.right;
            yield return self + Vector2Int.up;
        }
        
        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}