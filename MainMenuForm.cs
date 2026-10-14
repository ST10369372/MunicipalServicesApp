
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.ClientSize = new Size(600, 520);
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Municipal Services - South Africa";

            // ---- Header ----
            Panel header = new Panel
            {
                BackColor = Color.FromArgb(0, 51, 102),
                Dock = DockStyle.Top,
                Height = 100
            };

            Label lblTitle = new Label
            {
                Text = "Municipal Services",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(24, 20)
            };

            Label lblSub = new Label
            {
                Text = "South Africa - Citizen Portal",
                ForeColor = Color.FromArgb(173, 216, 230),
                Font = new Font("Segoe UI", 11F),
                AutoSize = true,
                Location = new Point(26, 62)
            };

            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSub);

          
            Button btnReport = MakeMenuButton(
                "Report Issues",
                "Log potholes, water leaks, broken streetlights and more.",
                new Point(60, 140),
                Color.FromArgb(0, 102, 204)
            );

            btnReport.Click += btnReportIssues_Click;

           
            Button btnEvents = MakeMenuButton(
                "Local Events and Announcements",
                "Find upcoming community events and announcements.",
                new Point(60, 250),
                Color.FromArgb(0, 153, 76)
            );

            btnEvents.Click += btnLocalEvents_Click;

            
            Button btnStatus = MakeMenuButton(
                "Service Request Status",
                "Track the progress of issues you have reported.",
                new Point(60, 360),
                Color.Gray
            );

            btnStatus.Click += (s, e) =>
            {
                MessageBox.Show(
                    "Service Request Status is coming soon!\n\n" +
                    "This feature is currently under development " +
                    "and will be available in a future update.",
                    "Coming Soon",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            };

           
            this.Controls.Add(header);
            this.Controls.Add(btnReport);
            this.Controls.Add(btnEvents);
            this.Controls.Add(btnStatus);

            // Keep the header at the top
            header.BringToFront();
        }

      
        private void btnReportIssues_Click(object sender, EventArgs e)
        {
            using (ReportIssuesForm form = new ReportIssuesForm())
            {
                OpenForm(form);
            }
        }

        
        private void btnLocalEvents_Click(object sender, EventArgs e)
        {
            using (LocalEventsForm form = new LocalEventsForm())
            {
                OpenForm(form);
            }
        }

      
        private Button MakeMenuButton(
            string title,
            string subtitle,
            Point loc,
            Color accent)
        {
            Button btn = new Button
            {
                Location = loc,
                Size = new Size(480, 90),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 10F),
                Padding = new Padding(14, 0, 0, 0),
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderColor = Color.LightGray;

            
            Label lblButtonTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                AutoSize = true,
                Location = new Point(16, 14),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            Label lblButtonSub = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(17, 50),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

           
            Panel strip = new Panel
            {
                BackColor = accent,
                Size = new Size(6, 90),
                Location = new Point(0, 0),
                Cursor = Cursors.Hand
            };

           
            btn.Controls.Add(lblButtonTitle);
            btn.Controls.Add(lblButtonSub);
            btn.Controls.Add(strip);

            lblButtonTitle.Click += (s, e) => btn.PerformClick();
            lblButtonSub.Click += (s, e) => btn.PerformClick();
            strip.Click += (s, e) => btn.PerformClick();

            return btn;
        }

       
        private void OpenForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterParent;

            // Display the form as a dialog over the main menu
           
            form.ShowDialog(this);
        }
    }
}
