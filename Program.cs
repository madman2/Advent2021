﻿using AdventOfCode.Interfaces;
using System;
using System.Diagnostics;
using System.IO;

namespace AdventOfCode
{
    class Program
    {
        private const int DayToSolve = 4;

        static void Main(string[] args)
        {
            // Create solver
            var solverTypeName = "AdventOfCode.Day" + DayToSolve.ToString("D2");
            var solverType = Type.GetType(solverTypeName);
            if (solverType == null)
            {
                Console.WriteLine($"Invalid type: {solverTypeName}");
                return;
            }
            var solver = Activator.CreateInstance(solverType) as ISolver;

            try
            {
                // Read first input file
                var firstStarFilePath = Path.Combine(solver.GetType().Name, solver.InputFileName);
                var firstStarFileReader = File.OpenText(firstStarFilePath);

                var stopWatch = Stopwatch.StartNew();
                var firstStarResult = solver.SolveFirstStar(firstStarFileReader);
                stopWatch.Stop();
                Console.WriteLine($"First star result: {firstStarResult}, {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown while solving first star: {e.Message}");
            }

            try
            {
                // Read second input file
                var secondStarFilePath = Path.Combine(solver.GetType().Name, solver.InputFileName);
                var secondStarFileReader = File.OpenText(secondStarFilePath);

                var stopWatch = Stopwatch.StartNew();
                var secondStarResult = solver.SolveSecondStar(secondStarFileReader);
                stopWatch.Stop();
                Console.WriteLine($"Second star result: {secondStarResult}, {stopWatch.ElapsedMilliseconds}ms");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown while solving second star: {e.Message}");
            }
        }
    }
}
