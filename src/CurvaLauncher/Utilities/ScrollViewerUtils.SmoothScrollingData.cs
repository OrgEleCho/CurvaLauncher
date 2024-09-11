using System.Windows;

namespace CurvaLauncher.Utilities;

static partial class ScrollViewerUtils
{
    public record class SmoothScrollingData
    {
        public bool IsAnimationRunning { get; set; }
        public int LastScrollDelta { get; set; }
        public int LastVerticalScrollingDelta { get; set; }
        public int LastHorizontalScrollingDelta { get; set; }
        public long LastScrollingTick { get; set; }
        public double HorizontalOffsetTarget { get; set; }
        public double VerticalOffsetTarget { get; set; }

        public FrameworkElement? ScrollContentPresenter { get; set; }
    }
}
