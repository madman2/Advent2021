using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day03 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private enum RateConstants
        {
            Gamma,
            Epsilon
        }

        private enum RatingType
        {
            Oxygen,
            CO2
        }

        public string SolveFirstStar(StreamReader reader)
        {
            var bitStrings = StreamParsers.GetStreamAsStringList(reader);

            var countOnes = new Dictionary<int, int>();
            for (int i = 0; i < bitStrings[0].Length; i++)
            {
                countOnes[i] = bitStrings.Count(x => x[i] == '1');
            }

            var gamma = ComputeRateConstant(countOnes, (bitStrings.Count + 1) / 2, RateConstants.Gamma);
            var epsilon = ComputeRateConstant(countOnes, (bitStrings.Count + 1) / 2, RateConstants.Epsilon);

            return (gamma * epsilon).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var bitStrings = StreamParsers.GetStreamAsStringList(reader);

            var oxgenRating = ComputeRatingValue(new List<string>(bitStrings.ToList()), RatingType.Oxygen);
            var co2Rating = ComputeRatingValue(new List<string>(bitStrings.ToList()), RatingType.CO2);

            return (oxgenRating * co2Rating).ToString();
        }

        private int ComputeRateConstant(IDictionary<int, int> countOnes, int quorum, RateConstants rateConstant)
        {
            var rateValue = "";
            for (int i = 0; i < countOnes.Count; ++i)
            {
                rateValue += ((rateConstant == RateConstants.Epsilon) ^ countOnes[i] >= quorum) ? '1' : '0';
            }

            return Convert.ToInt32(rateValue, 2);
        }

        private int ComputeRatingValue(List<string> numbers, RatingType type)
        {
            int index = 0;
            while (numbers.Count > 1 && index < numbers[0].Length)
            {
                var ones = numbers.Count(x => x[index] == '1');
                var zeroes = numbers.Count - ones;

                char target = (type == RatingType.Oxygen ^ zeroes > ones) ? '1' : '0';

                numbers = numbers.Where(x => x[index] == target).ToList();
                index++;
            }

            return Convert.ToInt32(numbers[0], 2);
        }
    }
}
