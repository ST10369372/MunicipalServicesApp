using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
namespace MunicipalServicesApp
{
    // ==============================================================
    // MainMenuForm
    //
    // Main application home screen.
    //
    // Options:
    //   a) Report Issues                -> Active
    //   b) Local Events & Announcements -> Coming soon
    //   c) Service Request Status       -> Coming soon
    //
    // Includes:
    //   - Fade-in animation
    //   - Staggered slide-in animation
    //   - Hover effects
    //   - Responsive centering
    //   - Issue count
    //   - Safe form navigation
    // ==============================================================

    public class MainMenuForm : Form
    {
        // ==========================================================
        // CONTROLS
        // ==========================================================

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblPrompt;

        private Button btnReportIssues;
        private Button btnLocalEvents;
        private Button btnServiceStatus;

        private Label lblIssueCount;
        private Label lblFooter;

        private Panel pnlSAFlag;

        // ==========================================================
        // COLOURS
        // ==========================================================

        private static readonly Color ColHeaderTop =
            Color.FromArgb(0, 51, 102);

        private static readonly Color ColHeaderBot =
            Color.FromArgb(0, 82, 165);

        private static readonly Color ColReport =
            Color.FromArgb(0, 102, 204);

        private static readonly Color ColReportHov =
            Color.FromArgb(0, 82, 170);

        private static readonly Color ColDisabled =
            Color.FromArgb(140, 155, 170);

        private static readonly Color ColBackground =
            Color.FromArgb(244, 247, 252);

        private static readonly Color ColFooter =
            Color.FromArgb(215, 228, 248);

        // ==========================================================
        // BUTTON SIZE
        // ==========================================================

        private const int BTN_W = 380;
        private const int BTN_H = 58;

        // ==========================================================
        // ANIMATION
        // ==========================================================

        private Timer _slideTimer;
        private int _slideStep;

        // IMPORTANT:
        // Prevents OnResize from trying to access controls while
        // BuildForm() is still creating them.
        private bool _isBuilding = false;

        // Prevents multiple ReportIssues forms from opening.
        private bool _reportFormOpen = false;

        // ==========================================================
        // CONSTRUCTOR
        // ==========================================================

        public MainMenuForm()
        {
            BuildForm();
        }

        // ==========================================================
        // BUILD FORM
        // ==========================================================

        private void BuildForm()
        {
            _isBuilding = true;

            SuspendLayout();

            try
            {
                // --------------------------------------------------
                // FORM SETTINGS
                // --------------------------------------------------

                Text =
                    "Municipal Services — South Africa";

                Size =
                    new Size(560, 560);

                MinimumSize =
                    new Size(480, 480);

                StartPosition =
                    FormStartPosition.CenterScreen;

                BackColor =
                    ColBackground;

                Font =
                    new Font(
                        "Segoe UI",
                        10F,
                        FontStyle.Regular);

                FormBorderStyle =
                    FormBorderStyle.Sizable;

                Icon =
                    SystemIcons.Application;

                // --------------------------------------------------
                // SOUTH AFRICAN FLAG STRIP
                // --------------------------------------------------

                pnlSAFlag =
                    new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 5,
                        BackColor = Color.White
                    };

                pnlSAFlag.Paint +=
                    PnlSAFlag_Paint;

                // --------------------------------------------------
                // HEADER
                // --------------------------------------------------

                pnlHeader =
                    new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = 130,
                        BackColor = ColHeaderTop
                    };

                pnlHeader.Paint +=
                    PnlHeader_Paint;

                // --------------------------------------------------
                // TITLE
                // --------------------------------------------------

                lblTitle =
                    new Label
                    {
                        Text =
                            "Municipal Services Application",

                        Font =
                            new Font(
                                "Segoe UI",
                                18F,
                                FontStyle.Bold),

                        ForeColor =
                            Color.White,

                        BackColor =
                            Color.Transparent,

                        AutoSize =
                            true,

                        Location =
                            new Point(20, 20)
                    };

                // --------------------------------------------------
                // SUBTITLE
                // --------------------------------------------------

