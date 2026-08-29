using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public static class AnimationHelper
    {
        public static void FadeIn(Form form, double step = 0.06, int intervalMs = 16)
        {
            if (form == null || form.IsDisposed)
                return;

            form.Opacity = 0.0;

            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };

            timer.Tick += (s, e) =>
            {
                if (form.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                form.Opacity = Math.Min(1.0, form.Opacity + step);

                if (form.Opacity >= 1.0)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        public static void SlideInFromLeft(Control control, int targetX, int intervalMs = 16)
        {
            if (control == null || control.IsDisposed)
                return;

            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };

            timer.Tick += (s, e) =>
            {
                if (control.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                int diff = targetX - control.Left;

                if (Math.Abs(diff) <= 3)
                {
                    control.Left = targetX;
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                int step = Math.Max(5, Math.Abs(diff) / 5);
                control.Left += diff > 0 ? step : -step;
            };

            timer.Start();
        }

        public static void AnimateBar(
            ProgressBar bar,
            int target,
            Label label = null,
            int intervalMs = 16)
        {
            if (bar == null || bar.IsDisposed)
                return;

            target = Math.Max(bar.Minimum, Math.Min(bar.Maximum, target));

            // Stop a previous animation on this ProgressBar.
            if (bar.Tag is Timer oldTimer)
            {
                oldTimer.Stop();
                oldTimer.Dispose();
            }

            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };
            bar.Tag = timer;

            timer.Tick += (s, e) =>
            {
                if (bar.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    if (ReferenceEquals(bar.Tag, timer))
                        bar.Tag = null;
                    return;
                }

                if (bar.Value == target)
                {
                    UpdateLabel(label, bar.Value);
                    timer.Stop();
                    timer.Dispose();
                    if (ReferenceEquals(bar.Tag, timer))
                        bar.Tag = null;
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
            if (label == null || label.IsDisposed)
                return;

            if (value >= 100)
            {
                label.Text = "✓ Ready to submit!";
                label.ForeColor = Color.FromArgb(34, 139, 34);
            }
            else
            {
                label.Text = $"{value}% complete";
                label.ForeColor = Color.FromArgb(0, 102, 204);
            }
        }

        public static void FlashButton(
            Button button,
            Color flash,
            int times = 3,
            int intervalMs = 130)
        {
            if (button == null || button.IsDisposed)
                return;

            Color original = button.BackColor;
            int count = 0;
            bool lit = false;

            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };

            timer.Tick += (s, e) =>
            {
                if (button.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                lit = !lit;
                button.BackColor = lit ? flash : original;

                if (!lit)
                {
                    count++;

                    if (count >= Math.Max(1, times))
                    {
                        button.BackColor = original;
                        timer.Stop();
                        timer.Dispose();
                    }
                }
            };

            timer.Start();
        }

        public static void FadeLabelText(
            Label label,
            string newText,
            Color targetColor,
            int steps = 10,
            int intervalMs = 25)
        {
            if (label == null || label.IsDisposed)
                return;

            steps = Math.Max(1, steps);
            Color background = label.Parent?.BackColor ?? SystemColors.Control;
            int step = 0;
            bool fadingIn = false;

            Timer timer = null;
            timer = new Timer { Interval = Math.Max(1, intervalMs) };

            timer.Tick += (s, e) =>
            {
                if (label.IsDisposed)
                {
                    timer.Stop();
                    timer.Dispose();
                    return;
                }

                if (!fadingIn)
                {
                    double ratio = 1.0 - ((double)step / steps);
                    label.ForeColor = Blend(background, targetColor, ratio);
                    step++;

                    if (step > steps)
                    {
                        label.Text = newText;
                        step = 0;
                        fadingIn = true;
                    }
                }
                else
                {
                    double ratio = (double)step / steps;
                    label.ForeColor = Blend(background, targetColor, ratio);
                    step++;

                    if (step > steps)
                    {
                        label.ForeColor = targetColor;
                        timer.Stop();
                        timer.Dispose();
                    }
                }
            };

            timer.Start();
        }

        public static void AddHover(Button button, Color normal, Color hover)
        {
            if (button == null)
                return;

            button.BackColor = normal;

            button.MouseEnter += (s, e) =>
            {
                if (button.Enabled)
                    button.BackColor = hover;
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = normal;
            };

            button.EnabledChanged += (s, e) =>
            {
                if (!button.Enabled)
                    button.BackColor = SystemColors.ControlDark;
                else
                    button.BackColor = normal;
            };
        }

        public static Color Blend(Color c1, Color c2, double ratio)
        {
            ratio = Math.Max(0.0, Math.Min(1.0, ratio));

            return Color.FromArgb(
                (int)(c1.R + (c2.R - c1.R) * ratio),
                (int)(c1.G + (c2.G - c1.G) * ratio),
                (int)(c1.B + (c2.B - c1.B) * ratio));
        }

        public static Color Lighten(Color color, int by = 30)
        {
            return Color.FromArgb(
                Math.Min(255, color.R + by),
                Math.Min(255, color.G + by),
                Math.Min(255, color.B + by));
        }
    }
}
