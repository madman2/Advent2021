using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day08 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        /*       2       */
        /*   ---------   */
        /*  |         |  */
        /* 1|         |3 */
        /*  |    6    |  */
        /*   ---------   */
        /*  |         |  */
        /* 0|         |4 */
        /*  |    5    |  */
        /*   ---------   */

        private static readonly List<HashSet<int>> SegmentLookup = new List<HashSet<int>> { 
            new HashSet<int> { 0, 1, 2, 3, 4, 5 },      // 0
            new HashSet<int> { 3, 4 },                  // 1
            new HashSet<int> { 0, 2, 3, 5, 6 },         // 2
            new HashSet<int> { 2, 3, 4, 5, 6 },         // 3
            new HashSet<int> { 1, 3, 4, 6 },            // 4
            new HashSet<int> { 1, 2, 4, 5, 6 },         // 5
            new HashSet<int> { 0, 1, 2, 4, 5, 6 },      // 6
            new HashSet<int> { 2, 3, 4 },               // 7
            new HashSet<int> { 0, 1, 2, 3, 4, 5, 6},    // 8
            new HashSet<int> { 1, 2, 3, 4, 5, 6}        // 9
        };

        public string SolveFirstStar(StreamReader reader)
        {
            string line;
            int count = 0;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var inputOutput = StringParsers.SplitDelimitedStringIntoStringList(line, '|');
                var outputs = StringParsers.SplitDelimitedStringIntoStringList(inputOutput[1]);
                count += outputs.Count(x => (x.Length != 5 && x.Length != 6));
            }

            return count.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            string line;
            int sum = 0;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var inputOutput = StringParsers.SplitDelimitedStringIntoStringList(line, '|');
                var inputList = StringParsers.SplitDelimitedStringIntoStringList(inputOutput[0]);
                var outputList = StringParsers.SplitDelimitedStringIntoStringList(inputOutput[1]);
                var inputSets = inputList.Select(x => new HashSet<char>(x)).ToList();

                sum += ComputeOutputValue(inputSets, outputList);
            }

            return sum.ToString();
        }

        private int ComputeOutputValue(List<HashSet<char>> inputs, List<string> outputs)
        {
            var decoder = new char[7];

            var oneDigit = inputs.First(x => x.Count == 2);
            var sevenDigit = inputs.First(x => x.Count == 3);
            var fourDigit = inputs.First(x => x.Count == 4);
            var eightDigit = inputs.First(x => x.Count == 7);

            // Top segment
            decoder[2] = sevenDigit.Except(oneDigit).First();

            // Segments 1 & 6, order unknown
            var topLeftAndMiddle = fourDigit.Except(oneDigit);

            var nineDigit = inputs.Where(x => (x.Count == 6 && x.IsSupersetOf(fourDigit) && x.Contains(decoder[2]))).First();
            inputs.Remove(nineDigit);

            // Bottom segment
            decoder[5] = nineDigit.Except(fourDigit).Where(x => x != decoder[2]).First();

            var zeroDigit = inputs.Where(x => x.Count == 6 && x.Intersect(topLeftAndMiddle).Count() == 1).First();
            inputs.Remove(zeroDigit);

            // Middle segment
            decoder[6] = eightDigit.Except(zeroDigit).First();
            // Top left segment
            decoder[1] = topLeftAndMiddle.Where(x => x != decoder[6]).First();
            // Bottom left segment
            decoder[0] = eightDigit.Except(nineDigit).First();

            var sixDigit = inputs.Where(x => x.Count == 6).First();

            // Top right segment
            decoder[3] = oneDigit.Except(sixDigit).First();
            // Bottom right segment
            decoder[4] = oneDigit.Where(x => x != decoder[3]).First();

            int decodedNumber = 0;
            foreach (var digit in outputs)
            {
                decodedNumber = 10 * decodedNumber + Decode(decoder, digit);
            }

            return decodedNumber;
        }

        private int Decode(char[] decoder, string code)
        {
            var decoderDict = new Dictionary<char, int>();
            for (int segment = 0; segment < decoder.Count(); ++segment)
            {
                decoderDict.Add(decoder[segment], segment);
            }

            var segments = new HashSet<int>();
            foreach (var c in code)
            {
                segments.Add(decoderDict[c]);
            }

            for (int i = 0; i < 10; ++i)
            {
                if (SegmentLookup[i].SetEquals(segments))
                    return i;
            }

            throw new Exception("Unable to decode this into a valid number");
        }
    }
}
