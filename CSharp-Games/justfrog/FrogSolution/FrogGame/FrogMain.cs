using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FrogGame
{
    class FrogMain
    {
        private const long RefreshInterval = 40;
        private const int MaxLives = 5;

        public const int RoadCols = 80;
        public const int RoadRows = 35;

        public static Random rand;
        public enum GameState { Stopped, Running, RoadKill, Success }
        public static Frog frog;
        public static GameState gameState;
        public static Stopwatch gameClock;

        private static long gameTime;
        private static int life;
        private static Lane[] allLanes;
        private static ConsoleColor[] colors;
        private static long lastRefresh;
        private static bool lanesPopulated;

        static void Main()
        {
            // Set console dimensions
            SetConsole();

            // Title animation
            ConsoleAnimations.AnimateTitle();

            // Start game
            StartNewGame(MaxLives);

            while (true)
            {
                gameTime = gameClock.ElapsedMilliseconds;

                //Move player
                if (lanesPopulated)
                {
                    if (Console.KeyAvailable)
                    {
                        frog.Move(Console.ReadKey(true).Key);
                    }
                }
                else if (allLanes[3].cars.Count > 0 &&
                    allLanes[3].cars[0].col > RoadCols / 2)
                {
                    // At least one car from the fastest lane has crossed the middile of the gamefield
                    lanesPopulated = true;
                    while (Console.KeyAvailable) Console.ReadKey(true); // Flush the console buffer
                }

                // Generate new cars
                foreach (var lane in allLanes)
                {
                    if (gameTime - lane.lastCarGenTime > lane.genCarInterval)
                    {
                        lane.lastCarGenTime = gameTime;
                        lane.genCarInterval = rand.Next(lane.lowInterval, lane.highInterval);
                        lane.GenerateCar();
                    }
                }

                // Move cars
                foreach (var lane in allLanes)
                {
                    if (gameTime - lane.lastCarMove > lane.speedInterval)
                    {
                        lane.lastCarMove = gameTime;
                        lane.UpdateCarPositions();
                    }
                }

                // Refresh console
                if (gameTime - lastRefresh > RefreshInterval)
                {
                    lastRefresh = gameTime;
                    RefreshConsole();

                    // Exit the game here and only after refresh
                    if (gameState == GameState.RoadKill)
                    {
                        RoadKill();
                    }

                    if (frog.Row == 0)
                    {
                        Sucess();
                    }
                }
            }
        }

        private static void CreateLanes()
        {
            // 6 lanes with speeds from 20ms to 40ms
            allLanes = new Lane[6];

            int rowInterval = 4;
            int speedInterval = 10;

            // Left going lanes
            int i = 0, row = rowInterval, speed = 40;
            for (; i < allLanes.Length / 2; i++, row += rowInterval, speed -= speedInterval)
            {
                allLanes[i] = new Lane(row, Direction.Left, speed);
            }

            // Right going lanes
            row += rowInterval;
            speed += speedInterval;
            for (; i < allLanes.Length; i++, row += rowInterval, speed += speedInterval)
            {
                allLanes[i] = new Lane(row, Direction.Right, speed);
            }
        }

        public static ConsoleColor RandomColor()
        {
            return colors[rand.Next(colors.Length)];
        }

        private static void RefreshConsole()
        {
            foreach (var lane in allLanes)
            {
                lane.DrawCars();
            }

            if (lanesPopulated)
                frog.Redraw();

            int sum = 0;
            foreach (var line in allLanes)
                sum += line.cars.Count;

            //PrintMessage(String.Format("Total cars: {0}", sum));
            PrintMessage(String.Format("Lives: {0}", life));
        }

        private static void RoadKill()
        {
            life--;
            frog.DrawRoadKill();
            if (life > 0)
            {
                PrintMessage(String.Format("RoadKill ({0} lives left)", life));
                PressAnyKeyToContinue();
                StartNewGame(life);
            }
            else
            {
                PrintMessage(String.Format("You lost {0} lives. Try again (Y/N)?", MaxLives));
                TryAgain();
            }
        }

        private static void Sucess()
        {
            PrintMessage("Success! Try again (Y/N)?");
            TryAgain();
        }

        private static void TryAgain()
        {
            ConsoleKey key = Console.ReadKey(true).Key;
            while (key != ConsoleKey.Y && key != ConsoleKey.N)
            {
                key = Console.ReadKey(true).Key;

            }
            if (key == ConsoleKey.Y)
            {
                StartNewGame(MaxLives);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        static void PrintMessage(string message)
        {
            if (ValidateCursorPosition(0, Console.WindowHeight - 2))
            {
                // Delete the line
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine(new String(' ', Console.WindowWidth - 1));

                // Print the message
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                Console.WriteLine(message);
            }
        }

        private static void StartNewGame(int lives)
        {
            Console.Clear();

            // Print demarkations
            string demark = ">";
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(0, 15);
            for (int i = 0; i <= RoadCols / demark.Length; i++)
                Console.Write(demark);
            Console.SetCursorPosition(0, 18);
            for (int i = 0; i <= RoadCols / demark.Length; i++)
                Console.Write(demark);
            Console.ResetColor();

            // Initialize variables
            gameClock = Stopwatch.StartNew();
            rand = new Random();
            lastRefresh = gameClock.ElapsedMilliseconds;

            // List of available colors
            colors = new ConsoleColor[] {
                ConsoleColor.Cyan,
                ConsoleColor.Magenta,
                ConsoleColor.Red,
                ConsoleColor.Yellow,
            };

            // Create objects
            CreateLanes();
            lanesPopulated = false;
            frog = new Frog();
            life = lives;
            gameState = GameState.Running;
        }

        private static void SetConsole()
        {
            Console.CursorVisible = false;
            try
            {
                // Set console dimensions
                Console.WindowWidth = RoadCols + 1;
                Console.WindowHeight = RoadRows + 1;

                Console.BufferWidth = RoadCols + 1;
                Console.BufferHeight = RoadRows + 1;
            }
            catch (ArgumentOutOfRangeException)
            {

                Console.WriteLine("Cannot set console dimensions!");
                Console.ReadKey(true);
            }
        }

        // Checks if the requested cursor position is on the console
        public static bool ValidateCursorPosition(int col, int row)
        {
            return (
                row >= 0 &&
                row < RoadRows &&
                col >= 0 &&
                col <= RoadCols
                );
        }

        // Waits for a key press different than up/down arrow
        private static void PressAnyKeyToContinue()
        {
            Console.ResetColor();
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            while (pressedKey.Key == ConsoleKey.UpArrow || pressedKey.Key == ConsoleKey.DownArrow)
            {
                pressedKey = Console.ReadKey(true);
            }
        }
    }
}