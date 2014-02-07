using System;

namespace FrogGame
{
    class Car
    {
        public const int CarSize = 6;

        public int row;
        public int col;

        private ConsoleColor color;
        private Direction direction;
        public char[,] parts = 
        { 
          {' ',' ','=','=',' ',' '},
          {'=','o','=','=','o','='}
        };

        private int RowOld { get; set; }
        private int ColOld { get; set; }
        private bool hasMoved;

        //public int Row { get; set; }
        //public int Col { get; set; }

        public Direction Direction { get { return direction; } private set { } }

        public Car(int row, int col, ConsoleColor colour, Direction direction)
        {
            this.row = row;
            this.col = col;
            this.color = colour;
            this.direction = direction;
            this.hasMoved = true;
        }

        public void Move()
        {
            this.hasMoved = true;

            if (this.direction == Direction.Left)
            {
                this.col--;
            }
            else
            {
                this.col++;
            }
            CheckIsFrogHere();
        }

        internal void Redraw()
        {
            if (this.hasMoved)
            {
                if (this.col > CarSize && this.col < FrogMain.RoadCols - CarSize)
                {
                    // Delete from the old position
                    if(FrogMain.ValidateCursorPosition(this.ColOld + 2, this.RowOld + 0))
                    {
                        Console.SetCursorPosition(this.ColOld + 2, this.RowOld + 0);
                        Console.Write("  ");
                    }
                    if (FrogMain.ValidateCursorPosition(this.ColOld + 0, this.RowOld + 1))
                    {
                        Console.SetCursorPosition(this.ColOld + 0, this.RowOld + 1);
                        Console.Write("      ");
                    }
                    // Draw on the new postion
                    Console.ForegroundColor = this.color;
                    if (FrogMain.ValidateCursorPosition(this.col + 2, this.row + 0))
                    {
                        Console.SetCursorPosition(this.col + 2, this.row + 0);
                        Console.Write("==");
                    }
                    if (FrogMain.ValidateCursorPosition(this.col + 0, this.row + 1))
                    {
                        Console.SetCursorPosition(this.col + 0, this.row + 1);
                        Console.Write("=o==o=");
                    }
                }
                else
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

                    // Draw on the new postion
                    Console.ForegroundColor = this.color;
                    for (int row = 0; row < parts.GetLength(0); row++)
                    {
                        for (int col = 0; col < parts.GetLength(1); col++)
                        {
                            if (FrogMain.ValidateCursorPosition(this.col + col, this.row + row))
                            {
                                Console.SetCursorPosition(this.col + col, this.row + row);
                                Console.Write(this.parts[row, col]);
                            }
                        }
                    }
                }

                Console.ResetColor();

                // Remember the new position
                this.RowOld = this.row;
                this.ColOld = this.col;
                this.hasMoved = false;
            }
        }    

        private void CheckIsFrogHere()
        {
            if (this.row == FrogMain.frog.Row)
            {
                if (this.col - FrogMain.frog.Col < Frog.FrogSize - 1 &&
                    FrogMain.frog.Col - this.col < Car.CarSize - 1)
                {
                    FrogMain.gameState = FrogMain.GameState.RoadKill;
                }                
            } 
        }
    }
}
