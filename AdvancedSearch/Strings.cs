using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedSearch
{
    public class Strings
    {
        public static string addonName = "Advanced Search";
    }
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
