using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using System.Numerics;
using System.ComponentModel.DataAnnotations;

namespace ExtensionMethods
{
    public static class NullableDateTimeExtensions
    {
        /// <summary>
        /// Returns short date string or empty string if the value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToShortDateString(this DateTime? item)
        {
            return item.HasValue ? item.Value.ToShortDateString() : string.Empty;
        }

        /// <summary>
        /// Calculates age (at given date) form date of birth
        /// </summary>
        /// <param name="birthDate">Date of Birth</param>
        /// <param name="ageAt">Date which to calculate age for</param>
        /// <returns></returns>
        public static int ToAge(this DateTime? birthDate, DateTime ageAt)
        {
            if (!birthDate.HasValue) return -1;

            int age = ageAt.Year - birthDate.Value.Year;
            if (ageAt.Month < birthDate.Value.Month || (ageAt.Month == birthDate.Value.Month && ageAt.Day < birthDate.Value.Day)) age--;
            return age;
        }

        /// <summary>
        /// Calculates age (now) from date of birth
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public static int ToAge(this DateTime? birthDate)
        {
            return birthDate.ToAge(DateTime.Today);
        }
    }

    public static class DateTimeExtensions
    {
        /// <summary>
        /// Given date in list of dates, return short date representation if the date with this day is only once in the list ie there is no other date with same day in the list
        /// Otherwise, when the list contains at least two dates with same day as this day, return ShortDate+hour:minutes
        /// Example:  
        /// d1 : 2015-05-23-20-20-20;
        /// d2 : 2015-06-24-08-30-00; (same day)
        /// d3 : 2015-06-24-09-45-00; (same day)
        /// var dates = [ d1, d2, d3 ]
        /// 
        /// d1.ToShortDateUnique() will return d1.ToShortDateString()
        /// d2.ToShortDateUnique() will return d2.ToShortDateString() + d2.ToShortTimeString()
        ///     //may return "24. 6. 2015 8:30" depending on what return ShortDateString
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToShortDateUnique(this DateTime value, IEnumerable<DateTime> listOfDates)
        {
            //if this day is in list of dates more than once
            if (listOfDates.Select(d => d.ToShortDateString()).Where(d => d == value.ToShortDateString()).Count() > 1)
            {
                return value.ToShortDateString() + " " + value.ToShortTimeString();
            }
            else
            {
                return value.ToShortDateString();
            }
        }

        /// <summary>
        /// Get the quarter of the year for this datetime.
        /// </summary>
        /// <param name="value">1 to 4 ...quarter of year to which this date belongs </param>
        /// <returns></returns>
        public static int Quarter(this DateTime value)
        {
            return (value.Month - 1) / 3 + 1;
        }

        public static DateTime FirstDayOfQUarter(this DateTime value)
        {
            int quarterNumber = value.Quarter();
            return new DateTime(value.Year, (quarterNumber - 1) * 3 + 1, 1);
        }

        public static DateTime LastDayOfQUarter(this DateTime value)
        {
            return value.FirstDayOfQUarter().AddMonths(3).AddDays(-1);
        }

        /// <summary>
        /// Get first day of the week (that this date is part of)
        /// </summary>
        /// <param name="startOfWeek">Which day is considered the start of week (yeah, hello americans)</param>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Last day of thsi month
        /// </summary>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        public static DateOnly ToDateOnly(this DateTime value)
        {
            return DateOnly.FromDateTime(value);
        }
    }

    public static class DateExt
    {
        /// <summary>
        /// Counts how many Mondays (or Tuesdays,..) is in given date range
        /// </summary>
        /// <param name="day">day of week you want to count</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int CountDays(DayOfWeek day, DateTime startDate, DateTime endDate)
        {
            int cnt = 0;

            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1.0))
            {
                if (dt.DayOfWeek == day)
                {
                    cnt++;
                }
            }

