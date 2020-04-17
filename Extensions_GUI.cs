using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExtensionMethods
{
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
}

