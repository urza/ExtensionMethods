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
    }

}