            return cnt;
        }

        /// <summary>
        /// List all Mondays (or Tuesdays,..) is in given date range
        /// </summary>
        /// <param name="day">day of week you want to count</param>
        /// <param name="startDate">including this</param>
        /// <param name="endDate">including this</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetDaysBetween(DayOfWeek day, DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1.0))
            {
                if (dt.DayOfWeek == day)
                {
                    yield return dt;
                }
            }

        }


        public static IEnumerable<DateTime> monthsDatesBetween(DateTime d0, DateTime d1)
        {
            return Enumerable.Range(0, (d1.Year - d0.Year) * 12 + (d1.Month - d0.Month + 1))
                             .Select(m => new DateTime(d0.Year, d0.Month, 1).AddMonths(m));
        }

        public static IEnumerable<string> monthsNamesBetween(DateTime d0, DateTime d1)
        {
            return monthsDatesBetween(d0, d1).Select(x => DateTimeFormatInfo.CurrentInfo.MonthNames[x.Month - 1]);
        }
    }

    public static class TimeExtensions
    {
        /// <summary>
        /// 11:40 -> 11.6
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double HoursToDecimal(this string time)
        {
            try
            {
                DateTime dt0 = DateTime.Parse("00:00");
                DateTime dt1 = DateTime.Parse(time);
                double span = (dt1 - dt0).TotalHours;
                var rounded = Math.Round(span, 2);
                return rounded;
            }
            catch
            {
                return -1;
            }
        }

        public static TimeOnly ToTimeOnly(this TimeSpan item)
        {
            return TimeOnly.FromTimeSpan(item);
        }
    }

    public static class NullableExtensions
    {
        /// <summary>
        /// Returns value of nullable type as string ot empty string if the value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToStringOrEmpty<T>(this Nullable<T> item) where T : struct
        {
            return item.HasValue ? item.ToString() : string.Empty;
        }

        /// <summary>
        /// Returns value of nullable double as string (formated according to parameter) ot empty string if the value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String double_ToStringOrEmpty(this double? item, string format)
        {
            return item.HasValue ? item.Value.ToString(format) : string.Empty;
        }

        /// <summary>
        /// Returns value of nullable float as string (formated according to parameter) ot empty string if the value is null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String float_ToStringOrEmpty(this float? item, string format)
        {
            return item.HasValue ? item.Value.ToString(format) : string.Empty;
        }

    }

    public static class BoolExtensions
    {
        /// <summary>
        /// Returns true if value is contained in given collection.
        /// 
        /// Example usage:
        /// if(reallyLongIntegerVariableName.In(1,6,9,11))
        // {
        ///      // do something....
        /// }
        /// and
        /// if(reallyLongStringVariableName.In("string1","string2","string3"))
        /// {
        ///      // do something....
        /// }
        /// and
        /// if(reallyLongMethodParameterName.In(SomeEnum.Value1, SomeEnum.Value2, SomeEnum.Value3, SomeEnum.Value4)
        /// {
        ///  // do something....
        /// }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        /// <summary>
        /// Returns true if the value is between lower and upper. Works for everything that implement IComparable e.g. Int, Double etc.
        /// </summary>
        /// <typeparam name="T">T must implement IComparable</typeparam>
        /// <param name="actual"></param>
        /// <param name="lower">Lower bound INCLUSIVE</param>
        /// <param name="upper">Upper bound EXCLUSIVE</param>
        /// <returns></returns>
        public static bool Between<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }

        /// <summary>
        /// Checks whether objects in this collection are in incremental order. (Objects must be comparable)
        /// </summary>
        /// <typeparam name="T">Type of objects in collection. Must implement IComparable.</typeparam>
        /// <param name="sequence">Collection of comparable elements.</param>
        /// <param name="stronglyIncremental">Strongly incremental means that next object is greater than previous object. Not strongly incremental means that next object is greater or equal to previous object</param>
        /// <returns>True if the elements in this collection are sorted incrementaly</returns>
        public static bool IsIncremental<T>(this IEnumerable<T> sequence, bool stronglyIncremental = false) where T : IComparable<T>
        {
            using (var iter = sequence.GetEnumerator())
            {
                if (iter.MoveNext())
                {
                    var prevItem = iter.Current;
                    while (iter.MoveNext())
                    {
                        var nextItem = iter.Current;
                        if (stronglyIncremental)
                        {
                            if (prevItem.CompareTo(nextItem) >= 0)
                                return false;
                        }
                        else
                        {
                            if (prevItem.CompareTo(nextItem) > 0)
                                return false;
                        }
                        prevItem = nextItem;
                    }
                }
            }

            return true;
        }
    }

    public static class GenericExtensions
    {
        /// <summary>
        /// Return this.ToString or alternative string value if this item does not satisfy the predicate condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="predicate"></param>
        /// <param name="alternativeValue"></param>
        /// <returns></returns>
        public static string ToStringOrValue<T>(this T item, Func<T, bool> predicate, string alternativeValue)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (predicate == null) throw new ArgumentNullException("predicate");

            if (predicate(item))
                return item.ToString();
            else
                return alternativeValue;
        }
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// This extension method makes it easy to get the description (friendly name with spaces etc.) for a given value of the enum.
        /// 
        /// You can use it like that :
        ///
        /// public enum MyEnum
        /// {
        ///    [Description("Description for Foo")]
        ///    Foo,
        ///    [Description("Description for Bar")]
        ///    Bar
        /// } 
        ///
        /// MyEnum x = MyEnum.Foo;
        /// string description = x.GetDescription();
        /// (http://stackoverflow.com/questions/1415140/c-enums-can-my-enums-have-friendly-names/1415187#1415187)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets [Display(Name = "Some nice name")] of enum.
        /// </summary>
        /// <returns>effective DisplayAttribute.Name of given enum.</returns>
        public static string GetDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible // C# 7.3+: where T : struct, Enum
        {
            if (!typeof(T).IsEnum) // Not needed in C# 7.3+ with above updated constraint
                throw new ArgumentException("Argument must be of type Enum");

            DisplayAttribute displayAttribute = enumValue.GetType()
                                                         .GetMember(enumValue.ToString())
                                                         .First()
                                                         .GetCustomAttribute<DisplayAttribute>();

            string displayName = displayAttribute?.GetName();

            return displayName ?? enumValue.ToString();
        }
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> mapFunction)
        {
            foreach (var item in @enum) mapFunction(item);
        }

        /// <summary>
        /// foreach((int index, string item) in myList.WithIndex())...
        /// </summary>
        public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> source)
        {
            int index = 0;
            foreach (var item in source)
            {
                index = index + 1;
                yield return (index, item);
            }
        }

        private static Random random = new Random();

        public static T GetRandomElement<T>(this IEnumerable<T> list)
        {
            // If there are no elements in the collection, return the default value of T
            if (list.Count() == 0)
                return default(T);

            return list.ElementAt(random.Next(list.Count()));
        }

        /// <summary>
        /// Create string from IEnumerable of chars, because .ToString does not do this
        /// example use:
        /// string source = "hello there";
        /// string fistWord = source.TakeWhile(Char.IsLetterOrDigit).MakeString();
        /// </summary>
        /// <param name="chars">IEnumerable of chars</param>
        /// <returns>string.Concat(chars)</returns>
        public static string MakeString(this IEnumerable<char> chars)
        {
            return string.Concat(chars);
        }

        /// <summary>
        /// Take last x elements from sequence (in order as they are in the sequence)
        /// Ex: collection.TakeLast(5);
        /// </summary>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Create new collection that contains the original input and extends it with "alternative" items.
        /// For example consider having list of file extensions as input:
        /// ["pdf", "doc", "jpg", "jif", "zip"]
        /// and definition of alternatives:
        /// "doc" => ["docx"] // doc is expected to be in input collection, docx is alternative you want to add
        /// "jpg" => ["jpeg" , "jpe" , "jif" , "jfif"] // jpg is in input collection and we want to add jpeg, jpe, jif, jfif
        /// 
        /// Then applying this method on the input collection will result in this new collection:
        /// ["pdf", "doc", "jpg", "jif", "zip", "docx", "jpeg" , "jpe" , "jfif"]
        /// 
        /// </summary>
        /// <typeparam name="T">type must be comparable</typeparam>
        /// <param name="list">input, will not be modified</param>
        /// <param name="alternatives">Key is item from input list and value is one or more alternatives of the same type</param>
        /// <returns>new collection created by concatenation, original colleciton + alternatives</returns>
        public static IEnumerable<T> AddAlternatives<T>(this IEnumerable<T> list, Dictionary<T, IEnumerable<T>> alternatives)
        {
            List<T> add = new();
            var input_uniques = list.Distinct();

            //iterate over the collection we want to extend, but only disctinct
            foreach (var item in input_uniques)
            {
                //if the item has defined altenative(s) 
                if (alternatives.ContainsKey(item))
                {
                    //and they are not present in resulting collection yet, add them
                    foreach (var alternative in alternatives[item])
                    {
                        if (!input_uniques.Contains(alternative)
                            && !add.Contains(alternative)) // also avoid duplicited in what we are adding
                        {
                            add.Add(alternative); // List - modify
                        }

                    }
                }
            }

            //return new collection containing original + alternatives
            return list.Concat(add);
        }
    }

    public static class ListExtensions
    {
        ///Swap two items in List
        static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

    public static class IntExtensions
    {
        public static void Times(this Int32 times, Action<Int32> action)
        {
            for (int i = 0; i < times; i++)
                action(i);
        }
    }

    public static class FloatExtensions
    {
        public static float Round(this float item, int digits)
        {
            return (float)Math.Round(item, digits);
        }
    }

    public static class DoubleExtensions
    {
        public static double Round(this double item, int digits)
        {
            return Math.Round(item, digits);
        }
    }


    public static class StringExtensions
    {
        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Instead of !string.IsNullOrEmpty(str) just do str.IsNotEmpty()
        /// </summary>
        public static bool IsNotEmpty(this string str)
        {

            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Instead of !string.IsNullOrWhiteSpace(str) just do str.IsNotEmptyOrWhiteSpace()
        /// </summary>
        public static bool IsNotNullOrWhiteSpace(this string str)
        {

            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Shortens a string by keeping only the start and end portions, separated by two dots (..).
        /// If the string's length is less than or equal to the specified maximum length * 2, it returns the original string.
        /// </summary>
        /// <param name="str">The string to shorten.</param>
        /// <param name="length">The length of the start and end portions to preserve.</param>
        /// <returns>A shortened string.</returns>
        /// <example>
        /// Given the string "HelloWorld" and a length of 3, the method would return "Hel..rld".
        /// </example>
        public static string ShortenCutMiddle(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Length <= length * 2 ? str : $"{str[..length]}..{str[^length..]}";
        }

        public static string ShortenToLength(this string text, int length)
        {
            if (text != null && text.Length > length)
            {
                return $"{new string(text.Take(length - 3).ToArray())}...";
            }
            else
                return text;
        }

        public static void Println(this string str)
        {
            System.Console.WriteLine(str);
        }
        public static void PrintlnAndPause(this string str)
        {
            System.Console.WriteLine(str);
            System.Console.ReadKey();
        }

        /// <summary>
        /// First line from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetFirstLine(this string str)
        {
            if (str == null) return string.Empty;

            string[] lines = Regex.Split(str, Environment.NewLine);
            if (lines.Length > 0)
                return lines[0];
            else return string.Empty;
        }

        public static string GetFirstWord(this string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                var words = item.Trim().Split(' ');
                return words.First();
            }

            return "";
        }

        /// <summary>
        /// Substring without crashing when not long enough
        /// </summary>
        public static string Substring2(this string str, int startIndex, int length)
        {
            return str.Substring(startIndex, Math.Min(length, str.Length));
        }

        /// <summary>
        /// Split string by new lines
        /// https://stackoverflow.com/questions/1508203/best-way-to-split-string-into-lines/41176852#41176852
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetLines(this string str, bool removeEmptyLines = false)
        {
            using (var sr = new StringReader(str))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (removeEmptyLines && String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    yield return line;
                }
            }
        }

        /// <summary>
        /// "my text".ContainsAny("tems1", "term 2",...)
        /// </summary>
        public static bool ContainsAny(this string value, params string[] pars)
        {
            return pars.Any(p => value.Contains(p));
        }

        /// <summary>
        /// "my text".ContainsAll("tems1", "term 2",...)
        /// </summary>
        public static bool ContainsAll(this string value, params string[] pars)
        {
            return pars.All(p => value.Contains(p));
        }

        /// <summary>
        /// Use at your own risk
        /// </summary>
        public static string RemoveDiacritics(this string value)
        {
            string stFormD = value.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Try to parse string to int with either "." or "," as decimal separator. Must not contain both and must be only once in input string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("use TryParseNumberCzEn")]

        public static int IntParseCzEn(this string value)
        {
            int result = 0;
            string cislo = value;

            bool proslo = int.TryParse(cislo, out result);

            if (proslo)
                return result;
            else
            {
                if (value.Contains(','))
                    cislo = cislo.Replace(',', '.');
                else if (value.Contains('.'))
                    cislo = cislo.Replace('.', ',');

                proslo = int.TryParse(cislo, out result);

                if (proslo)
                    return result;
                else
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Try to parse string to float with either "." or "," as decimal separator. Must not contain both and must be only once in input string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Obsolete("use TryParseNumberCzEn")]
        public static float FloatParseCzEn(this string value)
        {
            float result = 0;
            string cislo = value;

            bool proslo = float.TryParse(cislo, out result);

            if (proslo)
                return result;
            else
            {
                if (value.Contains(','))
                    cislo = cislo.Replace(',', '.');
                else if (value.Contains('.'))
                    cislo = cislo.Replace('.', ',');

                proslo = float.TryParse(cislo, out result);

                if (proslo)
                    return result;
                else
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Parsing numbers as it should be in the framework.
        /// use like this:
        /// var parse = "12.5".TryParseNumber<double>();
        ///  if (parse.success)
        ///  { 
        ///    double x = parse.result;
        ///  }
        /// </summary>
        public static (bool success, T result) TryParseNumber<T>(this string value) where T : INumber<T>
        {
            T result = default(T);
            var succ = T.TryParse(value, CultureInfo.InvariantCulture, out result);
            return (succ, result);
        }

        /// <summary>
        /// Try to parse string to number with either "." or "," as decimal separator. 
        /// Must not contain both and must be only once in input string.
        /// 
        /// </summary>
        /// <typeparam name="T">must implement INumber</typeparam>
        /// <returns>tuple (bool success, T result)</returns>
        /// <exception cref="ArgumentException"></exception>
        public static (bool success, T result) TryParseNumberCzEn<T>(this string value) where T : INumber<T>
        {
            string cislo = value;

            if (value.Contains(','))
                cislo = cislo.Replace(',', '.');

            var tryparse = cislo.TryParseNumber<T>();
            bool proslo = tryparse.success;

            if (proslo)
                return tryparse;
            else
            {
                if (value.Contains(','))
                    cislo = cislo.Replace(',', '.');
                else if (value.Contains('.'))
                    cislo = cislo.Replace('.', ',');

                tryparse = cislo.TryParseNumber<T>();
                proslo = tryparse.success;

                if (proslo)
                    return tryparse;
                else
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Removes characters that are not valid in file names under windows.
        /// Sanitize string to be filename. Removes illegal characters.
        /// Does not deal with resctrictions like lenght or reserved words. Simple removes illegal characters and replaces them with "_"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MakeValidFileName(this string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }

        public static string SanitizeCzech(this string value)
        {
            string notallowed = @"[^a-z0-9áéíóúýčďěňřšťžů:_ \.\,\-\(\)]"; //^ = negace toho co nasleduje a potom vycet znaku ktere chci povolit: a-z 0-9 diakritika podtrzitko pomlcka tecka
            return Regex.Replace(value, notallowed, "_", RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// Only low letters, digits and -
        /// </summary>
        /// <param name="value"></param>
        /// <returns>replaces non valid chars with -</returns>
        public static string SanitizeStrict(this string value, string replacement = "-")
        {
            string notallowed = @"[^a-z0-9\-]"; //^ = negace toho co nasleduje a potom vycet znaku ktere chci povolit: a-z 0-9 diakritika podtrzitko pomlcka tecka
            return Regex.Replace(value, notallowed, replacement, RegexOptions.IgnoreCase);

        }

        /// <summary>
        /// Capitalize first letter
        /// </summary>
        public static string FirstCharToUpper(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input == "")
                throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static IEnumerable<string> SplitLazy(this string toSplit, params char[] splits)
        {
            if (string.IsNullOrEmpty(toSplit))
                yield break;

            StringBuilder sb = new StringBuilder();

            foreach (var c in toSplit)
            {
                if (splits.Contains(c))
                {
                    yield return sb.ToString();
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }


    }

    /// <summary>
    /// Not extension methods, but static helper methods
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Return first string (from parameters) that is not null and not empty
        /// </summary>
        public static string FirstNonEmpty(params string[] strings)
        {
            return strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));
        }

        public static string StringSha256Hash(string text)
        {
            return string.IsNullOrEmpty(text) ? string.Empty
                : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().
                                ComputeHash(System.Text.Encoding.UTF8.GetBytes(text)))
                                .Replace("-", string.Empty);
        }
    }

    /// <summary>
    /// copy directories recusrively
    /// http://stackoverflow.com/questions/2742300/what-is-the-best-way-to-copy-a-folder-and-all-subfolders-and-files-using-c-sharp
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        // Copies all files from one directory to another.
        public static void CopyTo(this DirectoryInfo source,
                string destDirectory, bool recursive)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destDirectory == null)
                throw new ArgumentNullException("destDirectory");
            // If the source doesn't exist, we have to throw an exception.
            if (!source.Exists)
                throw new DirectoryNotFoundException(
                        "Source directory not found: " + source.FullName);
            // Compile the target.
            DirectoryInfo target = new DirectoryInfo(destDirectory);
            // If the target doesn't exist, we create it.
            if (!target.Exists)
                target.Create();
            // Get all files and copy them over.
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }
            // Return if no recursive call is required.
            if (!recursive)
                return;
            // Do the same for all sub directories.
            foreach (DirectoryInfo directory in source.GetDirectories())
            {
                CopyTo(directory,
                    Path.Combine(target.FullName, directory.Name), recursive);
            }
        }

        /// <summary>
        /// Create directory if necessery
        /// </summary>
        /// <returns>the directory path</returns>
        public static string EnsureDirExists(this string dir)
        {
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    public static class Helpers
    {
        /// <summary>
        /// Return first string (from parameters) that is not null and not empty
        /// Example usage: return FirstNonEmpty(CustomColor, Teacher?.Color, "#6F3948");
        /// </summary>
        public static string FirstNonEmpty(params string[] strings)
        {
            return strings.FirstOrDefault(s => !string.IsNullOrEmpty(s));
        }

        /// <summary>
        /// Counts how many Mondays (or Tuesdays,..) is in given date range
        /// </summary>
        /// <param name="day">day of week you want to count</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int CountDays(DayOfWeek day, DateTime startDate, DateTime endDate)
        {
            int cnt = 0;

            for (DateTime dt = startDate; dt < endDate; dt = dt.AddDays(1.0))
            {
                if (dt.DayOfWeek == day)
                {
                    cnt++;
                }
            }

            return cnt;
        }


        /// <summary>
        /// Static method, that tries to parse datetime from string and returns null in case of fail.
        /// Can be used like this:
        /// 
        /// "var dt_or_null = TryParseDate("2011-12-20");
        /// 
        /// this will be either valid datetime or null
        /// so you can also do default value like this:
        /// 
        /// "var dt_or_default = TryParseDate("2011-12-20") ?? new DateTime(2000,1,1);
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? TryParseDate(string value)
        {
            DateTime result;
            if (!DateTime.TryParse(value, out result))
                return null;
            return result;
        }

    }

}

