using System;
using System.Windows.Forms;

namespace MunicipalServicesApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // No database to connect to anymore - issues are stored in memory
            // for the lifetime of the running application (see IssueManager).
            Application.Run(new MainMenuForm());
        }
    }
}
