using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public class ReportIssuesForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            uint message,
            IntPtr wParam,
            string lParam);

        private const uint EM_SETCUEBANNER = 0x1501;

        private Panel pnlSAFlag;
        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;

        private Panel pnlEngagement;
        private Label lblEngIcon;
        private Label lblEngMsg;
        private ProgressBar progressBar;
        private Label lblPct;

        private Panel pnlBody;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblDescription;
        private RichTextBox rtbDescription;
        private Label lblCharCount;
        private Label lblAttachment;
        private Button btnAttachMedia;
        private Label lblFileName;
        private Panel pnlButtons;
        private Button btnSubmit;
        private Button btnBack;
        private Button btnViewReports;

        private static readonly Color ColHeaderTop = Color.FromArgb(0, 51, 102);
        private static readonly Color ColHeaderBot = Color.FromArgb(0, 82, 165);
        private static readonly Color ColEngBg = Color.FromArgb(235, 245, 255);
        private static readonly Color ColEngText = Color.FromArgb(0, 82, 165);
        private static readonly Color ColBackground = Color.FromArgb(244, 247, 252);
        private static readonly Color ColLabelFg = Color.FromArgb(30, 40, 60);
        private static readonly Color ColSubmit = Color.FromArgb(0, 153, 51);
        private static readonly Color ColSubmitHov = Color.FromArgb(0, 128, 40);
        private static readonly Color ColBack = Color.FromArgb(96, 125, 139);
        private static readonly Color ColBackHov = Color.FromArgb(72, 95, 108);
        private static readonly Color ColAttach = Color.FromArgb(0, 102, 204);
        private static readonly Color ColAttachHov = Color.FromArgb(0, 82, 170);
        private static readonly Color ColViewRep = Color.FromArgb(204, 102, 0);
        private static readonly Color ColViewRepHov = Color.FromArgb(170, 82, 0);

        private static readonly string[] EngMessages =
        {
            "Your report helps improve your community!",
            "Every issue reported makes a real difference!",
            "Together we build a better city for everyone!",
            "Your voice matters — keep going!",
            "Thank you for being an active citizen!"
        };

        private string _attachedFilePath = string.Empty;
        private int _engIndex;
        private Timer _engTimer;
        private bool _isSubmitting;

        public ReportIssuesForm()
        {
            BuildForm();
            StartEngagementTimer();
        }

        private void BuildForm()
        {
            SuspendLayout();

            Text = "Report an Issue — Municipal Services";
            ClientSize = new Size(700, 720);
            MinimumSize = new Size(580, 620);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ColBackground;
            Font = new Font("Segoe UI", 10F);
            AutoScroll = false;

            BuildSAFlag();
            BuildHeader();
            BuildEngagementStrip();
            BuildBody();

            ResumeLayout(false);
            PerformLayout();
        }

        private void BuildSAFlag()
        {
            pnlSAFlag = new Panel
            {
                Dock = DockStyle.Top,
                Height = 5
            };

            pnlSAFlag.Paint += (s, e) =>
            {
                Color[] colors =
                {
                    Color.FromArgb(0, 122, 77),
                    Color.Black,
                    Color.FromArgb(0, 50, 160),
                    Color.FromArgb(255, 182, 18),
                    Color.FromArgb(222, 19, 1),
                    Color.White
                };

                int width = pnlSAFlag.Width;
                int segment = Math.Max(1, width / colors.Length);

                for (int i = 0; i < colors.Length; i++)
                {
                    int x = i * segment;
                    int w = i == colors.Length - 1
                        ? width - x
                        : segment;

                    using (SolidBrush brush = new SolidBrush(colors[i]))
                        e.Graphics.FillRectangle(
                            brush, x, 0, w, pnlSAFlag.Height);
                }
            };

            Controls.Add(pnlSAFlag);
        }

        private void BuildHeader()
        {
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 86,
                BackColor = ColHeaderTop
            };
            pnlHeader.Paint += PnlHeader_Paint;

            lblTitle = new Label
            {
                Text = "Report a Municipal Issue",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(20, 16)
            };

            lblSubtitle = new Label
            {
                Text = "Help us keep your community running smoothly — every report counts!",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(190, 220, 255),
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(22, 56)
            };

            pnlHeader.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblSubtitle
            });

            Controls.Add(pnlHeader);
        }

        private void BuildEngagementStrip()
        {
            pnlEngagement = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = ColEngBg
            };

            lblEngIcon = new Label
            {
                Text = "★",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 182, 18),
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(14, 20)
            };

            lblEngMsg = new Label
            {
                Text = EngMessages[0],
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ColEngText,
                BackColor = Color.Transparent,
                Location = new Point(46, 10),
                Size = new Size(500, 22),
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };

            progressBar = new ProgressBar
            {
                Location = new Point(46, 38),
                Size = new Size(530, 20),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous,
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };

            lblPct = new Label
            {
                Text = "0% complete",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = ColEngText,
                BackColor = Color.Transparent,
                Location = new Point(46, 61),
                Size = new Size(200, 14),
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };

            pnlEngagement.Controls.AddRange(new Control[]
            {
                lblEngIcon,
                lblEngMsg,
                progressBar,
                lblPct
            });

            Controls.Add(pnlEngagement);
        }

        private void BuildBody()
        {
            pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColBackground
            };

            const int LX = 32;
            const int FW = 590;
            int y = 20;

            lblLocation = MkLabel("a)  Location of Issue", LX, y);
            y += 28;

            txtLocation = new TextBox
            {
                Location = new Point(LX, y),
                Size = new Size(FW, 30),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };
            txtLocation.TextChanged += OnField_Changed;
            y += 50;

            lblCategory = MkLabel("b)  Category", LX, y);
            y += 28;

            cmbCategory = new ComboBox
            {
                Location = new Point(LX, y),
                Size = new Size(FW, 30),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.White,
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };

            cmbCategory.Items.AddRange(new object[]
            {
                "Sanitation",
                "Roads and Potholes",
                "Water and Utilities",
                "Electricity",
                "Waste Management",
                "Public Safety",
                "Parks and Recreation",
                "Street Lighting",
                "Other"
            });

            cmbCategory.SelectedIndex = 0;
            cmbCategory.SelectedIndexChanged += OnField_Changed;
            y += 50;

            lblDescription = MkLabel("c)  Detailed Description", LX, y);
            y += 28;

            rtbDescription = new RichTextBox
            {
                Location = new Point(LX, y),
                Size = new Size(FW, 145),
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };
            rtbDescription.TextChanged += OnField_Changed;

            lblCharCount = new Label
            {
                Text = "0 characters  (minimum 10 required)",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(LX, y + 150),
                Size = new Size(FW, 18),
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };

            y += 185;

            lblAttachment = MkLabel(
                "d)  Attach Media  (Image or Document)", LX, y);
            y += 28;

            btnAttachMedia = MkActionButton(
                "Browse for File...",
                LX,
                y,
                180,
                36,
                ColAttach,
                ColAttachHov);
            btnAttachMedia.Click += BtnAttachMedia_Click;

            lblFileName = new Label
            {
                Text = "No file selected",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(LX + 192, y + 9),
                Size = new Size(FW - 195, 20),
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top,
                AutoEllipsis = true
            };

            y += 60;

            pnlButtons = new Panel
            {
                Location = new Point(LX, y),
                Size = new Size(FW, 50),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left | AnchorStyles.Right |
                         AnchorStyles.Top
            };

            btnSubmit = MkActionButton(
                "✔  Submit Report",
                0,
                0,
                200,
                46,
                ColSubmit,
                ColSubmitHov);
            btnSubmit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnSubmit.Click += BtnSubmit_Click;

            btnViewReports = MkActionButton(
                "View All Reports",
                212,
                0,
                160,
                46,
                ColViewRep,
                ColViewRepHov);
            btnViewReports.Click += BtnViewReports_Click;

            btnBack = MkActionButton(
                "← Back to Menu",
                384,
                0,
                165,
                46,
                ColBack,
                ColBackHov);
            btnBack.Click += BtnBack_Click;

            pnlButtons.Controls.AddRange(new Control[]
            {
                btnSubmit,
                btnViewReports,
                btnBack
            });

            pnlBody.Controls.AddRange(new Control[]
            {
                lblLocation,
                txtLocation,
                lblCategory,
                cmbCategory,
                lblDescription,
                rtbDescription,
                lblCharCount,
                lblAttachment,
                btnAttachMedia,
                lblFileName,
                pnlButtons
            });

            Controls.Add(pnlBody);
        }

        private Label MkLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ColLabelFg,
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private Button MkActionButton(
            string text,
            int x,
            int y,
            int width,
            int height,
            Color normal,
            Color hover)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = normal,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.BorderColor = Color.Transparent;

            AnimationHelper.AddHover(button, normal, hover);
            return button;
        }

        private void PnlHeader_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                pnlHeader.ClientRectangle,
                ColHeaderTop,
                ColHeaderBot,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(
                    brush,
                    pnlHeader.ClientRectangle);
            }

            using (Pen pen = new Pen(Color.FromArgb(0, 40, 90), 2))
            {
                e.Graphics.DrawLine(
                    pen,
                    0,
                    pnlHeader.Height - 1,
                    pnlHeader.Width,
                    pnlHeader.Height - 1);
            }
        }

        private void StartEngagementTimer()
        {
            _engTimer = new Timer { Interval = 4000 };

            _engTimer.Tick += (s, e) =>
            {
                _engIndex = (_engIndex + 1) % EngMessages.Length;

                AnimationHelper.FadeLabelText(
                    lblEngMsg,
                    EngMessages[_engIndex],
                    ColEngText);
            };

            _engTimer.Start();
        }

        private void OnField_Changed(object sender, EventArgs e)
        {
            if (rtbDescription == null ||
                txtLocation == null ||
                progressBar == null)
                return;

            int chars = rtbDescription.TextLength;
            bool descOk = chars >= 10;

            lblCharCount.Text = descOk
                ? $"{chars} characters  ✓  Ready"
                : $"{chars} characters  (minimum 10 required)";

            lblCharCount.ForeColor = descOk
                ? Color.FromArgb(0, 128, 0)
                : Color.Gray;

            bool locOk = !string.IsNullOrWhiteSpace(txtLocation.Text);

            int filled = (locOk ? 1 : 0) + (descOk ? 1 : 0);
            int target = filled * 50;

            AnimationHelper.AnimateBar(
                progressBar,
                target,
                lblPct);
        }

        private void BtnAttachMedia_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select an Image or Document",
                Filter =
                    "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|" +
                    "Document Files|*.pdf;*.doc;*.docx|" +
                    "All Files|*.*",
                InitialDirectory =
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyPictures)
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                _attachedFilePath = dialog.FileName;

                long kb = Math.Max(
                    1,
                    new FileInfo(dialog.FileName).Length / 1024);

                lblFileName.Text =
                    $"✓  {Path.GetFileName(dialog.FileName)}  ({kb} KB)";
                lblFileName.ForeColor = Color.FromArgb(0, 128, 0);

                btnAttachMedia.Text = "Change File...";
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (_isSubmitting)
                return;

            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                ShowError(
                    "Please enter the location of the issue.",
                    txtLocation);
                AnimationHelper.FlashButton(
                    btnSubmit,
                    Color.FromArgb(210, 50, 50),
                    2,
                    110);
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                ShowError(
                    "Please select a category.",
                    cmbCategory);
                return;
            }

            if (rtbDescription.TextLength < 10)
            {
                ShowError(
                    "Please provide a description of at least 10 characters.",
                    rtbDescription);
                AnimationHelper.FlashButton(
                    btnSubmit,
                    Color.FromArgb(210, 50, 50),
                    2,
                    110);
                return;
            }

            _isSubmitting = true;
            btnSubmit.Enabled = false;

            try
            {
                Issue issue = new Issue(
                    txtLocation.Text.Trim(),
                    cmbCategory.SelectedItem?.ToString() ?? "Other",
                    rtbDescription.Text.Trim(),
                    _attachedFilePath);

                IssueDataStore.Add(issue);

                AnimationHelper.FlashButton(
                    btnSubmit,
                    Color.FromArgb(80, 220, 100),
                    3);

                AnimationHelper.AnimateBar(
                    progressBar,
                    100,
                    lblPct);

                AnimationHelper.FadeLabelText(
                    lblEngMsg,
                    "Thank you! Your report has been submitted!",
                    ColEngText);

                string attachment = string.IsNullOrWhiteSpace(
                    issue.MediaFilePath)
                    ? "None"
                    : Path.GetFileName(issue.MediaFilePath);

                MessageBox.Show(
                    this,
                    $"Issue reported successfully!\n\n" +
                    $"Reference  :  {issue.ReferenceCode}\n" +
                    $"Category   :  {issue.Category}\n" +
                    $"Location   :  {issue.Location}\n" +
                    $"Attachment :  {attachment}\n" +
                    $"Date       :  {issue.ReportedDate:dd MMM yyyy  HH:mm}\n\n" +
                    $"Total issues reported this session:  " +
                    $"{IssueDataStore.Count}",
                    "Submitted Successfully",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
            }
            finally
            {
                _isSubmitting = false;
                btnSubmit.Enabled = true;
            }
        }

        private void BtnViewReports_Click(object sender, EventArgs e)
        {
            if (IssueDataStore.Count == 0)
            {
                MessageBox.Show(
                    this,
                    "No issues have been reported yet.\n" +
                    "Submit your first report to get started!",
                    "No Reports",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            System.Text.StringBuilder builder =
                new System.Text.StringBuilder();

            builder.AppendLine(
                $"{"Reference",-24} {"Category",-22} " +
                $"{"Location",-28} {"Status",-10} Date");
            builder.AppendLine(new string('─', 105));

            foreach (Issue issue in IssueDataStore.Issues)
            {
                builder.AppendLine(
                    $"{issue.ReferenceCode,-24} " +
                    $"{TrimForTable(issue.Category, 22),-22} " +
                    $"{TrimForTable(issue.Location, 28),-28} " +
                    $"{issue.Status,-10} " +
                    $"{issue.ReportedDate:dd MMM yyyy HH:mm}");
            }

            builder.AppendLine();
            builder.AppendLine(
                $"Total: {IssueDataStore.Count} issue(s) reported this session.");

            MessageBox.Show(
                this,
                builder.ToString(),
                "Reported Issues",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private static string TrimForTable(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length <= maxLength)
                return value;

            if (maxLength <= 3)
                return value.Substring(0, maxLength);

            return value.Substring(0, maxLength - 3) + "...";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowError(string message, Control focus)
        {
            MessageBox.Show(
                this,
                message,
                "Validation Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            focus?.Focus();
        }

        private void ClearForm()
        {
            txtLocation.Clear();
            cmbCategory.SelectedIndex = 0;
            rtbDescription.Clear();

            _attachedFilePath = string.Empty;

            lblFileName.Text = "No file selected";
            lblFileName.ForeColor = Color.Gray;

            btnAttachMedia.Text = "Browse for File...";

            lblCharCount.Text =
                "0 characters  (minimum 10 required)";
            lblCharCount.ForeColor = Color.Gray;

            progressBar.Value = 0;
            lblPct.Text = "0% complete";
            lblPct.ForeColor = ColEngText;

            txtLocation.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Opacity = 0;
            AnimationHelper.FadeIn(this);

            if (txtLocation.IsHandleCreated)
            {
                SendMessage(
                    txtLocation.Handle,
                    EM_SETCUEBANNER,
                    (IntPtr)1,
                    "e.g.  12 Oak Street, Soweto or Corner of Bree & Plein, Cape Town");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (progressBar != null && pnlEngagement != null)
            {
                progressBar.Width =
                    Math.Max(200,
                        pnlEngagement.ClientSize.Width - 130);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_engTimer != null)
            {
                _engTimer.Stop();
                _engTimer.Dispose();
                _engTimer = null;
            }

            base.OnFormClosing(e);
        }
    }
}
