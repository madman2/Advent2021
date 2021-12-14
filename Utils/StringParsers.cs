using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utils
{
    public static class StringParsers
    {
        public static List<string> SplitDelimitedStringIntoStringList(string stringToSplit, string delimiter = " ")
        {
            return stringToSplit.Trim().Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public static List<int> SplitDelimitedStringIntoIntList(string stringToSplit, string delimiter = " ", int fromBase = 10)
        {
            return stringToSplit.Trim().Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x, fromBase))
                .ToList();
        }

        public static List<long> SplitDelimitedStringIntoLongIntList(string stringToSplit, string delimiter = " ", int fromBase = 10)
        {
            return stringToSplit.Trim().Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt64(x, fromBase))
                .ToList();
        }

        public static KeyValuePair<string, string> SplitDelimitedStringIntoStringKeyValuePair(string stringToSplit, string delimiter)
        {
            var keyValuePairList = stringToSplit.Trim().Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            return new KeyValuePair<string, string>(keyValuePairList[0], keyValuePairList[1]);
        }

        public static KeyValuePair<string, int> SplitDelimitedStringIntoIntKeyValuePair(string stringToSplit, string delimiter, int fromBase = 10)
        {
            var keyValuePairList = stringToSplit.Trim().Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            return new KeyValuePair<string, int>(keyValuePairList[0], Convert.ToInt32(keyValuePairList[1], fromBase));
        }

        public static IDictionary<string, string> SplitDelimitedStringIntoStringDict(string stringToSplit, char keyValuePairDelimiter, string entryDelimiter = " ")
        {
            IDictionary<string, string> dictToReturn = new Dictionary<string, string>();
            var splitString = SplitDelimitedStringIntoStringList(stringToSplit, entryDelimiter);
            foreach (var keyValuePairString in splitString)
            {
                dictToReturn.Add(SplitDelimitedStringIntoStringKeyValuePair(keyValuePairString, keyValuePairDelimiter.ToString()));
            }

            return dictToReturn;
        }

        public static IDictionary<string, int> SplitDelimitedStringIntoIntDict(string stringToSplit, char keyValuePairDelimiter, string entryDelimiter = " ", int fromBase = 10)
        {
            IDictionary<string, int> dictToReturn = new Dictionary<string, int>();
            var splitString = SplitDelimitedStringIntoStringList(stringToSplit, entryDelimiter);
            foreach (var keyValuePairString in splitString)
            {
                dictToReturn.Add(SplitDelimitedStringIntoIntKeyValuePair(keyValuePairString, keyValuePairDelimiter.ToString()));
            }

            return dictToReturn;
        }
    }
}
