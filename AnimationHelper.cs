using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public static class AnimationHelper
    {
        public static void FadeIn(Form form, double step = 0.06, int intervalMs = 16)
        {
            if (form == null || form.IsDisposed) return;
            form.Opacity = 0.0;
            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };
            timer.Tick += (s, e) =>
            {
                if (form.IsDisposed) { timer.Stop(); timer.Dispose(); return; }
                form.Opacity = Math.Min(1.0, form.Opacity + step);
                if (form.Opacity >= 1.0) { timer.Stop(); timer.Dispose(); }
            };
            timer.Start();
        }

        public static void AnimateBar(ProgressBar bar, int target, Label label = null, int intervalMs = 16)
        {
            if (bar == null || bar.IsDisposed) return;
            target = Math.Max(bar.Minimum, Math.Min(bar.Maximum, target));
            if (bar.Tag is Timer oldTimer) { oldTimer.Stop(); oldTimer.Dispose(); }
            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };
            bar.Tag = timer;
            timer.Tick += (s, e) =>
            {
                if (bar.IsDisposed)
                {
                    timer.Stop(); timer.Dispose();
                    if (ReferenceEquals(bar.Tag, timer)) bar.Tag = null;
                    return;
                }
                if (bar.Value == target)
                {
                    UpdateLabel(label, bar.Value);
                    timer.Stop(); timer.Dispose();
                    if (ReferenceEquals(bar.Tag, timer)) bar.Tag = null;
                    return;
                }
                int diff = target - bar.Value;
                int step = Math.Max(1, Math.Abs(diff) / 5);
                int next = bar.Value + (diff > 0 ? step : -step);
                next = Math.Max(bar.Minimum, Math.Min(bar.Maximum, next));
                bar.Value = next;
                UpdateLabel(label, bar.Value);
            };
            timer.Start();
        }

        private static void UpdateLabel(Label label, int value)
        {
            if (label == null || label.IsDisposed) return;
            if (value >= 100)
            {
                label.Text = "Ready to submit!";
                label.ForeColor = Color.FromArgb(34, 139, 34);
            }
            else
            {
                label.Text = string.Format("{0}% complete", value);
                label.ForeColor = Color.FromArgb(0, 102, 204);
            }
        }

        public static Color Blend(Color c1, Color c2, double ratio)
        {
            ratio = Math.Max(0.0, Math.Min(1.0, ratio));
            return Color.FromArgb(
                (int)(c1.R + (c2.R - c1.R) * ratio),
                (int)(c1.G + (c2.G - c1.G) * ratio),
                (int)(c1.B + (c2.B - c1.B) * ratio));
        }
    }
}
