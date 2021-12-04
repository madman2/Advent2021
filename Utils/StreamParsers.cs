using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Utils
{
    public static class StreamParsers
    {
        public static List<string> GetStreamAsStringList(StreamReader reader)
        {
            var listToReturn = new List<string>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                listToReturn.Add(line);
            }

            return listToReturn;
        }

        public static List<int> GetStreamAsIntList(StreamReader reader, int fromBase = 10)
        {
            var listToReturn = new List<int>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var number = Convert.ToInt32(line, fromBase);
                listToReturn.Add(number);
            }

            return listToReturn;
        }

        public static List<long> GetStreamAsLongIntList(StreamReader reader, int fromBase = 10)
        {
            var listToReturn = new List<long>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var number = Convert.ToInt64(line, fromBase);
                listToReturn.Add(number);
            }

            return listToReturn;
        }

        public static List<IList<string>> GetStreamAsListOfDelimitedStrings(StreamReader reader, char delimiter = ' ')
        {
            var listToReturn = new List<IList<string>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoStringList(line, delimiter));
            }

            return listToReturn;
        }

        public static List<IList<int>> GetStreamAsListOfDelimitedInts(StreamReader reader, char delimiter = ' ', int fromBase = 10)
        {
            var listToReturn = new List<IList<int>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoIntList(line, delimiter, fromBase));
            }

            return listToReturn;
        }

        public static List<IList<long>> GetStreamAsListOfDelimitedLongInts(StreamReader reader, char delimiter = ' ', int fromBase = 10)
        {
            var listToReturn = new List<IList<long>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                listToReturn.Add(StringParsers.SplitDelimitedStringIntoLongIntList(line, delimiter, fromBase));
            }

            return listToReturn;
        }

        public static char[][] GetStreamAs2DCharArray(StreamReader reader)
        {
            var stringList = GetStreamAsStringList(reader);
            var arrayToReturn = new char[stringList.Count][];

            for (int i = 0; i < stringList.Count; ++i)
            {
                arrayToReturn[i] = stringList[i].ToArray();
            }

            return arrayToReturn;
        }

        public static int[][] GetStreamAs2DIntArray(StreamReader reader, char delimiter = ' ', int fromBase = 10)
        {
            var stringList = GetStreamAsStringList(reader);
            var arrayToReturn = new int[stringList.Count][];

            for (int i = 0; i < stringList.Count; ++i)
            {
                arrayToReturn[i] = StringParsers.SplitDelimitedStringIntoIntList(stringList[i], delimiter, fromBase).ToArray();
            }

            return arrayToReturn;
        }

        public static int[][] GetStreamAs2DBitArray(StreamReader reader, char oneRepresentation = '#')
        {
            var stringList = GetStreamAsStringList(reader);
            var arrayToReturn = new int[stringList.Count][];

            for (int i = 0; i < stringList.Count; ++i)
            {
                arrayToReturn[i] = stringList[i].Select(x => (x == oneRepresentation) ? 1 : 0).ToArray();
            }

            return arrayToReturn;
        }

        public static List<List<char>> GetStreamAs2DCharList(StreamReader reader)
        {
            var listToReturn = new List<List<char>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                listToReturn.Add(line.ToList());
            }

            return listToReturn;
        }

        public static List<List<int>> GetStreamAs2DIntList(StreamReader reader, char delimiter = ' ', int fromBase = 10)
        {
            var listToReturn = new List<List<int>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var lineAsList = StringParsers.SplitDelimitedStringIntoIntList(line, delimiter, fromBase);
                listToReturn.Add(lineAsList.Select(x => Convert.ToInt32(x)).ToList());
            }

            return listToReturn;
        }

        public static List<List<int>> GetStreamAs2DBitList(StreamReader reader, char oneRepresentation = '#')
        {
            var listToReturn = new List<List<int>>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                listToReturn.Add(line.Select(x => x == oneRepresentation ? 1 : 0).ToList());
            }

            return listToReturn;
        }

        public static IDictionary<string, string> GetStreamAsStringDict(StreamReader reader, char delimiter)
        {
            IDictionary<string, string> dictToReturn = new Dictionary<string, string>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var keyValuePairList = new List<string> { line };
                if (delimiter != ' ')
                {
                    keyValuePairList = StringParsers.SplitDelimitedStringIntoStringList(line);
                }
                foreach (var keyValuePairString in keyValuePairList)
                {
                    dictToReturn.Add(StringParsers.SplitDelimitedStringIntoStringKeyValuePair(keyValuePairString, delimiter));
                }
            }

            return dictToReturn;
        }

        public static IDictionary<string, int> GetStreamAsIntDict(StreamReader reader, char delimiter, int fromBase = 10)
        {
            IDictionary<string, int> dictToReturn = new Dictionary<string, int>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var keyValuePairList = new List<string> { line };
                if (delimiter != ' ')
                {
                    keyValuePairList = StringParsers.SplitDelimitedStringIntoStringList(line);
                }
                foreach (var keyValuePairString in keyValuePairList)
                {
                    dictToReturn.Add(StringParsers.SplitDelimitedStringIntoIntKeyValuePair(keyValuePairString, delimiter, fromBase));
                }
            }

            return dictToReturn;
        }
    }
}
