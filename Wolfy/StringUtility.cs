using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wolfy
{
    public static class StringUtility
    {
        public static bool IsMatch(this string text, string match, string mode)
        {
            switch (mode)
            {
                case "any":
                    return true;
                case "whole_case_sensitive":
                    return text == match;
                case "whole":
                    return text.ToLower() == match.ToLower();
                case "contains":
                    return text.ToLower().Contains(match.ToLower());
                case "regex_case_sensitive":
                    return Regex.IsMatch(text, match);
                case "regex":
                    return Regex.IsMatch(text, match, RegexOptions.IgnoreCase);
                default:
                    return false;
            }
        }
    }
}
