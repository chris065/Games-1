using System;

namespace FrogGame
{
    public class Frog
    {
        private const int JumpStep = 4;

        private char[,] parts =
        {
            {' ', '@', '.', '.', '@', ' '},
            {'(', '-', '+', '+', '-', ')'},
        };

        public ConsoleColor color;
        public const int FrogSize = 6;
        public int Row { get; set; }
        public int Col { get; set; }

        private int RowOld { get; set; }
        private int ColOld { get; set; }
        private bool hasMoved;

        private int Height
        {
            get { return this.parts.GetLength(0); }
        }

        private int Width
        {
            get { return this.parts.GetLength(1); }
        }

        public Frog()
        {
            this.Row = FrogMain.RoadRows - this.Height - 1;
            this.Col = FrogMain.RoadCols / 2 - this.Width / 2;
            this.color = ConsoleColor.Green;
            this.hasMoved = true;
        }

        public void Move(ConsoleKey pressedKey)
        {
            this.hasMoved = true;
            switch (pressedKey)
            {
                case ConsoleKey.UpArrow:
                    if (this.Row >= JumpStep) this.Row -= JumpStep;
                    break;
                case ConsoleKey.DownArrow:
                    if (this.Row < FrogMain.RoadRows - JumpStep) this.Row += JumpStep;
                    break;
                case ConsoleKey.LeftArrow:
                    if (this.Col > 0) this.Col--;
                    break;
                case ConsoleKey.RightArrow:
                    if (this.Col <= FrogMain.RoadCols - this.Width) this.Col++;
                    break;
            }
        }

        public void Redraw()
        {
            if (this.hasMoved)
            {
                // Delete from the old position
                for (int row = 0; row < parts.GetLength(0); row++)
                {
                    for (int col = 0; col < parts.GetLength(1); col++)
                    {
                        if (FrogMain.ValidateCursorPosition(this.ColOld + col, this.RowOld + row))
                        {
                            Console.SetCursorPosition(this.ColOld + col, this.RowOld + row);
                            Console.Write(' ');
                        }
                    }
                }

              
                Console.ForegroundColor = this.color;

                for (int row = 0; row < parts.GetLength(0); row++)
                {
                    for (int col = 0; col < parts.GetLength(1); col++)
                    {
                        if (FrogMain.ValidateCursorPosition(this.Col + col, this.Row + row))
                        {
                            Console.SetCursorPosition(this.Col + col, this.Row + row);
                            Console.Write(this.parts[row, col]);
                        }
                    }
                }
                Console.ResetColor();

                // Remember the new position
                this.RowOld = this.Row;
                this.ColOld = this.Col;
                this.hasMoved = false;
            }
        }

        public void DrawRoadKill()
        {
            this.parts[0, 1] = 'X';
            this.parts[0, 4] = 'X';

            for (int row = 0; row < parts.GetLength(0); row++)
            {
                for (int col = 0; col < parts.GetLength(1); col++)
                {
                    if (FrogMain.ValidateCursorPosition(this.ColOld + col, this.RowOld + row))
                    {
                        Console.SetCursorPosition(this.ColOld + col, this.RowOld + row);
                        Console.Write(' ');
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;

            for (int row = 0; row < parts.GetLength(0); row++)
            {
                for (int col = 0; col < parts.GetLength(1); col++)
                {
                    if (FrogMain.ValidateCursorPosition(this.Col + col, this.Row + row))
                    {
                        Console.SetCursorPosition(this.Col + col, this.Row + row);
                        Console.Write(this.parts[row, col]);
                    }
                }
            }
            Console.ResetColor();

        }
    }
}
