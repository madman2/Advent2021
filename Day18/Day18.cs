using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Day18 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private const int MaxLevels = 4;
        private const int MaxDataValue = 9;

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);

            PairNode root = null;
            foreach (var line in lines)
            {
                if (root == null)
                    root = BuildTree(line);
                else
                    root = new PairNode(root, BuildTree(line));

                while (Reduce(root));
            }

            return Magnitude(root).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);

            int max = int.MinValue;
            for (int i = 0; i < lines.Count; ++i)
                for (int j = 0; j < lines.Count; ++j)
                {
                    if (i == j)
                        continue;

                    var newRoot = new PairNode(BuildTree(lines[i]), BuildTree(lines[j]));
                    while (Reduce(newRoot));
                    max = Math.Max(max, Magnitude(newRoot));
                }

            return max.ToString();
        }

        private int Magnitude (PairNode root)
        {
            if (root.Data >= 0)
                return root.Data;

            var leftMagnitude = (root.Left == null) ? 0 : 3 * Magnitude(root.Left);
            var rightMagnitude = (root.Right == null) ? 0 : 2 * Magnitude(root.Right);
            return leftMagnitude + rightMagnitude;
        }

        private bool Reduce(PairNode root)
        {
            var s = new Stack<(PairNode, int)>();
            s.Push((root, 0));

            var preOrderTraversal = new List<PairNode>();
            var explodingPairIndex = -1;
            var splitPairIndex = -1;

            while (s.Count > 0)
            {
                var (current, level) = s.Pop();
                if (current.Data >= 0)
                {
                    preOrderTraversal.Add(current);
                    if (current.Data > MaxDataValue && splitPairIndex < 0)
                        splitPairIndex = preOrderTraversal.Count - 1;
                }
                else
                {
                    if (level == MaxLevels)
                    {
                        if (explodingPairIndex < 0)
                        {
                            preOrderTraversal.Add(current);
                            explodingPairIndex = preOrderTraversal.Count - 1;
                            continue;
                        }
                    }
                }

                if (current.Right != null)
                    s.Push((current.Right, level + 1));

                if (current.Left != null)
                    s.Push((current.Left, level + 1));
            }

            if (explodingPairIndex >= 0)
            {
                ExplodePair(preOrderTraversal, explodingPairIndex);
                return true;
            }
            else if (splitPairIndex >= 0)
            {
                SplitPair(preOrderTraversal[splitPairIndex]);
                return true;
            }
            return false;
        }

        private void ExplodePair(List<PairNode> nodes, int index)
        {
            var nodeToExplode = nodes[index];
            if (index > 0)
            {
                nodes[index - 1].Data += nodeToExplode.Data;
            }
            if (index < nodes.Count - 1)
            {
                nodes[index + 1].Data += nodeToExplode.Data;
            }
            (nodeToExplode.Left, nodeToExplode.Right) = (null, null);
            nodeToExplode.Data = 0;
        }

        private void SplitPair(PairNode pair)
        {
            pair.Left = new PairNode(pair.Data / 2);
            pair.Right = new PairNode((pair.Data + 1) / 2);
            pair.Data = -1;
        }

        private PairNode BuildTree(string pairs)
        {
            if (pairs.Length == 1)
                return new PairNode(int.Parse(pairs));

            var unclosedBraces = 0;
            int pivot;
            for (pivot = 1; pivot < pairs.Length - 1; ++pivot)
            {
                var c = pairs[pivot];
                if (c == '[')
                    unclosedBraces++;
                else if (c == ']')
                    unclosedBraces--;
                else if (c == ',' && unclosedBraces == 0)
                    break;
            }

            var leftNode = BuildTree(pairs.Substring(1, pivot - 1));
            var rightNode = BuildTree(pairs.Substring(pivot + 1, pairs.Length - pivot - 2));

            return new PairNode(leftNode, rightNode);
        }

        private class PairNode
        {
            public PairNode Left { get; set; } = null;

            public PairNode Right { get; set; } = null;

            public int Data { get; set; } = -1;

            public PairNode Parent { get; set; } = null;

            public PairNode(int data) => Data = data;
            public PairNode(PairNode left, PairNode right) => (Left, Right) = (left, right);
        }
    }
}
