using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System.IO;

namespace AdventOfCode
{
    public class Day25 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        public string SolveFirstStar(StreamReader reader)
        {
            var map = new OceanMap(StreamParsers.GetStreamAs2DCharArray(reader));

            var steps = 0;
            do
            {
                steps++;
            } while (map.Step() > 0);

            return steps.ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            return "Merry Christmas!";  // Free star
        }

        private class OceanMap
        {
            private char[][] _map;
            public OceanMap(char[][] map) => _map = map;

            public int Step()
            {
                return MoveEast() + MoveSouth();
            }

            private int MoveEast()
            {
                var moved = 0;
                for (int i = 0; i < _map.Length; i++)
                {
                    var firstSpaceBlocked = _map[i][0] != '.';
                    for (int j = 0; j < _map[i].Length; ++j)
                    {
                        if (GetCharAt(i, j) == '>' && GetCharAt(i, j + 1) == '.')
                        {
                            if (j == _map[0].Length - 1 && firstSpaceBlocked)
                                continue;

                            SetCharAt(i, j++, '.');
                            SetCharAt(i, j, '>');
                            moved++;
                        }
                    }
                }

                return moved;
            }

            private int MoveSouth()
            {
                var moved = 0;
                for (int j = 0; j < _map[0].Length; ++j)
                {
                    var firstSpaceBlocked = _map[0][j] != '.';
                    for (int i = 0; i < _map.Length; ++i)
                    {
                        if (GetCharAt(i, j) == 'v' && GetCharAt(i + 1, j) == '.')
                        {
                            if (i == _map.Length - 1 && firstSpaceBlocked)
                                continue;

                            SetCharAt(i++, j, '.');
                            SetCharAt(i, j, 'v');
                            moved++;
                        }
                    }
                }

                return moved;
            }

            private char GetCharAt(int row, int col)
            {
                return _map[row % _map.Length][col % _map[0].Length];
            }

            private void SetCharAt(int row, int col, char c)
            {
                _map[row % _map.Length][col % _map[0].Length] = c;
            }
        }
    }
}
