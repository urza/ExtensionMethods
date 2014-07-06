using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;

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
                        if(stronglyIncremental)
                        {
                            if (prevItem.CompareTo(nextItem) >= 0)
                                return false;
                        }
                        else
                        {
                            if (prevItem.CompareTo(nextItem) > 0 )
                                return false;
                        }
 		                prevItem = nextItem;
 		            }
 		        }
 		    }

            return true; 
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
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> @enum, Action<T> mapFunction)
        {
            foreach (var item in @enum) mapFunction(item);
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

    public static class ListBoxExtensions
    {
        public static ScrollViewer GetScrollViewer(this ListBox listBox)
        {
            Border scroll_border = VisualTreeHelper.GetChild(listBox, 0) as Border;
            if (scroll_border is Border)
            {
                ScrollViewer scroll = scroll_border.Child as ScrollViewer;
                if (scroll is ScrollViewer)
                {
                    return scroll;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public static class UIExtensions
    {
        /// <summary>
        /// UI Child finder
        /// http://stackoverflow.com/questions/636383/wpf-ways-to-find-controls/1501391#1501391
        /// e.g. :  comboBox.FindChild(typeof(TextBox), "PART_EditableTextBox") as TextBox;
        ///  http://stackoverflow.com/questions/2151285/wpf-selecting-all-the-text-in-and-setting-focus-to-a-comboboxs-editable-textbo
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="childName"></param>
        /// <param name="childType"></param>
        /// <returns></returns>
        public static DependencyObject FindChild(this DependencyObject reference, Type childType, string childName)
        {
            DependencyObject foundChild = null;
            if (reference != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    // If the child is not of the request child type child
                    if (child.GetType() != childType)
                    {
                        // recursively drill down the tree
                        foundChild = FindChild(child, childType, childName);
                    }
                    else if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = child;
                        break;
                    }
                }
            }
            return foundChild;
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
    }

}

