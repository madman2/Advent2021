using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day08 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            string line;
            int count = 0;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var inputOutput = StringParsers.SplitDelimitedStringIntoStringList(line, "|");
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
                var inputOutput = StringParsers.SplitDelimitedStringIntoStringList(line, "|");
                var inputList = StringParsers.SplitDelimitedStringIntoStringList(inputOutput[0]);
                var outputList = StringParsers.SplitDelimitedStringIntoStringList(inputOutput[1]);
                var inputSorted = inputList.Select(x => string.Concat(x.OrderBy(c => c))).ToList();
                var outputSorted = outputList.Select(x => string.Concat(x.OrderBy(c => c))).ToList();

                sum += Decode(inputSorted, outputSorted);
            }

            return sum.ToString();
        }

        private int Decode(List<string> inputs, List<string> outputs)
        {
            /* Segments:     */
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

            var segments = new char[7];
            var sortedInputs = new string[inputs.Count()];

            for (int i = inputs.Count - 1; i >= 0; --i)
            {
                var inputCode = inputs[i];
                switch (inputCode.Length)
                {
                    case 2:
                        sortedInputs[1] = inputCode;
                        inputs.RemoveAt(i);
                        break;
                    case 3:
                        sortedInputs[7] = inputCode;
                        inputs.RemoveAt(i);
                        break;
                    case 4:
                        sortedInputs[4] = inputCode;
                        inputs.RemoveAt(i);
                        break;
                    case 7:
                        sortedInputs[8] = inputCode;
                        inputs.RemoveAt(i);
                        break;
                }
            }

            // Top segment
            segments[2] = sortedInputs[7].Except(sortedInputs[1]).First();

            // Segments 1 & 6, order unknown
            var topLeftAndMiddle = sortedInputs[4].Except(sortedInputs[1]);

            var nineDigitIndex = inputs.FindIndex(x => (x.Length == 6 && sortedInputs[4].All(x.Contains) && x.Contains(segments[2])));
            sortedInputs[9] = inputs[nineDigitIndex];
            inputs.RemoveAt(nineDigitIndex);

            var zeroDigitIndex = inputs.FindIndex(x => x.Length == 6 && x.Intersect(topLeftAndMiddle).Count() == 1);
            sortedInputs[0] = inputs[zeroDigitIndex];
            inputs.RemoveAt(zeroDigitIndex);

            var sixDigitIndex = inputs.FindIndex(x => x.Length == 6);
            sortedInputs[6] = inputs[sixDigitIndex];
            inputs.RemoveAt(sixDigitIndex);

            // Top right segment
            segments[3] = sortedInputs[1].Except(sortedInputs[6]).First();

            var threeDigitIndex = inputs.FindIndex(x => sortedInputs[1].All(x.Contains));
            sortedInputs[3] = inputs[threeDigitIndex];
            inputs.RemoveAt(threeDigitIndex);

            var twoDigitIndex = inputs.FindIndex(x => x.Contains(segments[3]));
            sortedInputs[2] = inputs[twoDigitIndex];
            inputs.RemoveAt(twoDigitIndex);

            sortedInputs[5] = inputs.First();

            var segmentsToValue = new Dictionary<string, int>();
            for (int i = 0; i < sortedInputs.Length; ++i)
            {
                segmentsToValue.Add(sortedInputs[i], i);
            }

            int decodedNumber = 0;
            foreach (var digit in outputs)
            {
                decodedNumber = 10 * decodedNumber + segmentsToValue[digit];
            }

            return decodedNumber;
        }
    }
}
