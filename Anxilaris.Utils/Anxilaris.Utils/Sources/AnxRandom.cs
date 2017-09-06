//-----------------------------------------------------------------------
// <copyright file="AnxRandom.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System;
    using System.Threading;
    
    public static class AnxRandom
    {        
        private const int DEFAULT_LENGTH = 8;
        private const string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        private static int _location = GetLocation();
        private static readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _location)));

        /// <summary>
        /// Styles to strings result
        /// </summary>
        public enum eStringStyle
        {
            Normal,
            Upper,
            Lower
        }

        /// <summary>
        /// Get a random int value
        /// </summary>
        /// <returns>int32</returns>
        public static int Next()
        {
            return _random.Value.Next();
        }

        /// <summary>
        /// Get a random int value
        /// </summary>
        /// <param name="Max">Max value for random</param>
        /// <returns>Int value</returns>
        public static int Next(int Max)
        {
            return _random.Value.Next(Max);
        }

        /// <summary>
        /// Get a random int value
        /// </summary>
        /// <param name="Max">Max value for random</param>
        /// <param name="Min">Min value for random</param>
        /// <returns>Int value</returns>
        public static int Next(int Min, int Max)
        {
            return _random.Value.Next(Min, Max);
        }

        /// <summary>
        /// Get a random double value between 0.0 and 1.0
        /// </summary>
        /// <returns>Double value</returns>
        public static double NextDouble()
        {
            return _random.Value.NextDouble();
        }

        /// <summary>
        /// Get a random string with 8 characters and normal style
        /// </summary>
        /// <returns>String alphanumeric</returns>
        public static string NextString()
        {
            return NextString(eStringStyle.Normal, DEFAULT_LENGTH);
        }

        /// <summary>
        /// Get a random string with 8 characters with specified style
        /// </summary>
        /// <param name="Style">Style of string return</param>
        /// <returns>String alphanumeric</returns>
        public static string NextString(eStringStyle Style)
        {
            return NextString(Style, DEFAULT_LENGTH);
        }

        /// <summary>
        /// Get a random string with especified characters with specified style and lenght
        /// </summary>
        /// <param name="Style">Style of string return</param>
        /// <param name="Lenght">Number of characters to string return</param>
        /// <returns>String alphanumeric</returns>
        public static string NextString(eStringStyle Style, int Lenght)
        {
            string result = string.Empty;
            for (int i = 0; i < Lenght; i++)
            {
                switch (Style)
                {
                    case eStringStyle.Normal:
                        result += CHARS[Next(CHARS.Length)].ToString();
                        break;
                    case eStringStyle.Lower:
                        result += CHARS[Next(CHARS.Length)].ToString().ToLower();
                        break;
                    case eStringStyle.Upper:
                        result += CHARS[Next(CHARS.Length)].ToString().ToUpper();
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// GetLocation
        /// </summary>
        /// <returns></returns>
        private static int GetLocation()
        {
            DateTime current = DateTime.Now;
            string hourPart = string.Format("{0:HHmmss}", current);
            string datePart = string.Format("{0:yyMMdd}", current);
            return Convert.ToInt32(hourPart) + Convert.ToInt32(datePart) + current.Millisecond;
        }
    }
}
