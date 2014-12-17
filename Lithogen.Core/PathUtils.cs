using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public static class PathUtils
    {
        public static string GetSubPath(string commonParent, string child)
        {
            commonParent.ThrowIfNullOrWhiteSpace("commonParent");
            child.ThrowIfNullOrWhiteSpace("child");

            if (child.StartsWith(commonParent, StringComparison.OrdinalIgnoreCase))
            {
                string result = child.Substring(commonParent.Length);
                if (result[0] == '\\')
                    return result.Substring(1);
                else
                    return result;
            }
            else
            {
                return child;
            }
        }
    }
}
