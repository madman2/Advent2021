using AdventOfCode.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class Day16 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private int _packetVersionSum;

        private enum Operators
        {
            Sum = 0,
            Product = 1,
            Min = 2,
            Max = 3,
            Greater = 5,
            Less = 6,
            Equal = 7
        };

        public string SolveFirstStar(StreamReader reader)
        {
            return Solve(reader, 1);
        }

        public string SolveSecondStar(StreamReader reader)
        {
            return Solve(reader, 2);
        }

        private string Solve(StreamReader reader, int nStar)
        {
            var hexString = reader.ReadLine();
            var binaryString = string.Join(
                string.Empty,
                hexString.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))
            );

            int endIndex;
            var packetEval = EvaluatePacket(binaryString, 0, out endIndex);
            return ((nStar == 1) ? _packetVersionSum : packetEval).ToString();
        }

        private long EvaluatePacket(string binaryString, int startIndex, out int currentIndex)
        {
            currentIndex = startIndex;

            var packetVersionStr = binaryString.Substring(currentIndex, 3);
            _packetVersionSum += Convert.ToInt32(packetVersionStr, 2);
            currentIndex += 3;

            var typeIdStr = binaryString.Substring(currentIndex, 3);
            var typeId = Convert.ToInt32(typeIdStr, 2);
            currentIndex += 3;

            if (typeId == 4)
                return EvaluateLiteralPacket(binaryString, currentIndex, out currentIndex);

            return EvaluateOperatorPacket(binaryString, currentIndex, (Operators)typeId, out currentIndex);
        }

        private long EvaluateLiteralPacket(string binaryString, int startIndex, out int currentIndex)
        {
            currentIndex = startIndex;
            char prefix;
            var sb = new StringBuilder();
            do
            {
                prefix = binaryString[currentIndex];
                sb.Append(binaryString.Substring(currentIndex + 1, 4));
                currentIndex += 5;
            } while (prefix == '1');

            return Convert.ToInt64(sb.ToString(), 2);
        }

        private long EvaluateOperatorPacket(string binaryString, int startIndex, Operators op, out int currentIndex)
        {
            currentIndex = startIndex;
            var lengthTypeId = binaryString[currentIndex];
            currentIndex++;

            var packetEvals = new List<long>();
            var numSubPackets = int.MaxValue;
            var endIndex = int.MaxValue;
            if (lengthTypeId == '1')
            {
                numSubPackets = Convert.ToInt32(binaryString.Substring(currentIndex, 11), 2);
                currentIndex += 11;
            }
            else
            {
                var subPacketBitLength = Convert.ToInt32(binaryString.Substring(currentIndex, 15), 2);
                currentIndex += 15;
                endIndex = currentIndex + subPacketBitLength;
            }

            for (int i = 0; currentIndex < endIndex && i < numSubPackets; ++i)
                packetEvals.Add(EvaluatePacket(binaryString, currentIndex, out currentIndex));

            switch (op)
            {
                case Operators.Sum:
                    return packetEvals.Sum();
                case Operators.Product:
                    return packetEvals.Aggregate(1, (long total, long next) => total * next);
                case Operators.Min:
                    return packetEvals.Min();
                case Operators.Max:
                    return packetEvals.Max();
                case Operators.Greater:
                    return (packetEvals[0] > packetEvals[1]) ? 1 : 0;
                case Operators.Less:
                    return (packetEvals[0] < packetEvals[1]) ? 1 : 0;
                case Operators.Equal:
                    return (packetEvals[0] == packetEvals[1]) ? 1 : 0;
                default:
                    throw new Exception($"Invalid operator: {(int)op}");
            }
        }
    }
}
