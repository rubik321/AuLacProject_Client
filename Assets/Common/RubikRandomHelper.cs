using System;

namespace Rubik.Common
{
    public static class RubikRandomHelper
    {
        static Random rng = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// Random int number
        /// </summary>
        /// <param name="from">Include</param>
        /// <param name="to">Exclude</param>
        /// <returns>value = from .. to-1</returns>
        public static int Next(int from, int to)
        {
            return rng.Next(from, to);
        }

        /// <summary>
        /// Random int number
        /// </summary>
        /// <param name="to">Exclude</param>
        /// <returns>value = 0 .. to-1</returns>
        public static int Next(int to)
        {
            return rng.Next(to);
        }
        
        /// <summary>
        /// Random bool
        /// </summary>
        /// <returns>value = 0 .. to-1</returns>
        public static bool Next()
        {
            return rng.Next(2) == 0;
        }
        
    }
}
