using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CurvaLauncher.Utilities;

static partial class ScrollViewerUtils
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_ScrollInfo")]
    private extern static IScrollInfo GetScrollInfo(ScrollViewer scrollViewer);

    private static readonly IEasingFunction _scrollingAnimationEase = new CubicEase(){ EasingMode = EasingMode.EaseOut };
    private static readonly Duration _scrollingAnimationDuration = new Duration(TimeSpan.FromMilliseconds(250));

    private const long _millisecondsBetweenTouchpadScrolling = 100;

    public static bool GetEnableSmoothScrolling(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnableSmoothScrollingProperty);
    }

    public static void SetEnableSmoothScrolling(DependencyObject obj, bool value)
    {
        obj.SetValue(EnableSmoothScrollingProperty, value);
    }

    public static SmoothScrollingData? GetSmoothScrollingData(DependencyObject obj)
    {
        return (SmoothScrollingData)obj.GetValue(SmoothScrollingDataProperty);
    }

    public static void SetSmoothScrollingData(DependencyObject obj, SmoothScrollingData? value)
    {
        obj.SetValue(SmoothScrollingDataProperty, value);
    }
    /// <summary>
    /// Get value of VerticalOffset property
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static double GetVerticalOffset(DependencyObject d)
    {
        if (d is ScrollViewer sv)
        {
            return sv.VerticalOffset;
        }
        else if (d is ScrollContentPresenter scp)
        {
            return scp.VerticalOffset;
        }


        return (double)d.GetValue(VerticalOffsetProperty);
    }

    /// <summary>
    /// Set value of VerticalOffset property
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetVerticalOffset(DependencyObject obj, double value)
    {
        obj.SetValue(VerticalOffsetProperty, value);
    }

    /// <summary>
    /// Get value of HorizontalOffset property
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    public static double GetHorizontalOffset(DependencyObject d)
    {
        if (d is ScrollViewer sv)
        {
            return sv.HorizontalOffset;
        }
        else if (d is ScrollContentPresenter scp)
        {
            return scp.HorizontalOffset;
        }

        return (double)d.GetValue(HorizontalOffsetProperty);
    }

    /// <summary>
    /// Set value of HorizontalOffset property
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetHorizontalOffset(DependencyObject obj, double value)
    {
        obj.SetValue(HorizontalOffsetProperty, value);
    }


    private static void VerticalOffsetChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not double offset)
        {
            return;
        }

        if (d is ScrollViewer sv)
        {
            sv.ScrollToVerticalOffset(offset);
        }
        else if (d is ScrollContentPresenter scp)
        {
            scp.SetVerticalOffset(offset);
        }
    }

    private static void HorizontalOffsetChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not double offset)
        {
            return;
        }

        if (d is ScrollViewer sv)
        {
            sv.ScrollToHorizontalOffset(offset);
        }
        else if (d is ScrollContentPresenter scp)
        {
            scp.SetHorizontalOffset(offset);
        }
    }


    // Using a DependencyProperty as the backing store for EnableSmoothScrolling.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty EnableSmoothScrollingProperty =
        DependencyProperty.RegisterAttached("EnableSmoothScrolling", typeof(bool), typeof(ScrollViewerUtils), new PropertyMetadata(false, EnableSmoothScrollingPropertyChangedCallback));

    // Using a DependencyProperty as the backing store for SmoothScrollingData.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SmoothScrollingDataProperty =
        DependencyProperty.RegisterAttached("SmoothScrollingData", typeof(SmoothScrollingData), typeof(ScrollViewerUtils), new PropertyMetadata(null));

    /// <summary>
    /// The DependencyProperty of VerticalOffset property
    /// </summary>
    public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerUtils), new PropertyMetadata(0.0, VerticalOffsetChangedCallback));

    /// <summary>
    /// The DependencyProperty of HorizontalOffset property
    /// </summary>
    public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerUtils), new PropertyMetadata(0.0, HorizontalOffsetChangedCallback));

    private static void EnableSmoothScrollingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Control control)
        {
            throw new InvalidOperationException("Unsupported control");
        }

        if (e.NewValue is true)
        {
            ScrollViewer? scrollViewer =
                d as ScrollViewer ??
                VisualTreeUtils.FindChild<ScrollViewer>(d);

            if (scrollViewer is null)
            {
                RoutedEventHandler doAfterLoaded = null!;
                doAfterLoaded = (s, e) =>
                {
                    ScrollViewer? scrollViewer =
                        d as ScrollViewer ??
                        VisualTreeUtils.FindChild<ScrollViewer>(d);

                    if (scrollViewer is null)
                    {
                        throw new InvalidOperationException("Can not find scroll viewer in specified control");
                    }

                    SetSmoothScrollingData(scrollViewer, new());
                    scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;

                    control.Loaded -= doAfterLoaded;
                };

                control.Loaded += doAfterLoaded;
            }
            else
            {
                SetSmoothScrollingData(scrollViewer, new());
                scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            }
        }
        else
        {
            ScrollViewer? scrollViewer =
                d as ScrollViewer ??
                VisualTreeUtils.FindChild<ScrollViewer>(d);

            if (scrollViewer is null)
            {
                return;
            }

            scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
            SetSmoothScrollingData(scrollViewer, null);
        }
    }

    private static void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer)
        {
            return;
        }

        CoreScrollWithWheelDelta(scrollViewer, e);
    }

    private static void CoreScrollWithWheelDelta(ScrollViewer self, MouseWheelEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }

        if (GetSmoothScrollingData(self) is not SmoothScrollingData scrollingData)
        {
            self.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
            return;
        }

        bool vertical = self.ExtentHeight > 0;
        bool horizontal = self.ExtentWidth > 0;

        var tickCount = Environment.TickCount;
        var isTouchpadScrolling =
                    e.Delta % Mouse.MouseWheelDeltaForOneLine != 0 ||
                    (tickCount - scrollingData.LastScrollingTick < _millisecondsBetweenTouchpadScrolling && scrollingData.LastScrollDelta % Mouse.MouseWheelDeltaForOneLine != 0);

        double scrollDelta = e.Delta;

        if (isTouchpadScrolling)
        {
            // touchpad 应该滚动更慢一些, 所以这里预先除以一个合适的值
            scrollDelta /= 2;

            // 
            //scrollDelta *= 1;
        }
        else
        {
            //scrollDelta *= 1;
        }

        if (vertical)
        {
            if (GetScrollInfo(self) is IScrollInfo scrollInfo)
            {
                // 考虑到 VirtualizingPanel 可能是虚拟的大小, 所以这里需要校正 Delta
                scrollDelta *= scrollInfo.ViewportHeight / (scrollingData.ScrollContentPresenter?.ActualHeight ?? self.ActualHeight);
            }

            var sameDirectionAsLast = Math.Sign(e.Delta) == Math.Sign(scrollingData.LastVerticalScrollingDelta);
            var nowOffset = sameDirectionAsLast && scrollingData.IsAnimationRunning ? scrollingData.VerticalOffsetTarget : self.VerticalOffset;
            var newOffset = nowOffset - scrollDelta;

            if (newOffset < 0)
                newOffset = 0;
            if (newOffset > self.ScrollableHeight)
                newOffset = self.ScrollableHeight;

            scrollingData.VerticalOffsetTarget = newOffset;
            self.BeginAnimation(ScrollViewerUtils.VerticalOffsetProperty, null);

            if (isTouchpadScrolling)
            {
                self.ScrollToVerticalOffset(newOffset);
            }
            else
            {
                var diff = newOffset - self.VerticalOffset;
                var absDiff = Math.Abs(diff);
                var duration = _scrollingAnimationDuration;
                if (absDiff < Mouse.MouseWheelDeltaForOneLine)
                {
                    duration = new Duration(TimeSpan.FromTicks((long)(duration.TimeSpan.Ticks * absDiff / Mouse.MouseWheelDeltaForOneLine)));
                }

                DoubleAnimation doubleAnimation = new DoubleAnimation()
                {
                    EasingFunction = _scrollingAnimationEase,
                    Duration = duration,
                    From = self.VerticalOffset,
                    To = newOffset,
                };

                doubleAnimation.Completed += DoubleAnimation_Completed;

                scrollingData.IsAnimationRunning = true;
                self.BeginAnimation(ScrollViewerUtils.VerticalOffsetProperty, doubleAnimation, HandoffBehavior.SnapshotAndReplace);
            }

            scrollingData.LastVerticalScrollingDelta = e.Delta;
        }
        else if (horizontal)
        {
            if (GetScrollInfo(self) is IScrollInfo scrollInfo)
            {
                // 考虑到 VirtualizingPanel 可能是虚拟的大小, 所以这里需要校正 Delta
                scrollDelta *= scrollInfo.ViewportWidth / (scrollingData.ScrollContentPresenter?.ActualWidth ?? self.ActualWidth);
            }

            var sameDirectionAsLast = Math.Sign(e.Delta) == Math.Sign(scrollingData.LastHorizontalScrollingDelta);
            var nowOffset = sameDirectionAsLast && scrollingData.IsAnimationRunning ? scrollingData.HorizontalOffsetTarget : self.HorizontalOffset;
            var newOffset = nowOffset - scrollDelta;

            if (newOffset < 0)
                newOffset = 0;
            if (newOffset > self.ScrollableWidth)
                newOffset = self.ScrollableWidth;

            scrollingData.HorizontalOffsetTarget = newOffset;
            self.BeginAnimation(ScrollViewerUtils.HorizontalOffsetProperty, null);

            if (isTouchpadScrolling)
            {
                self.ScrollToHorizontalOffset(newOffset);
            }
            else
            {
                var diff = newOffset - self.HorizontalOffset;
                var absDiff = Math.Abs(diff);
                var duration = _scrollingAnimationDuration;
                if (absDiff < Mouse.MouseWheelDeltaForOneLine)
                {
                    duration = new Duration(TimeSpan.FromTicks((long)(duration.TimeSpan.Ticks * absDiff / Mouse.MouseWheelDeltaForOneLine)));
                }

                DoubleAnimation doubleAnimation = new DoubleAnimation()
                {
                    EasingFunction = _scrollingAnimationEase,
                    Duration = duration,
                    From = self.HorizontalOffset,
                    To = newOffset,
                };

                doubleAnimation.Completed += DoubleAnimation_Completed;

                scrollingData.IsAnimationRunning = true;
                self.BeginAnimation(ScrollViewerUtils.HorizontalOffsetProperty, doubleAnimation, HandoffBehavior.SnapshotAndReplace);
            }

            scrollingData.LastHorizontalScrollingDelta = e.Delta;
        }

        scrollingData.LastScrollingTick = tickCount;
        scrollingData.LastScrollDelta = e.Delta;

        e.Handled = true;
    }

    private static void DoubleAnimation_Completed(object? sender, EventArgs e)
    {
        if (sender is not ScrollViewer scrollViewer ||
            GetScrollInfo(scrollViewer) is not SmoothScrollingData scrollingData)
        {
            return;
        }

        scrollingData.IsAnimationRunning = false;
    }
}
