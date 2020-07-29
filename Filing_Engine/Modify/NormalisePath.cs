using BH.oM.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /*** Methods                                     ***/
        /***************************************************/

        [Description("Attempts to normalise a string Path.")]
        public static string NormalisePath(this string path, bool enableWarning = true)
        {
            string normalisedPath = path;

            try
            {
                normalisedPath = Path.GetFullPath(new Uri(path).LocalPath)
                               .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            } catch
            {
                BH.Engine.Reflection.Compute.RecordWarning("It was not possible to normalise the path.");
            }

            return normalisedPath;
        }

        /***************************************************/
    }
}
