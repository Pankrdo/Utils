//-----------------------------------------------------------------------
// <copyright file="Replacer.cs" company="BestDay">
//     Copyright (c) Sprocket Enterprises. All rights reserved.
// </copyright>
// <author>Jose Martín Trejo Pancardo</author>
//----------------------------------------------------------------------

namespace Anxilaris.Utils
{
    using System.Text.RegularExpressions;

    public class Replacer
    {
        /// <summary>
        /// Remove all blanks
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <returns>modified string</returns>
        public static string RemoveBlanks(string input)
        {
            Regex regex = new Regex(@"\t|\n|\r|\s+", RegexOptions.Multiline);
            input = regex.Replace(input, " ");
            regex = new Regex(@">\s+<", RegexOptions.Multiline);
            input = regex.Replace(input, "><");
            return input.Trim();
        }

        /// <summary>
        /// Replaces a string within another string
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <param name="oldValue">Search string</param>
        /// <param name="newValue">String to replace</param>
        /// <returns>Modified string(search words completes an replace all ocuurrences)</returns>
        public static string ChangeValue(string input, string oldValue, object newValue)
        {
            return ChangeValue(input, oldValue, newValue, true, true);
        }

        /// <summary>
        /// Replaces a string within another string
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <param name="oldValue">Search string</param>
        /// <param name="newValue">String to replace</param>
        /// <param name="wordComplete">Indicates if match words completes only</param>
        /// <returns>Modified string(search and replace all ocuurrences)</returns>
        public static string ChangeValue(string input, string oldValue, object newValue, bool wordComplete)
        {
            return ChangeValue(input, oldValue, newValue, wordComplete, true);
        }

        /// <summary>
        /// Replaces a string within another string
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <param name="oldValue">Search string</param>
        /// <param name="newValue">String to replace</param>
        /// <param name="wordComplete">Indicates if match words completes only</param>
        /// <param name="replaceAll">Indicates if all matches are replaced or only the first</param>
        /// <returns>Modified string</returns>
        public static string ChangeValue(string input, string oldValue, object newValue, bool wordComplete, bool replaceAll)
        {
            string result = string.Empty;
            if (newValue != null)
            {
                result = newValue.ToString();
            }

            if (wordComplete)
            {
                oldValue += @"\b";
            }

            Regex regex = new Regex(oldValue);
            if (replaceAll)
            {
                input = regex.Replace(input, result);
            }
            else
            {
                input = regex.Replace(input, result, 1);
            }
            return input;
        }

        /// <summary>
        /// Replaces a block string within another string
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <param name="startIndex">initial position to remove</param>
        /// <param name="endBlock">end position to remove</param>
        /// <param name="newBlock">String to replace</param>
        /// <returns>Modified string</returns>
        public static string ChangeBlock(string input, int startIndex, int endBlock, string newBlock)
        {
            input = input.Remove(startIndex, endBlock);
            input = input.Insert(startIndex, newBlock);
            return input;
        }

        /// <summary>
        /// Obtiene el numero de coincidencias de la cadena especificada
        /// </summary>
        /// <param name="input">Oiginal string</param>
        /// <param name="search">Search string</param>
        /// <returns></returns>
        public static int CountMatch(string input, string search)
        {
            MatchCollection matches = Regex.Matches(input, search);

            return matches.Count;
        }
    }
}
