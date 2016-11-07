using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Tools
{
    public static class StringHelper
    {
        public static string Escape(string str)
        {

            return str.Replace("\\", "\\\\").
                       Replace("\"", "\\\"").
                       Replace("\b", "\\b").
                       Replace("\f", "\\f").
                       Replace("\n", "\\n").
                       Replace("\r", "\\r").
                       Replace("\t", "\\t");
        }
    }
}
