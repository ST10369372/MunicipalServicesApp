using System;

namespace MunicipalServicesApp
{
    public class Issue
    {
        public int IssueID { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string AttachmentPath { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Status { get; set; }
        public int CivicPointsEarned { get; set; }

        public Issue()
        {
            ReportedDate = DateTime.Now;
            Status = "Pending";
            CivicPointsEarned = 10;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Category, Location, Status);
        }
    }
}
