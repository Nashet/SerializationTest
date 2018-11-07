using System.Collections.Generic;

namespace UnitTests
{
    /// <summary>
    /// used in unit tests
    /// </summary>
    internal static class ListRandExtensions
    {
        internal static bool IsSame(ListRand a, ListRand b)
        {
            if (a.Count != b.Count)
                return false;
            if (a.Tail != null)
                if (a.Tail.Data != b.Tail.Data)
                    return false;

            if (a.Head != null)
                if (a.Head.Data != b.Head.Data)
                    return false;

            var listA = a.ToList();
            var listB = b.ToList();

            for (int i = 0; i < listA.Count; i++)
            {
                if (!IsSame(listA[i], listB[i]))
                    return false;
            }

            return true;
        }

        private static List<ListNode> ToList(this ListRand source)
        {
            var res = new List<ListNode>();

            ListNode nextNode = source.Head;
            while (nextNode != null)
            {
                res.Add(nextNode);
                nextNode = nextNode.Next;
            }
            return res;
        }

        internal static bool IsSame(ListNode a, ListNode b)
        {
            if (a.Data != b.Data)
                return false;

            if (a.Next == null ^ b.Next == null)
                return false;
            if (a.Rand == null ^ b.Rand == null)
                return false;
            if (a.Prev == null ^ b.Prev == null)
                return false;

            if (a.Next != null)
                if (a.Next.Data != b.Next.Data)
                    return false;

            if (a.Prev != null)
                if (a.Prev.Data != b.Prev.Data)
                    return false;

            if (a.Rand != null)
                if (a.Rand.Data != b.Rand.Data)
                    return false;

            return true;
        }
    }
}
