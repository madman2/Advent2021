using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public static class LinkedListExtensions
    {
        public static T RemoveAndReturnMinBy<T, TKey>(this LinkedList<T> linkedList, Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            var current = linkedList.First;
            if (current == null)
                return default(T);

            var minValSeen = keySelector(current.Value);
            var minNode = linkedList.First;
            var comparerToUse = (comparer == null) ? Comparer<TKey>.Default : comparer;
            while (current.Next != null)
            {
                current = current.Next;
                var currentVal = keySelector(current.Value);
                if (comparerToUse.Compare(currentVal, minValSeen) < 0)
                {
                    minValSeen = currentVal;
                    minNode = current;
                }
            }
            linkedList.Remove(minNode);
            return minNode.Value;
        }
    }
}
