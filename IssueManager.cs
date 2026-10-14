using System.Collections.Generic;

namespace MunicipalServicesApp
{
    // Single source of truth for reported issues, held purely in memory.
    // As required by the brief ("utilise appropriate data structures ...
    // e.g., list for storing issues"), a List<Issue> is used to store and
    // organise every report for the lifetime of the running application.
    // Note: this data is not persisted anywhere - it resets each time the
    // app is closed and restarted.
    public static class IssueManager
    {
        public static List<Issue> Issues { get; } = new List<Issue>();

        // Simple in-memory ID generator, since there's no database to
        // hand out identities for us anymore.
        private static int nextIssueId = 1;

        public static int AddIssue(Issue issue)
        {
            issue.IssueID = nextIssueId++;
            Issues.Add(issue);
            return issue.IssueID;
        }

        public static int GetIssueCount()
        {
            return Issues.Count;
        }

        public static int GetTotalCivicPoints()
        {
            int total = 0;
            foreach (Issue issue in Issues)
            {
                total += issue.CivicPointsEarned;
            }
            return total;
        }
    }
}
