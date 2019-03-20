using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace SD200_Final_Project_Blog.MyHelpers
{
    [NotMapped]
    public static class StringHelper
    {
        private static Random Random { get; } = new Random();

        /// <summary>
        ///     Generates a slug by passed in GetCharactersBy delegate
        ///     then with that this method returns the 
        /// </summary>
        /// <param name="str">string that you want as a slug</param>
        /// <param name="unwantedCharacters">all the characters you want removed</param>
        /// <returns>slug (URL friendly string)</returns>
        public static string GenerateSlug(this string str, HashSet<char> unwantedCharacters)
        {
            // Make all characters lowercase
            string tempString = str.ToLower();

            // Match multiple white spaces
            Regex multiWhiteSpace = new Regex(@"\s+");

            // Replace multiple white spaces with a single whitespace character
            tempString = multiWhiteSpace.Replace(tempString, " ");

            // Remove whitespace (at the start and end) then replace remaining whitespace with a dash
            tempString = tempString.Trim().Replace(" ", "-");

            // return slug
            return tempString.FilterOutCharacters(unwantedCharacters);
        }


        /// <summary>
        ///     Filters out the unwanted characters from a string 
        ///     then returns a new string without the unwanted characters
        /// </summary>
        /// <param name="str">string that contains characters that you want to filtered out</param>
        /// <param name="unwantedCharacters">all the characters you want removed</param>
        /// <returns>string that doesn't contain any of the characters that was passed in</returns>
        public static string FilterOutCharacters(this string str, HashSet<char> unwantedCharacters) => new string
        (
            str.Where(character => (
                    !unwantedCharacters.Any(unWantedCharacter => character == unWantedCharacter)
               )).ToArray()
        );


        public static string GetPlainTextFromHtml(this string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            htmlString = Regex.Replace(htmlString, htmlTagPattern, "");
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            return htmlString;
        }


        public static string GenerateRandomStringFromGuid(this Guid guid)
        {
            return new string(guid.ToString().Skip(Random.Next(16)).Take(Random.Next(16)).ToArray());
        }

        public static string GenerateRandomStringFromGuid(this Guid guid, int take)
        {
            if (take > 16)
            {
                take = 16;
            }
            else if (take < 1)
            {
                take = 1;
            }

            return new string(guid.ToString().Take(take).ToArray());
        }
    }
}
