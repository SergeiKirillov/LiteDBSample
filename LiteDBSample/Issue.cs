using LiteDB;
using System;

namespace LiteDBSample
{
    public class Issue
    {
        [BsonId]
        public Guid IssueId { get; set; }

        public DateTime DateTime { get; set; }

        public string ErrorText { get; set; }

        public string IssueType { get; set; }
    }
}
