
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public partial class LocalEventsForm : Form
    {
        private TextBox txtSearch;
        private ComboBox cmbCategory;
        private DateTimePicker dtpDate;
        private CheckBox chkFilterDate;
        private ListBox lstResults;
        private Label lblRecommendations;
        private ListBox lstRecommendations;
        private Label lblAnnouncements;
        private Button btnBack;

        private Timer announcementTimer;

        public LocalEventsForm()
        {
            InitializeComponent();
            SetupForm();

            Load += LocalEventsForm_Load;
        }

        private void SetupForm()
        {
            this.ClientSize = new Size(940, 640);
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Local Events and Announcements";

            // Header
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                BackColor = Color.FromArgb(0, 102, 51)
            };

            Label lblTitle = new Label
            {
                Text = "Local Events and Announcements",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 20)
            };

            header.Controls.Add(lblTitle);

            // Search panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 90),
                Size = new Size(280, 420),
                BackColor = Color.White
            };

            searchPanel.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    searchPanel.ClientRectangle,
                    Color.LightGray,
                    ButtonBorderStyle.Solid
                );
            };

            Label lblSearch = new Label
            {
                Text = "Find Events",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 14)
            };

            Label lblK = new Label
            {
                Text = "Keyword",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(16, 52)
            };

            txtSearch = new TextBox
            {
                Location = new Point(16, 70),
                Size = new Size(248, 23)
            };

            Label lblC = new Label
            {
                Text = "Category",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(16, 104)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(16, 122),
                Size = new Size(248, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            chkFilterDate = new CheckBox
            {
                Text = "Filter by specific date",
                Location = new Point(16, 158),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            dtpDate = new DateTimePicker
            {
                Location = new Point(16, 182),
                Size = new Size(248, 23),
                Format = DateTimePickerFormat.Short,
                Enabled = false
            };

            chkFilterDate.CheckedChanged += (s, e) =>
            {
                dtpDate.Enabled = chkFilterDate.Checked;
            };

            Button btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(16, 224),
                Size = new Size(248, 36),
                BackColor = Color.FromArgb(0, 102, 51),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Click += BtnSearch_Click;

            Label lblTip = new Label
            {
                Text =
                    "Search by keyword, category or date.\n" +
                    "Results and recommendations appear below.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(16, 276),
                Size = new Size(248, 60)
            };

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(lblK);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(lblC);
            searchPanel.Controls.Add(cmbCategory);
            searchPanel.Controls.Add(chkFilterDate);
            searchPanel.Controls.Add(dtpDate);
            searchPanel.Controls.Add(btnSearch);
            searchPanel.Controls.Add(lblTip);

            // Upcoming events
            Label lblResults = new Label
            {
                Text = "Upcoming Events",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(320, 94)
            };

            lstResults = new ListBox
            {
                Location = new Point(320, 122),
                Size = new Size(600, 240),
                Font = new Font("Consolas", 9.5F),
                IntegralHeight = false
            };

            lstResults.SelectedIndexChanged +=
                LstResults_SelectedIndexChanged;

            // Announcements
            lblAnnouncements = new Label
            {
                Text = "Announcements",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(180, 90, 0),
                AutoSize = true,
                Location = new Point(320, 372)
            };

            // Recommendations
            lblRecommendations = new Label
            {
                Text = "Recommended for you (based on your searches)",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 524)
            };

            lstRecommendations = new ListBox
            {
                Location = new Point(20, 550),
                Size = new Size(900, 72),
                Font = new Font("Segoe UI", 9F),
                IntegralHeight = false,
                ForeColor = Color.FromArgb(0, 51, 102)
            };

            // Back button
            btnBack = new Button
            {
                Text = "< Back to Main Menu",
                Location = new Point(700, 18),
                Size = new Size(220, 38),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(0, 102, 51),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Click += BtnBack_Click;

            // Add controls
            this.Controls.Add(header);
            this.Controls.Add(searchPanel);
            this.Controls.Add(lblResults);
            this.Controls.Add(lstResults);
            this.Controls.Add(lblAnnouncements);
            this.Controls.Add(lblRecommendations);
            this.Controls.Add(lstRecommendations);
            this.Controls.Add(btnBack);

            // Ensure the back button stays above the header
            btnBack.BringToFront();
        }

        // Return to the main menu
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (announcementTimer != null)
            {
                announcementTimer.Stop();
                announcementTimer.Dispose();
                announcementTimer = null;
            }

            this.Close();
        }

        // Load events and announcements
        private void LocalEventsForm_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.Clear();

            cmbCategory.Items.Add("(All Categories)");

            foreach (string cat in EventManager.GetCategories())
            {
                cmbCategory.Items.Add(cat);
            }

            cmbCategory.SelectedIndex = 0;

            ShowResults(EventManager.GetUpcomingEvents());
            ShowRecommendations();

            PostNextAnnouncement();
        }

        // Search events
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string category =
                cmbCategory.SelectedIndex <= 0
                    ? null
                    : cmbCategory.SelectedItem.ToString();

            DateTime? date = chkFilterDate.Checked
                ? dtpDate.Value.Date
                : (DateTime?)null;

            var results = EventManager.Search(
                txtSearch.Text.Trim(),
                category,
                date
            );

            ShowResults(results);
            ShowRecommendations();
        }

        // Display search results
        private void ShowResults(
            System.Collections.Generic.List<LocalEvent> events)
        {
            lstResults.Items.Clear();

            foreach (LocalEvent ev in events)
            {
                lstResults.Items.Add(ev);
            }

            if (lstResults.Items.Count == 0)
            {
                lstResults.Items.Add("No events match your search.");
            }
        }

        // Display recommendations
        private void ShowRecommendations()
        {
            lstRecommendations.Items.Clear();

            foreach (LocalEvent ev in EventManager.GetRecommendations())
            {
                lstRecommendations.Items.Add(
                    ev.Title + " - " +
                    ev.Category + " (" +
                    ev.Date.ToString("dd MMM yyyy") + ")"
                );
            }

            if (lstRecommendations.Items.Count == 0)
            {
                lstRecommendations.Items.Add(
                    "Search for a few events and recommendations " +
                    "will appear here."
                );
            }
        }

        // Show details when an event is selected
        private void LstResults_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (lstResults.SelectedItem is LocalEvent ev)
            {
                EventManager.PushViewed(ev);

                MessageBox.Show(
                    ev.Description +
                    "\n\nVenue: " + ev.Venue +
                    "\nDate: " +
                    ev.Date.ToString("dddd, dd MMMM yyyy") +
                    "\nCategory: " + ev.Category,
                    ev.Title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        // Display queued announcements
        private void PostNextAnnouncement()
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            if (!EventManager.HasAnnouncements)
            {
                return;
            }

            string text = EventManager.DequeueAnnouncement();

            lblAnnouncements.Text = "Announcements: " + text;

            if (announcementTimer != null)
            {
                announcementTimer.Stop();
                announcementTimer.Dispose();
            }

            announcementTimer = new Timer
            {
                Interval = 6000
            };

            announcementTimer.Tick += (s, e) =>
            {
                announcementTimer.Stop();
                announcementTimer.Dispose();
                announcementTimer = null;

                PostNextAnnouncement();
            };

            announcementTimer.Start();
        }

        // Clean up the announcement timer
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (announcementTimer != null)
            {
                announcementTimer.Stop();
                announcementTimer.Dispose();
                announcementTimer = null;
            }

            base.OnFormClosed(e);
        }
    }
}
