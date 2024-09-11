using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Utilities
{
    internal static class VisualTreeUtils
    {
        public static T? FindChild<T>(DependencyObject reference)
            where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(reference);
            for (int i = 0; i < count; i++)
            {
                var currentChild = VisualTreeHelper.GetChild(reference, i);
                if (currentChild is T foundChild)
                {
                    return foundChild;
                }
                else if (FindChild<T>(currentChild) is T foundSubChild)
                {
                    return foundSubChild;
                }
            }

            return null;
        }
    }
}
