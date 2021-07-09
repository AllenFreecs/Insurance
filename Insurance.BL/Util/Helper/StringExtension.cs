using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Insurance.BL.Util.Helper
{
   public static class StringExtension
    {
        public static int? ToInt32Nullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Convert.ToInt32(value);
        }

        public static decimal? ToDecimalNullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Convert.ToDecimal(value);
        }

        public static DateTime? ToDateTimeNullable(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return Convert.ToDateTime(value);
        }

        public static string ToCamelCase(this string text)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return Char.ToLowerInvariant(text[0]) + text.Substring(1);
            }
            return text;
        }
        public static string ToTitleCase(this string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            if (!string.IsNullOrEmpty(text) && text.Length > 1)
            {
                return textInfo.ToTitleCase(text.ToLower());
            }
            return text;
        }

        public static decimal ToZeroIfNullOrEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value) || value == "")
            {
                return (decimal)0.00;
            }

            return Convert.ToDecimal(value);
        }

        public static string GetValueOrDefault(this string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.TrimEnd();
            }
        }

        public static string ToTitleCaseInvariant(this string targetString)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(targetString);
        }
    }
}