                lblSubtitle =
                    new Label
                    {
                        Text =
                            "South Africa — Citizen Engagement Portal",

                        Font =
                            new Font(
                                "Segoe UI",
                                9.5F,
                                FontStyle.Italic),

                        ForeColor =
                            Color.FromArgb(
                                190,
                                220,
                                255),

                        BackColor =
                            Color.Transparent,

                        AutoSize =
                            true,

                        Location =
                            new Point(22, 65)
                    };

                // --------------------------------------------------
                // PROMPT
                // --------------------------------------------------

                lblPrompt =
                    new Label
                    {
                        Text =
                            "Welcome! Select a service below:",

                        Font =
                            new Font(
                                "Segoe UI",
                                9F,
                                FontStyle.Regular),

                        ForeColor =
                            Color.FromArgb(
                                210,
                                235,
                                255),

                        BackColor =
                            Color.Transparent,

                        AutoSize =
                            true,

                        Location =
                            new Point(22, 98)
                    };

                // --------------------------------------------------
                // ADD HEADER CONTROLS
                // --------------------------------------------------

                pnlHeader.Controls.Add(
                    lblTitle);

                pnlHeader.Controls.Add(
                    lblSubtitle);

                pnlHeader.Controls.Add(
                    lblPrompt);

                // ==================================================
                // REPORT ISSUES BUTTON
                // ==================================================

                btnReportIssues =
                    MakeMenuButton(
                        "a.  Report Issues",
                        "Report local infrastructure and service problems",
                        ColReport,
                        true);

                btnReportIssues.Location =
                    new Point(
                        -BTN_W - 100,
                        160);

                btnReportIssues.Click +=
                    BtnReportIssues_Click;

                // ==================================================
                // LOCAL EVENTS BUTTON
                // ==================================================

                btnLocalEvents =
                    MakeMenuButton(
                        "b.  Local Events & Announcements",
                        "Coming in Part 2 — stay tuned!",
                        ColDisabled,
                        false);

                btnLocalEvents.Location =
                    new Point(
                        -BTN_W - 100,
                        232);

                // ==================================================
                // SERVICE STATUS BUTTON
                // ==================================================

                btnServiceStatus =
                    MakeMenuButton(
                        "c.  Service Request Status",
                        "Coming in Part 2 — stay tuned!",
                        ColDisabled,
                        false);

                btnServiceStatus.Location =
                    new Point(
                        -BTN_W - 100,
                        304);

                // ==================================================
                // ISSUE COUNT
                // ==================================================

                lblIssueCount =
                    new Label
                    {
                        Text =
                            "Issues reported this session:  0",

                        Font =
                            new Font(
                                "Segoe UI",
                                9F,
                                FontStyle.Italic),

                        ForeColor =
                            Color.FromArgb(
                                80,
                                110,
                                150),

                        AutoSize =
                            true,

                        Location =
                            new Point(
                                90,
                                380)
                    };

                // ==================================================
                // FOOTER
                // ==================================================

                lblFooter =
                    new Label
                    {
                        Text =
                            "© 2025 Municipality of Excellence  |  All Rights Reserved",

                        Font =
                            new Font(
                                "Segoe UI",
                                8F,
                                FontStyle.Regular),

                        ForeColor =
                            Color.FromArgb(
                                110,
                                135,
                                170),

                        Dock =
                            DockStyle.Bottom,

                        TextAlign =
                            ContentAlignment.MiddleCenter,

                        Height =
                            30,

                        BackColor =
                            ColFooter
                    };

                // ==================================================
                // ADD CONTROLS TO FORM
                // ==================================================

                Controls.Add(
                    pnlSAFlag);

                Controls.Add(
                    pnlHeader);

                Controls.Add(
                    btnReportIssues);

                Controls.Add(
                    btnLocalEvents);

                Controls.Add(
                    btnServiceStatus);

                Controls.Add(
                    lblIssueCount);

