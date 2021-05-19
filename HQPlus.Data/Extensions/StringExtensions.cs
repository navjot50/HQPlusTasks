using System.Text;
using System.Text.Json;

namespace HQPlus.Data.Extensions {
    public static class StringExtensions {
        
        public static string ToCamelCase(this string str) {
            return string.IsNullOrEmpty(str) ? str : JsonNamingPolicy.CamelCase.ConvertName(str);
        }
        
        public static string ToSnakeCase(this string text)
        {
            if (string.IsNullOrEmpty(text)) {
                return text;
            }

            if(text.Length < 2) {
                return text;
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(text[0]));
            for(int i = 1; i < text.Length; ++i) {
                char c = text[i];
                if(char.IsUpper(c)) {
                    sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                } else {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}