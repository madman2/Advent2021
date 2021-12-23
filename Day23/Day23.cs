using AdventOfCode.Interfaces;
using AdventOfCode.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace AdventOfCode
{
    public class Day23 : ISolver
    {
        public string InputFileName { get; } = "input.txt";

        private static readonly int[] RoomColumns = new int[4] { 3, 5, 7, 9 };
        private static readonly Dictionary<char, int> CostBySpecies = new Dictionary<char, int>
        {
            { 'A', 1 },
            { 'B', 10 },
            { 'C', 100 },
            { 'D', 1000 }
        };

        public string SolveFirstStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var burrow = ParseBurrow(lines);
            return FindMinCost(burrow, burrow with { Rooms = string.Concat(burrow.Rooms.OrderBy(x => x)) }).ToString();
        }

        public string SolveSecondStar(StreamReader reader)
        {
            var lines = StreamParsers.GetStreamAsStringList(reader);
            var additionalLines = new List<string>
            {
                "  #D#C#B#A#",
                "  #D#B#A#C#"
            };
            var burrow = ParseBurrow(lines, additionalLines, 3);
            return FindMinCost(burrow, burrow with { Rooms = string.Concat(burrow.Rooms.OrderBy(x => x)) }).ToString();
        }

        private long FindMinCost(BurrowState start, BurrowState end)
        {
            var minCostLookup = new Dictionary<BurrowState, long>();
            var q = new PriorityQueue<BurrowState, long>();

            minCostLookup.Add(start, 0);
            q.Enqueue(start, 0);

            while (q.Count > 0)
            {
                var state = q.Dequeue();
                var minCost = minCostLookup[state];

                foreach (var (newState, cost) in state.EnumerateMoves())
                {
                    if (minCostLookup.ContainsKey(newState))
                    {
                        if (minCostLookup[newState] > minCost + cost)
                            minCostLookup[newState] = minCost + cost;
                        else
                            continue;
                    }
                    else
                    {
                        minCostLookup.Add(newState, minCost + cost);
                    }
                    q.Enqueue(newState, minCost + cost);
                }
            }

            return minCostLookup[end];
        }

        private BurrowState ParseBurrow(List<string> lines, List<string> extraLines = null, int insertionPoint = -1)
        {
            var hall = lines[1];
            var rooms = new string[4];

            for (int i = 2; i < lines.Count - 1; ++i)
            {
                if (i == insertionPoint)
                {
                    foreach (var extraLine in extraLines)
                    {
                        for (int room = 0; room < RoomColumns.Length; ++room)
                        {
                            rooms[room] += extraLine[RoomColumns[room]];
                        }
                    }
                }
                for (int room = 0; room < RoomColumns.Length; ++room)
                {
                    rooms[room] += lines[i][RoomColumns[room]];
                }
            }

            return new BurrowState(String.Concat(rooms), hall);
        }

        record BurrowState(string Rooms, string Hall)
        {
            public int RoomCapacity { get; } = Rooms.Length / 4;

            public IEnumerable<(BurrowState state, long cost)> EnumerateMoves()
            {
                for (int roomNumber = 0; roomNumber < 4; ++roomNumber)
                {
                    var (species, roomPos) = GetRoomMover(roomNumber);
                    if (roomPos < 0)
                        continue;

                    foreach (var openSpot in EnumerateOpenHallSpots(roomNumber))
                    {
                        yield return LeaveRoom(species, roomNumber, roomPos, openSpot);
                    }

                    var homeRoomNumber = species - 'A';
                    if (roomNumber != homeRoomNumber && TryGetHome(species, RoomColumns[roomNumber], out var homeRoomPos))
                    {
                        yield return LeaveRoom(species, roomNumber, roomPos, homeRoomNumber, homeRoomPos);
                    }
                }

                for (int hallPos = 1; hallPos < Hall.Length - 1; ++hallPos)
                {
                    if (Hall[hallPos] == '.')
                        continue;

                    var species = Hall[hallPos];
                    var homeRoomNumber = species - 'A';
                    if (TryGetHome(species, hallPos, out var homeRoomPos))
                    {
                        yield return EnterRoom(species, homeRoomNumber, homeRoomPos, hallPos);
                    }
                }
            }

            private (char species, int roomPos) GetRoomMover(int roomNumber)
            {
                var roomStartIndex = roomNumber * RoomCapacity;
                var roomPos = 0;
                while (roomPos < RoomCapacity && Rooms[roomStartIndex + roomPos] == '.')
                {
                    roomPos++;
                }

                return (roomPos == RoomCapacity) ? ('.', -1) : (Rooms[roomStartIndex + roomPos], roomPos);
            }

            private (BurrowState state, long cost) LeaveRoom(char species, int roomNumber, int roomPos, int hallPos)
            {
                var cost = GetPathCost(species, roomNumber, roomPos, hallPos);

                var tempNewRoom = Rooms.ToCharArray();
                tempNewRoom[roomNumber * RoomCapacity + roomPos] = '.';
                var newRoom = new string(tempNewRoom);

                var tempNewHall = Hall.ToCharArray();
                tempNewHall[hallPos] = species;
                var newHall = new string(tempNewHall);

                return (new BurrowState(newRoom, newHall), cost);
            }

            private (BurrowState state, long cost) LeaveRoom(char species, int roomNumber, int roomPos, int homeRoomNumber, int homeRoomPos)
            {
                var cost = GetPathCost(species, roomNumber, roomPos, homeRoomNumber, homeRoomPos);

                var tempNewRoom = Rooms.ToCharArray();
                tempNewRoom[roomNumber * RoomCapacity + roomPos] = '.';
                tempNewRoom[homeRoomNumber * RoomCapacity + homeRoomPos] = species;
                var newRoom = new string(tempNewRoom);

                return (new BurrowState(newRoom, Hall), cost);
            }

            private (BurrowState state, long cost) EnterRoom(char species, int roomNumber, int roomPos, int hallPos)
            {
                var cost = GetPathCost(species, roomNumber, roomPos, hallPos);

                var tempNewRoom = Rooms.ToCharArray();
                tempNewRoom[roomNumber * RoomCapacity + roomPos] = species;
                var newRoom = new string(tempNewRoom);

                var tempNewHall = Hall.ToCharArray();
                tempNewHall[hallPos] = '.';
                var newHall = new string(tempNewHall);

                return (new BurrowState(newRoom, newHall), cost);
            }

            private IEnumerable<int> EnumerateOpenHallSpots(int roomNumber)
            {
                var roomColumn = RoomColumns[roomNumber];

                // Check open spots to the left
                for (int i = roomColumn - 1; i > 0; --i)
                {
                    if (Hall[i] != '.')
                        break;

                    if (RoomColumns.Contains(i))
                        continue;

                    yield return i;
                }

                // Check open spots to the right
                for (int i = roomColumn + 1; i < Hall.Length - 1; ++i)
                {
                    if (Hall[i] != '.')
                        break;

                    if (RoomColumns.Contains(i))
                        continue;

                    yield return i;
                }
            }

            private bool TryGetHome(char species, int hallPos, out int homeRoomPos)
            {
                homeRoomPos = 0;
                var homeRoomNumber = species - 'A';
                var homeRoomStartIndex = homeRoomNumber * RoomCapacity;
                var homeRoomContents = Rooms.Substring(homeRoomStartIndex, RoomCapacity);
                if (!homeRoomContents.All(x => x == '.' || x == species))
                    return false;

                var homeRoomCol = RoomColumns[homeRoomNumber];
                var dir = (homeRoomCol - hallPos < 0) ? -1 : 1;
                var i = hallPos + dir;
                while (i != homeRoomCol)
                {
                    if (Hall[i] != '.')
                        return false;
                    i += dir;
                }

                while (homeRoomPos < RoomCapacity && Rooms[homeRoomStartIndex + homeRoomPos] == '.')
                {
                    homeRoomPos++;
                }
                homeRoomPos--;
                return true;
            }

            private int GetPathCost(char species, int roomNumber, int roomPos, int hallPos)
            {
                var pathLength = (roomPos + 1) + Math.Abs(hallPos - RoomColumns[roomNumber]);
                return pathLength * CostBySpecies[species];
            }

            private int GetPathCost(char species, int roomNumber, int roomPos, int homeRoomNumber, int homeRoomPos)
            {
                var pathLength = (roomPos + homeRoomPos + 2) + Math.Abs(RoomColumns[roomNumber] - RoomColumns[homeRoomNumber]);
                return pathLength * CostBySpecies[species];
            }
        }
    }
}
