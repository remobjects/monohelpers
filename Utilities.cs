using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RemObjects.Mono.Helpers
{
    static class Utilities
    {
        public static string HomePath { get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); } }

        public static string PathCombine(params string[] args)
        {
            if (args == null || args.Length == 0) throw new ArgumentException("args");
            if (args.Length == 1) return args[0];
            string res = args[0];
            for (int i = 1; i < args.Length; i++)
                res = Path.Combine(res, args[i]);
            return res;
        }

        public static string PathResolve(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (path == "~") return HomePath;
            // there's currently no cross platform way to get 
            // another users' homedir, so we don't support that
            // yet.

            if (path.StartsWith("~" + Path.DirectorySeparatorChar.ToString()))
            {
                path = Path.Combine(HomePath, path.Substring(2));
            }
            return path;
        }

        public static string PathRelative(string path, string relativeto)
        {
            // already relative? 
            if (!Path.IsPathRooted(path))
                return path;

            // do this before tolower
            string result = Path.GetFileName(path);

            // not on same drive? 
            if (string.Compare(Path.GetPathRoot(path), Path.GetPathRoot(relativeto), true) != 0)
                return path;

            path = Path.GetDirectoryName(path);

            while (!relativeto.StartsWith(
                path.EndsWith(Path.DirectorySeparatorChar.ToString()) ?
                path: path + Path.DirectorySeparatorChar))
            {
                result = Path.Combine(Path.GetFileName(path), result);
                path = Path.GetDirectoryName(path);
            }

            relativeto = Path.GetDirectoryName(relativeto);

            while (relativeto != path)
            {
                result = Path.Combine("..", result);
                relativeto = Path.GetDirectoryName(relativeto);
            }
            return result;
        }
    }
}
