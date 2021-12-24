﻿using AdventOfCode.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode
{
    /*
     * The input is a set of instructions, I reverse-engineered them into the code in ExecuteProgram() below.
     * All the else {} cases were removed because it is impossible for Z == 0 at the end of execution if the conditions
     * of every single if () statement are not met.
     * 
     * I deduced the min/max values for each digit of the input that will allow ExecuteProgram() to return 0, see the comments.
     */
    public class Day24 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        /*
         * These are the independent digits and their valid ranges
         */
        private static readonly Dictionary<int, (int min, int max)> IndependentDigitRanges = new Dictionary<int, (int, int)>
        {
            {0, (8, 9) },
            {1, (1, 9) },
            {2, (9, 9) },
            {3, (1, 1) },
            {4, (4, 9) },
            {8, (1, 4) },
            {10, (1, 6) },
            {11, (7, 9) }
        };

        private static readonly Dictionary<int, (int pairedIndex, int offset)> DependentDigitMapping = new Dictionary<int, (int, int)>
        {
            {5, (4, -3) },
            {6, (1, 0) },
            {7, (0, -7) },
            {9, (8, 5) },
            {12, (11, -6) },
            {13, (10, 3) }
        };

        public string SolveFirstStar(StreamReader reader)
        {
            var maxValidModelNumber = BuildModelNumber(true);
            Debug.Assert(ExecuteProgram(maxValidModelNumber) == 0);

            return string.Concat(maxValidModelNumber);
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var minValidModelNumber = BuildModelNumber(false);
            Debug.Assert(ExecuteProgram(minValidModelNumber) == 0);

            return string.Concat(minValidModelNumber);
        }

        private int[] BuildModelNumber(bool max)
        {
            var modelNumber = new int[14];
            for (int i = 0; i < modelNumber.Length; ++i)
            {
                if (IndependentDigitRanges.ContainsKey(i))
                {
                    modelNumber[i] = max ? IndependentDigitRanges[i].max : IndependentDigitRanges[i].min;
                }
                else
                {
                    var (pairedIndex, offset) = DependentDigitMapping[i];
                    modelNumber[i] = modelNumber[pairedIndex] + offset;
                }
            }

            return modelNumber;
        }

        private int ExecuteProgram(int[] inputs)
        {
            var w = inputs[0];
            var z = (w + 5);    // Z = w[0] + 5

            w = inputs[1];
            z *= 26;
            z += (w + 9);   // Z = 26 * w[0] + w[1] + 139

            w = inputs[2];
            z *= 26;
            z += (w + 4);   // Z = 26 * 26 * w[0] + 26 * w[1] + w[2] + 3618

            /* 
             * 13 <= Z % 26 <= 21
             * 13 <= (w[2] + 3618) % 26 <= 21
             * 
             * w[2] == 9
             * w[3] == 1
             */
            w = inputs[3];
            if (w == (z % 26 - 12))
                z /= 26;    // Z = 26 * w[0] + w[1] + 139

            w = inputs[4];
            z *= 26;
            z += (w + 10);  // Z = 26 * 26 * w[0] + 26 * w[1] + w[4] + 3624

            /* 
             * 14 <= Z % 26 <= 22
             * 14 <= (w[4] + 3624) % 26 <= 22
             * 
             * 4 <= w[4] <= 9
             * w[5] = w[4] - 3
             */
            w = inputs[5];
            if (w == (z % 26 - 13))
                z /= 26;    // Z = 26 * w[0] + w[1] + 139

            /* 
             * 10 <= Z % 26 <= 18
             * 10 <= (w[1] + 139) % 26 <= 18
             * 
             * 1 <= w[1] <= 9
             * w[6] = w[1]
             */
            w = inputs[6];
            if (w == (z % 26 - 9))
                z /= 26;    // Z = w[0] + 5

            /*
             * 13 <= Z % 26 <= 21
             * 13 <= (w[0] + 5) % 26 <= 21
             * 
             * 8 <= w[0] <= 9
             * w[7] = w[0] - 7
             */
            w = inputs[7];
            if (w == (z % 26 - 12))
                z /= 26;    // Z = 0

            w = inputs[8];
            z *= 26;
            z += (w + 14);  // Z = w[8] + 14

            /*
             * 10 <= Z % 26 <= 18
             * 10 <= (w[8] + 14) % 26 <= 18
             * 
             * 1 <= w[8] <= 4
             * w[9] = w[8] + 5
             */
            w = inputs[9];
            if (w == (z % 26 - 9))
                z /= 26;    // Z = 0

            w = inputs[10];
            z *= 26;
            z += (w + 5);   // Z = w[10] + 5

            w = inputs[11];
            z *= 26;
            z += (w + 10);  // Z = 26 * w[10] + w[11] + 140

            /*
             * 17 <= Z % 26 <= 25
             * 17 <= (w[11] + 140) % 26 <= 25
             * 
             * 7 <= w[11] <= 9
             * w[12] = w[11] - 6
             */
            w = inputs[12];
            if (w == (z % 26 - 16))
                z /= 26;    // Z = w[10] + 5

            /*
             * 3 <= Z % 26 <= 11
             * 3 <= (w[10] + 5) % 26 <= 11
             * 
             * 1 <= w[10] <= 6
             * w[13] = w[10] + 3
             */
            w = inputs[13];
            if (w == (z % 26 - 2))
                z /= 26;    // Z = 0

            return z;
        }
    }
}
