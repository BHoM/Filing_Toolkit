/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.Filing
{
    public static partial class Query
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Tells if a filename or path contains a Regex. Also parses for valid BHoM-regex operators, like the single asterisk `*`.")]
        public static bool TryGetRegex(string path, out Regex regex)
        {
            string regexStr = path;
            regex = null;

            try
            {
                // If the path is a valid Uri, it may still contain a Query (e.g. "http://www.somesite.com/folder/file.jpg?q=randomquery.string")
                // that gives a false positive for a Regex
                Uri uriRemovedQuery = new Uri(regexStr);
                regexStr = String.Format("{0}{1}{2}{3}", uriRemovedQuery.Scheme, Uri.SchemeDelimiter, uriRemovedQuery.Authority, uriRemovedQuery.AbsolutePath);
            }
            catch { }

            regexStr = Path.GetFileName(regexStr);

            bool escapedDots = true;
            List<char> regexOperatorChars = new List<char> { '.', '/', '\\', '[', ']', '$', '*', '^', '+', '?', '{', '}', '(', ')', '/' };
            var regexOperatorInPathCount = regexOperatorChars.Count(c => regexStr.Contains(c));
            bool pathContainsOnlyOneDot = regexStr.Count(f => f == '.') == 1;

            if (regexOperatorInPathCount == 1 && pathContainsOnlyOneDot)
            {
                char charBeforeDot = regexStr.ElementAtOrDefault(regexStr.IndexOf('.') - 1);
                char charAfterDot = regexStr.ElementAtOrDefault(regexStr.IndexOf('.') + 1);
                if (!regexOperatorChars.Contains(charBeforeDot) && !regexOperatorChars.Contains(charAfterDot))
                    escapedDots = false;
            }

            // Parse for asterisks
            for (int i = 0; i < regexStr.Count(); i++)
            {
                if (regexStr[i] != '*')
                    continue;

                char charBeforeAsterisk = regexStr.ElementAtOrDefault(i - 1);

                if (!regexOperatorChars.Contains(charBeforeAsterisk))
                    regexStr = "^" + Regex.Escape(regexStr).Replace("\\*", ".*") + "$";
            }

            if (!escapedDots)
                return false;

            try
            {
                regex = new Regex(regexStr);
            }
            catch (Exception e)
            {
                // Invalid regex
                throw e;
            }

            return true;
        }

        /***************************************************/
    }
}
