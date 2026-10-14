
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class ReportIssuesForm : Form
    {
        private TextBox txtLocation;
        private ComboBox cmbCategory;
        private RichTextBox rtbDescription;
        private ProgressBar progressDescription;
        private Label lblProgress;
        private Label lblAttachment;
        private Button btnAttach;
        private Button btnSubmit;
        private Button btnBack;
        private Label lblPoints;

        private string attachmentPath = null;

        private static readonly string[] Categories =
        {
            "Pothole",
            "Water Leak",
            "Broken Streetlight",
            "Burst Pipe",
            "Illegal Dumping",
            "Vandalism",
            "Blocked Drain",
            "Noise Complaint",
            "Other"
        };

        public ReportIssuesForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            FontAwesomeHelper.LoadFont();

            // Header
            Panel header = new Panel
            {
                BackColor = Color.FromArgb(0, 102, 204),
                Dock = DockStyle.Top,
                Height = 80
            };

            Label lblTitle = new Label
            {
                Text = "Report an Issue",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 22)
            };

            header.Controls.Add(lblTitle);

            // Location
            Label lblLoc = new Label
            {
                Text = "Location *",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 100)
            };

            txtLocation = new TextBox
            {
                Location = new Point(30, 122),
                Size = new Size(610, 23),
                Font = new Font("Segoe UI", 10F)
            };

            // Category
            Label lblCat = new Label
            {
                Text = "Category *",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 158)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(30, 180),
                Size = new Size(610, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };

            cmbCategory.Items.AddRange(Categories);
            cmbCategory.SelectedIndex = 0;

            // Description
            Label lblDesc = new Label
            {
                Text = "Description * (aim for at least 100 characters)",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 216)
            };

            rtbDescription = new RichTextBox
            {
                Location = new Point(30, 238),
                Size = new Size(610, 180),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            rtbDescription.TextChanged += RtbDescription_TextChanged;

            // Description progress
            progressDescription = new ProgressBar
            {
                Location = new Point(30, 426),
                Size = new Size(610, 14),
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            lblProgress = new Label
            {
                Text = "0% complete",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(0, 102, 204),
                AutoSize = true,
                Location = new Point(30, 444)
            };

            // Attachment button
            btnAttach = new Button
            {
                Text = "Attach Photo / Document",
                Location = new Point(30, 480),
                Size = new Size(180, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };

            btnAttach.FlatAppearance.BorderColor = Color.LightGray;
            btnAttach.Click += BtnAttach_Click;

            lblAttachment = new Label
            {
                Text = "No file attached (optional)",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(222, 489)
            };

            // Submit button
            btnSubmit = new Button
            {
                Text = "Submit Report",
                Location = new Point(30, 536),
                Size = new Size(610, 44),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;

            // Civic points
            lblPoints = new Label
            {
                Text = "Civic points earned this session: 0",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(34, 139, 34),
                AutoSize = true,
                Location = new Point(30, 594)
            };

            // Back button
            btnBack = new Button
            {
                Text = "< Back",
                Location = new Point(490, 594),
                Size = new Size(150, 38),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 102, 204),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnBack.FlatAppearance.BorderColor =
                Color.FromArgb(0, 102, 204);

            btnBack.Click += BtnBack_Click;

            // Add controls
            this.Controls.Add(header);
            this.Controls.Add(lblLoc);
            this.Controls.Add(txtLocation);
            this.Controls.Add(lblCat);
            this.Controls.Add(cmbCategory);
            this.Controls.Add(lblDesc);
            this.Controls.Add(rtbDescription);
            this.Controls.Add(progressDescription);
            this.Controls.Add(lblProgress);
            this.Controls.Add(btnAttach);
            this.Controls.Add(lblAttachment);
            this.Controls.Add(btnSubmit);
            this.Controls.Add(lblPoints);
            this.Controls.Add(btnBack);
        }

        // Return to the previous menu
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Update description progress
        private void RtbDescription_TextChanged(object sender, EventArgs e)
        {
            int pct = Math.Min(
                100,
                rtbDescription.TextLength
            );

            AnimationHelper.AnimateBar(
                progressDescription,
                pct,
                lblProgress
            );
        }

        // Attach a photo or document
        private void BtnAttach_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Attach a photo or document";

                ofd.Filter =
                    "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|" +
                    "Documents|*.pdf;*.doc;*.docx|" +
                    "All Files|*.*";

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    attachmentPath = ofd.FileName;

                    lblAttachment.Text =
                        "Attached: " + Path.GetFileName(attachmentPath);

                    lblAttachment.ForeColor =
                        Color.FromArgb(34, 139, 34);
                }
            }
        }

        // Submit issue report
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                MessageBox.Show(
                    "Please enter the location of the issue.",
                    "Missing Location",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtLocation.Focus();
                return;
            }

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Please select a category.",
                    "Missing Category",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbCategory.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(rtbDescription.Text))
            {
                MessageBox.Show(
                    "Please describe the issue.",
                    "Missing Description",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                rtbDescription.Focus();
                return;
            }

            Issue issue = new Issue
            {
                Location = txtLocation.Text.Trim(),
                Category = cmbCategory.SelectedItem.ToString(),
                Description = rtbDescription.Text.Trim(),
                AttachmentPath = attachmentPath
            };

            int id = IssueManager.AddIssue(issue);

            MessageBox.Show(
                "Issue #" + id + " reported successfully!\n" +
                "You earned " + issue.CivicPointsEarned +
                " civic points.",
                "Thank you, citizen!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            lblPoints.Text =
                "Civic points earned this session: " +
                IssueManager.GetTotalCivicPoints();

            // Reset form
            txtLocation.Clear();
            cmbCategory.SelectedIndex = 0;
            rtbDescription.Clear();

            attachmentPath = null;

            lblAttachment.Text = "No file attached (optional)";
            lblAttachment.ForeColor = Color.Gray;

            AnimationHelper.AnimateBar(
                progressDescription,
                0,
                lblProgress
            );
        }
    }
}
