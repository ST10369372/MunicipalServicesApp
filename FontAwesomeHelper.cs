using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    public static class FontAwesomeHelper
    {
        private static PrivateFontCollection fontCollection;
        public static FontFamily FontFamily { get; private set; }

        public const string IconBuilding = "\uf1ad";
        public const string IconExclamation = "\uf06a";
        public const string IconCalendar = "\uf133";
        public const string IconSearch = "\uf002";
        public const string IconMapMarker = "\uf3c5";
        public const string IconTag = "\uf02b";
        public const string IconPaperclip = "\uf0c6";
        public const string IconCheck = "\uf00c";
        public const string IconArrowLeft = "\uf060";
        public const string IconTrophy = "\uf091";
        public const string IconStar = "\uf005";
        public const string IconChartBar = "\uf080";
        public const string IconPen = "\uf304";
        public const string IconComment = "\uf075";
        public const string IconUpload = "\uf093";
        public const string IconMedal = "\uf5a2";
        public const string IconCircleCheck = "\uf058";

        public static void LoadFont()
        {
            if (fontCollection == null)
            {
                fontCollection = new PrivateFontCollection();
                string fontPath = Path.Combine(Application.StartupPath, "fa-solid-900.ttf");
                if (File.Exists(fontPath))
                {
                    fontCollection.AddFontFile(fontPath);
                    FontFamily = fontCollection.Families[0];
                }
            }
        }

        public static Font GetFont(float size)
        {
            if (FontFamily == null) LoadFont();
            return FontFamily != null
                ? new Font(FontFamily, size)
                : new Font("Segoe UI", size);
        }
    }
}
