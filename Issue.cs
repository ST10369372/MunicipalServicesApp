using System;
using System.Collections.Generic;

namespace MunicipalServicesApp
{
    public sealed class Issue
    {
        public int Id { get; internal set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string MediaFilePath { get; set; }
        public DateTime ReportedDate { get; set; }
        public string Status { get; set; }

        public Issue(string location, string category, string description, string mediaFilePath = "")
        {
            Location = location?.Trim() ?? string.Empty;
            Category = category?.Trim() ?? string.Empty;
            Description = description?.Trim() ?? string.Empty;
            MediaFilePath = mediaFilePath ?? string.Empty;
            ReportedDate = DateTime.Now;
            Status = "Pending";
        }

        public string ReferenceCode => $"MSA-{ReportedDate:yyyyMMdd}-{Id:D4}";

        public override string ToString() =>
            $"[{ReferenceCode}] {Category} @ {Location} ({Status})";
    }

    public static class IssueDataStore
    {
        private static readonly List<Issue> _issues = new List<Issue>();
        private static int _nextId = 1;

        public static IReadOnlyList<Issue> Issues => _issues;
        public static int Count => _issues.Count;

        public static void Add(Issue issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            issue.Id = _nextId++;
            _issues.Add(issue);
        }

        public static void Clear()
        {
            _issues.Clear();
            _nextId = 1;
        }
    }
}
