namespace Wolfenstein.Common
{
    using System;

    /// <summary>
    /// A class for the random generator
    /// </summary>
    public static class RandomGenerator
    {
        /// <summary>
        /// Random generator
        /// </summary>
        private static readonly Random RandomGen = new Random();

        /// <summary>
        /// Generates a random percent
        /// </summary>
        /// <returns>A random percent</returns>
        public static double Percent()
        {
            double number = RandomGen.Next(0, 101);

            return number;
        }

        /// <summary>
        /// Generates a random integer
        /// </summary>
        /// <param name="bottom">Bottom range</param>
        /// <param name="top">Top range</param>
        /// <returns>RandomGen integer</returns>
        public static int Integer(int bottom, int top)
        {
            int number = RandomGen.Next(bottom, top);
            return number;
        }

        /// <summary>
        /// Generates a random direction
        /// </summary>
        /// <returns>Direction</returns>
        public static Direction Direction()
        {
            int number = RandomGen.Next(0, 4);
            var direction = (Direction)number;
            return direction;
        }
    }
}