                Controls.Add(
                    lblFooter);
            }
            finally
            {
                ResumeLayout(false);

                PerformLayout();

                // VERY IMPORTANT:
                // All controls now exist before layout is allowed.
                _isBuilding = false;
            }
        }

        // ==========================================================
        // SOUTH AFRICAN FLAG PAINT
        // ==========================================================

        private void PnlSAFlag_Paint(
            object sender,
            PaintEventArgs e)
        {
            if (pnlSAFlag == null)
                return;

            if (pnlSAFlag.IsDisposed)
                return;

            int width =
                pnlSAFlag.ClientSize.Width;

            int height =
                pnlSAFlag.ClientSize.Height;

            if (width <= 0 ||
                height <= 0)
            {
                return;
            }

            Color[] colors =
            {
                Color.FromArgb(0, 122, 77),
                Color.Black,
                Color.FromArgb(0, 50, 160),
                Color.FromArgb(255, 182, 18),
                Color.FromArgb(222, 19, 1),
                Color.White
            };

            int segmentWidth =
                width / colors.Length;

            for (int i = 0;
                 i < colors.Length;
                 i++)
            {
                int x =
                    i * segmentWidth;

                int currentWidth =
                    segmentWidth;

                if (i == colors.Length - 1)
                {
                    currentWidth =
                        width - x;
                }

                using (SolidBrush brush =
                       new SolidBrush(colors[i]))
                {
                    e.Graphics.FillRectangle(
                        brush,
                        x,
                        0,
                        currentWidth,
                        height);
                }
            }
        }

        // ==========================================================
        // MENU BUTTON FACTORY
        // ==========================================================

        private Button MakeMenuButton(
            string text,
            string sub,
            Color color,
            bool enabled)
        {
            Button btn =
                new Button
                {
                    Text =
                        string.Empty,

                    Size =
                        new Size(
                            BTN_W,
                            BTN_H),

                    BackColor =
                        color,

                    ForeColor =
                        Color.White,

                    Font =
                        new Font(
                            "Segoe UI",
                            11F,
                            FontStyle.Bold),

                    FlatStyle =
                        FlatStyle.Flat,

                    Cursor =
                        enabled
                            ? Cursors.Hand
                            : Cursors.Default,

                    Enabled =
                        enabled,

                    Tag =
                        new string[]
                        {
                            text,
                            sub
                        },

                    UseVisualStyleBackColor =
                        false,

                    TabStop =
                        enabled
                };

            btn.FlatAppearance.BorderSize = 0;

            btn.FlatAppearance.BorderColor =
                Color.Transparent;

            // ------------------------------------------------------
            // HOVER
            // ------------------------------------------------------

            if (enabled)
            {
                AnimationHelper.AddHover(
                    btn,
                    color,
                    ColReportHov);
            }

            // ------------------------------------------------------
            // CUSTOM PAINT
            // ------------------------------------------------------

            btn.Paint +=
                (sender, e) =>
                {
                    if (btn.IsDisposed)
                        return;

                    Graphics g =
                        e.Graphics;

                    g.SmoothingMode =
                        SmoothingMode.AntiAlias;

                    string[] labels =
                        btn.Tag as string[];

                    if (labels == null ||
                        labels.Length < 2)
                    {
                        return;
                    }

                    string main =
                        labels[0];

                    string hint =
                        labels[1];

                    // ------------------------------------------------
                    // LEFT ACCENT BAR
                    // ------------------------------------------------

                    using (SolidBrush accentBrush =
                           new SolidBrush(
                               Color.FromArgb(
                                   80,
                                   255,
                                   255,
                                   255)))
                    {
                        g.FillRectangle(
                            accentBrush,
                            0,
                            0,
                            5,
                            btn.Height);
                    }

                    // ------------------------------------------------
                    // MAIN TEXT
                    // ------------------------------------------------

                    using (Font mainFont =
                           new Font(
                               "Segoe UI",
                               11F,
                               FontStyle.Bold))

                    using (SolidBrush mainBrush =
                           new SolidBrush(
                               Color.White))
                    {
                        g.DrawString(
                            main,
                            mainFont,
                            mainBrush,
                            new PointF(
                                18,
                                10));
                    }

                    // ------------------------------------------------
                    // SUB TEXT
                    // ------------------------------------------------

                    using (Font subFont =
                           new Font(
                               "Segoe UI",
                               8F,
                               FontStyle.Italic))

                    using (SolidBrush subBrush =
                           new SolidBrush(
                               Color.FromArgb(
                                   200,
                                   255,
                                   255,
                                   255)))
                    {
                        g.DrawString(
                            hint,
                            subFont,
                            subBrush,
                            new PointF(
                                19,
                                36));
                    }

                    // ------------------------------------------------
                    // ACTIVE BUTTON ARROW
                    // ------------------------------------------------

                    if (btn.Enabled)
                    {
                        using (Font arrowFont =
                               new Font(
                                   "Segoe UI",
                                   16F,
                                   FontStyle.Bold))

                        using (SolidBrush arrowBrush =
                               new SolidBrush(
                                   Color.FromArgb(
                                       140,
                                       255,
                                       255,
                                       255)))
                        {
                            SizeF arrowSize =
                                g.MeasureString(
                                    "›",
                                    arrowFont);

                            g.DrawString(
                                "›",
                                arrowFont,
                                arrowBrush,
                                btn.Width -
                                    arrowSize.Width -
                                    14,
                                (btn.Height -
                                    arrowSize.Height) /
                                    2F);
                        }
                    }
                    else
                    {
                        // ------------------------------------------------
                        // COMING SOON PILL
                        // ------------------------------------------------

                        RectangleF pill =
                            new RectangleF(
                                btn.Width - 110,
                                10,
                                100,
                                20);

                        using (SolidBrush pillBrush =
                               new SolidBrush(
                                   Color.FromArgb(
                                       60,
                                       255,
                                       255,
                                       255)))
                        {
                            g.FillRectangle(
                                pillBrush,
                                pill);
                        }

                        using (Font pillFont =
                               new Font(
                                   "Segoe UI",
                                   7.5F,
                                   FontStyle.Bold))

                        using (SolidBrush pillTextBrush =
                               new SolidBrush(
                                   Color.White))
                        {
                            g.DrawString(
                                "COMING SOON",
                                pillFont,
                                pillTextBrush,
                                pill.X + 6,
                                pill.Y + 3);
                        }
                    }
                };

            // ------------------------------------------------------
            // TOOLTIP
            // ------------------------------------------------------

            ToolTip toolTip =
                new ToolTip
                {
                    AutoPopDelay = 5000,
                    InitialDelay = 300,
                    ReshowDelay = 100
                };

            toolTip.SetToolTip(
                btn,
                sub);

            return btn;
        }

        // ==========================================================
        // HEADER PAINT
        // ==========================================================

        private void PnlHeader_Paint(
            object sender,
            PaintEventArgs e)
        {
            if (pnlHeader == null)
                return;

            if (pnlHeader.IsDisposed)
                return;

            if (pnlHeader.ClientSize.Width <= 0 ||
                pnlHeader.ClientSize.Height <= 0)
            {
                return;
            }

            using (LinearGradientBrush brush =
                   new LinearGradientBrush(
                       pnlHeader.ClientRectangle,
                       ColHeaderTop,
                       ColHeaderBot,
                       LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(
                    brush,
                    pnlHeader.ClientRectangle);
            }

            using (Pen pen =
                   new Pen(
                       Color.FromArgb(
                           0,
                           40,
                           90),
                       2))
            {
                e.Graphics.DrawLine(
                    pen,
                    0,
                    pnlHeader.Height - 1,
                    pnlHeader.Width,
                    pnlHeader.Height - 1);
            }
        }

        // ==========================================================
        // START SLIDE ANIMATION
        // ==========================================================

        private void StartSlideAnimation()
        {
            // ------------------------------------------------------
            // SAFETY CHECK
            // ------------------------------------------------------

            if (_isBuilding)
                return;

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            if (btnReportIssues == null ||
                btnLocalEvents == null ||
                btnServiceStatus == null)
            {
                return;
            }

            // ------------------------------------------------------
            // STOP OLD TIMER
            // ------------------------------------------------------

            StopSlideTimer();

            // ------------------------------------------------------
            // START POSITION
            // ------------------------------------------------------

            int startX =
                -BTN_W - 100;

            btnReportIssues.Left =
                startX;

            btnLocalEvents.Left =
                startX;

            btnServiceStatus.Left =
                startX;

            _slideStep = 0;

            // ------------------------------------------------------
            // CREATE TIMER
            // ------------------------------------------------------

            _slideTimer =
                new Timer
                {
                    Interval = 14
                };

            _slideTimer.Tick +=
                SlideTimer_Tick;

            _slideTimer.Start();
        }

        // ==========================================================
        // SLIDE TIMER
        // ==========================================================

        private void SlideTimer_Tick(
            object sender,
            EventArgs e)
        {
            if (IsDisposed ||
                Disposing)
            {
                StopSlideTimer();
                return;
            }

            if (ClientSize.Width <= 0)
                return;

            // ------------------------------------------------------
            // SAFETY CHECK
            // ------------------------------------------------------

            if (btnReportIssues == null ||
                btnLocalEvents == null ||
                btnServiceStatus == null)
            {
                StopSlideTimer();
                return;
            }

            // ------------------------------------------------------
            // CENTER POSITION
            // ------------------------------------------------------

            int centerX =
                Math.Max(
                    10,
                    (ClientSize.Width - BTN_W) / 2);

            // ------------------------------------------------------
            // BUTTON 1
            // ------------------------------------------------------

            bool reportDone =
                MoveButtonTowards(
                    btnReportIssues,
                    centerX,
                    _slideStep >= 0);

            // ------------------------------------------------------
            // BUTTON 2
            // ------------------------------------------------------

            bool eventsDone =
                MoveButtonTowards(
                    btnLocalEvents,
                    centerX,
                    _slideStep >= 10);

            // ------------------------------------------------------
            // BUTTON 3
            // ------------------------------------------------------

            bool statusDone =
                MoveButtonTowards(
                    btnServiceStatus,
                    centerX,
                    _slideStep >= 20);

            _slideStep++;

            // ------------------------------------------------------
            // FINISHED
            // ------------------------------------------------------

            if (reportDone &&
                eventsDone &&
                statusDone)
            {
                StopSlideTimer();

                CentreAll();
            }
        }

        // ==========================================================
        // MOVE BUTTON
        // ==========================================================

        private bool MoveButtonTowards(
            Button button,
            int targetX,
            bool active)
        {
            if (button == null ||
                button.IsDisposed)
            {
                return true;
            }

            if (!active)
                return false;

            int difference =
                targetX -
                button.Left;

            if (Math.Abs(difference) <= 4)
            {
                button.Left =
                    targetX;

                return true;
            }

            int step =
                Math.Max(
                    6,
                    Math.Abs(difference) / 5);

            if (difference > 0)
            {
                button.Left += step;
            }
            else
            {
                button.Left -= step;
            }

            return false;
        }

        // ==========================================================
        // STOP SLIDE TIMER
        // ==========================================================

        private void StopSlideTimer()
        {
            if (_slideTimer == null)
                return;

            try
            {
                _slideTimer.Stop();

                _slideTimer.Dispose();
            }
            catch
            {
                // Timer may already be disposed.
            }

            _slideTimer = null;
        }

        // ==========================================================
        // CENTRE ALL CONTROLS
        //
        // THIS IS THE MAIN FIX FOR YOUR ERROR.
        //
        // Every control is checked before it is accessed.
        // ==========================================================

        private void CentreAll()
        {
            // ------------------------------------------------------
            // FORM SAFETY
            // ------------------------------------------------------

            if (_isBuilding)
                return;

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            if (ClientSize.Width <= 0)
                return;

            // ------------------------------------------------------
            // CALCULATE CENTER
            // ------------------------------------------------------

            int centerX =
                Math.Max(
                    10,
                    (ClientSize.Width - BTN_W) / 2);

            // ------------------------------------------------------
            // REPORT ISSUES
            // ------------------------------------------------------

            if (btnReportIssues != null &&
                !btnReportIssues.IsDisposed)
            {
                btnReportIssues.Left =
                    centerX;
            }

            // ------------------------------------------------------
            // LOCAL EVENTS
            // ------------------------------------------------------

            if (btnLocalEvents != null &&
                !btnLocalEvents.IsDisposed)
            {
                btnLocalEvents.Left =
                    centerX;
            }

            // ------------------------------------------------------
            // SERVICE STATUS
            // ------------------------------------------------------

            if (btnServiceStatus != null &&
                !btnServiceStatus.IsDisposed)
            {
                btnServiceStatus.Left =
                    centerX;
            }

            // ------------------------------------------------------
            // ISSUE COUNT
            // ------------------------------------------------------

            if (lblIssueCount != null &&
                !lblIssueCount.IsDisposed)
            {
                int labelWidth =
                    lblIssueCount.PreferredWidth;

                lblIssueCount.Left =
                    Math.Max(
                        10,
                        (ClientSize.Width -
                         labelWidth) / 2);
            }
        }

        // ==========================================================
        // FORM LOAD
        // ==========================================================

        protected override void OnLoad(
            EventArgs e)
        {
            base.OnLoad(e);

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            // All controls have been created at this point.
            CentreAll();

            // ------------------------------------------------------
            // FADE IN
            // ------------------------------------------------------

            try
            {
                AnimationHelper.FadeIn(
                    this,
                    0.06,
                    16);
            }
            catch
            {
                // If animation cannot start, simply remain visible.
                Opacity = 1.0;
            }

            // ------------------------------------------------------
            // SLIDE BUTTONS
            // ------------------------------------------------------

            StartSlideAnimation();
        }

        // ==========================================================
        // FORM RESIZE
        // ==========================================================

        protected override void OnResize(
            EventArgs e)
        {
            base.OnResize(e);

            if (_isBuilding)
                return;

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            // While animation is running, the animation controls
            // button positions.
            if (_slideTimer == null)
            {
                CentreAll();
            }
        }

        // ==========================================================
        // REPORT ISSUES
        // ==========================================================

        private void BtnReportIssues_Click(
            object sender,
            EventArgs e)
        {
            // ------------------------------------------------------
            // PREVENT MULTIPLE WINDOWS
            // ------------------------------------------------------

            if (_reportFormOpen)
                return;

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            _reportFormOpen = true;

            // ------------------------------------------------------
            // DISABLE BUTTON
            // ------------------------------------------------------

            if (btnReportIssues != null &&
                !btnReportIssues.IsDisposed)
            {
                btnReportIssues.Enabled = false;
            }

            // ------------------------------------------------------
            // HIDE MAIN MENU
            // ------------------------------------------------------

            Hide();

            ReportIssuesForm reportForm =
                null;

            try
            {
                reportForm =
                    new ReportIssuesForm();

                reportForm.FormClosed +=
                    ReportForm_FormClosed;

                reportForm.Show(this);
            }
            catch (Exception ex)
            {
                _reportFormOpen = false;

                Show();

                if (btnReportIssues != null &&
                    !btnReportIssues.IsDisposed)
                {
                    btnReportIssues.Enabled = true;
                }

                MessageBox.Show(
                    this,
                    "Unable to open the Report Issues page.\n\n" +
                    ex.Message,
                    "Application Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ==========================================================
        // REPORT FORM CLOSED
        // ==========================================================

        private void ReportForm_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            _reportFormOpen = false;

            if (IsDisposed ||
                Disposing)
            {
                return;
            }

            // ------------------------------------------------------
            // UPDATE ISSUE COUNT
            // ------------------------------------------------------

            if (lblIssueCount != null &&
                !lblIssueCount.IsDisposed)
            {
                lblIssueCount.Text =
                    "Issues reported this session:  " +
                    IssueDataStore.Count;
            }

            // ------------------------------------------------------
            // CENTER CONTROLS
            // ------------------------------------------------------

            CentreAll();

            // ------------------------------------------------------
            // SHOW MAIN FORM
            // ------------------------------------------------------

            Show();

            // ------------------------------------------------------
            // ENABLE REPORT BUTTON
            // ------------------------------------------------------

            if (btnReportIssues != null &&
                !btnReportIssues.IsDisposed)
            {
                btnReportIssues.Enabled = true;
            }

            // ------------------------------------------------------
            // FADE BACK IN
            // ------------------------------------------------------

            try
            {
                AnimationHelper.FadeIn(
                    this,
                    0.08,
                    16);
            }
            catch
            {
                Opacity = 1.0;
            }
        }

        // ==========================================================
        // DISPOSE
        // ==========================================================

        protected override void Dispose(
            bool disposing)
        {
            if (disposing)
            {
                StopSlideTimer();
            }

            base.Dispose(disposing);
        }
    }
}

