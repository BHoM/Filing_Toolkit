using BH.oM.Adapters.Filing;
using BH.oM.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.Engine.Reflection;
using System.IO;

namespace BH.Engine.Filing
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Methods                                   ****/
        /***************************************************/

        [Description("If an user path is specified – e.g. containing `C:\\Users\\SomeUser` – modifies SomeUser to match the current user.")]
        public static string RelativiseUserPath(string path)
        {
            string relativisedPath = path;
            char separator = '\\';

            string normalisedPath = path.NormalisePath(false);
            string[] directories = normalisedPath.Split(separator);

            string currentUserPath = GetUserPath();
            string currentUserFolderName = currentUserPath.Split(separator).Last();

            if (normalisedPath.StartsWith(Path.GetDirectoryName(currentUserPath)) && directories.Length > 2)
            {
                directories[2] = currentUserFolderName;

                relativisedPath = Path.Combine(directories);

                relativisedPath = string.Join(separator.ToString(), directories);
            }
            return relativisedPath;
        }

        // Get the user folder, in the form of C:\Users\UserName.
        // Written to work also for Windows versions prior to 10.
        private static string GetUserPath()
        {
            string path = System.IO.Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = System.IO.Directory.GetParent(path).ToString();
            }

            return path;
        }
    }
}
