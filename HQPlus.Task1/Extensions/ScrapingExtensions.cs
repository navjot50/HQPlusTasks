#nullable enable
using System;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace HQPlus.Task1.Extensions {
    internal static class ScrapingExtensions {

        public static string? GetClassForChildWhereClassStartsWith(this IElement element, string classWildcard) {
            string? cssClass = null;
            
            var children = element.Children;
            foreach (var childNode in children) {
                if (childNode == null) {
                    continue;
                }
                if (childNode.ClassList.Any(c => c.StartsWith(classWildcard))) {
                    cssClass = childNode.ClassList.First(c => c.StartsWith(classWildcard));
                    break;
                }
            }

            return cssClass;
        }

        public static int? GetFirstInteger(this string str) {
            if (string.IsNullOrEmpty(str)) {
                return null;
            }
            
            var firstIntStr = new string(str.SkipWhile(c => !char.IsDigit(c))
                .Take(1)
                .ToArray());
            var firstIntParseResult = int.TryParse(firstIntStr, out var firstInt);
            
            return firstIntParseResult ? firstInt : null;
        }

        public static string GetElementText(this IElement element) {
            var str = element.Text();
            if (string.IsNullOrEmpty(str)) {
                return str;
            }

            str = str.Replace(Environment.NewLine, string.Empty).Trim();
            str = Regex.Replace(str, @"\s+", " ");
            return str;
        }

    }
}