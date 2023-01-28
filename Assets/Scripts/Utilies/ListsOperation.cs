using System;
using System.Collections.Generic;

namespace Utilies
{
    public class ListsOperation
    {
        private static readonly Random _rng = new();  

        public static void Shuffle<T>(IList<T> list)  
        {  
            var n = list.Count;  
            while (n > 1) {  
                n--;  
                var k = _rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}