using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CurvaLauncher.Utilities;

static class ScrollViewerUtils
{
    public static bool GetEnableSmoothScrolling(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnableSmoothScrollingProperty);
    }

    public static void SetEnableSmoothScrolling(DependencyObject obj, bool value)
    {
        obj.SetValue(EnableSmoothScrollingProperty, value);
    }

    // Using a DependencyProperty as the backing store for EnableSmoothScrolling.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnableSmoothScrollingProperty =
        DependencyProperty.RegisterAttached("EnableSmoothScrolling", typeof(bool), typeof(ScrollViewerUtils), new PropertyMetadata(false, EnableSmoothScrollingPropertyChangedCallback));

    private static void EnableSmoothScrollingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Control control)
            throw new ArgumentException("Unsupport");


    }
}