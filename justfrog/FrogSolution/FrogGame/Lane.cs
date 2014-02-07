using System;
using System.Collections.Generic;

namespace FrogGame
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    };

    class Lane
    {
        public int lowInterval;
        public int highInterval;

        private int row;
        private int col;
        private Direction dir;

        public List<Car> cars = new List<Car>();
        public long lastCarGenTime;
        public long genCarInterval;
        public long lastCarMove;
        public long speedInterval;

        public Lane(int row, Direction dir, long speed)
        {
            this.row = row;
            this.dir = dir;
            this.col = (dir == Direction.Right ? -Car.CarSize : FrogMain.RoadCols);

            this.lastCarMove = FrogMain.gameClock.ElapsedMilliseconds;
            this.speedInterval = speed;

            this.genCarInterval = (int)this.speedInterval * 50;
            this.lastCarGenTime = FrogMain.gameClock.ElapsedMilliseconds - this.genCarInterval;

            this.lowInterval = (int)(this.genCarInterval * 0.8);
            this.highInterval = (int)(this.genCarInterval * 1.2);

            // TODO: Generate more than one car at first so the frog doesnt
            // cross before they are out. Maybe even start with like 5 cars per line?
        }

        public void GenerateCar()
        {
            this.cars.Add(new Car(this.row, this.col, FrogMain.RandomColor(), this.dir));
        }

        public void UpdateCarPositions()
        {
            for (int i = 0; i < this.cars.Count; i++)
            {
                cars[i].Move();

                if (this.dir == FrogGame.Direction.Left && cars[i].col < 0 - Car.CarSize - Car.CarSize)
                {
                    this.cars.RemoveAt(i);
                }
                else if (this.dir == FrogGame.Direction.Right && cars[i].col > FrogMain.RoadCols + Car.CarSize)
                {
                    this.cars.RemoveAt(i);
                }
            }
        }

        public void DrawCars()
        {
            foreach (var car in cars)
            {
                car.Redraw();
            }
        }
    }
}