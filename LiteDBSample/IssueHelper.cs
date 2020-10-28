using LiteDB;
using System;
using System.Collections.Generic;

namespace LiteDBSample
{
    public class IssueRepository
    {
        private string liteDBPath = "";

        public IssueRepository(string databasePath)
        {
            this.liteDBPath = databasePath;
        }

        /// <summary>
        /// Returns all Issue Types
        /// </summary>
        /// <returns></returns>
        public List<string> GetIssueTypes()
        {
            var issueTypes = new List<string>();
            issueTypes.Add("Warning");
            issueTypes.Add("Error");
            issueTypes.Add("Info");
            return issueTypes;
        }
                
        /// <summary>
        /// Returns a filtered list of all issues for the matching Issue Type and DateTime
        /// </summary>
        /// <param name="issueType">Issue Type</param>
        /// <param name="datetime">DateTime</param>
        /// <returns></returns>
        public IList<Issue> Get(string issueType, DateTime datetime)
        {
            var issuesToReturn = new List<Issue>();
            using (var db = new LiteEngine(liteDBPath))
            {
                var issues = db.GetCollection<Issue>("issues");
                IEnumerable<Issue> filteredIssues;

                if (issueType.Equals("All"))
                    filteredIssues = issues.All();
                else
                    filteredIssues = issues.Find(i => i.IssueType.Equals(issueType));

                foreach (Issue issueItem in filteredIssues)
                {
                    issuesToReturn.Add(issueItem);
                }
                return issuesToReturn.FindAll(i => i.DateTime.Date == datetime.Date);
            }
        }

        /// <summary>
        /// Returns a Collection of Issue Items
        /// </summary>
        /// <returns></returns>
        public IList<Issue> GetAll()
        {
            var issuesToReturn = new List<Issue>();
            using (var db = new LiteEngine(liteDBPath))
            {
                var issues = db.GetCollection<Issue>("issues");
                var results = issues.All();
                foreach (Issue issueItem in results)
                {
                    issuesToReturn.Add(issueItem);
                }
                return issuesToReturn;
            }
        }

        /// <summary>
        /// Save an Issue Item
        /// </summary>
        /// <param name="issue">Issue Item</param>
        public void Add(Issue issue)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteEngine(liteDBPath))
            {
                var issueCollection = db.GetCollection<Issue>("issues");
                // Insert a new issue document
                issueCollection.Insert(issue);
                IndexIssue(issueCollection);
            }
        }

        /// <summary>
        /// Update an Existing Issue Item
        /// </summary>
        /// <param name="issue">Issue Item</param>
        public void Update(Issue issue)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteEngine(liteDBPath))
            {
                var issueCollection = db.GetCollection<Issue>("issues");
                // Update an existing issue document
                issueCollection.Update(issue);                
            }
        }
        
        /// <summary>
        /// Delete an Issue Item by Issue ID (GUID)
        /// </summary>
        /// <param name="issueId">Issue Id(Guid)</param>
        public void Delete(Guid issueId)
        {
            using (var db = new LiteEngine(liteDBPath))
            {
                var issues = db.GetCollection<Issue>("issues");
                issues.Delete(i => i.IssueId == issueId);               
            }
        }
        
        /// <summary>
        /// Index Issue
        /// </summary>
        /// <param name="issueCollection">Issue Collection</param>
        private void IndexIssue(Collection<Issue> issueCollection)
        {
            // Index on IssueId
            issueCollection.EnsureIndex(x => x.IssueId);

            // Index on ErrorText
            issueCollection.EnsureIndex(x => x.ErrorText);

            // Index on DateTime
            issueCollection.EnsureIndex(x => x.DateTime);

            // Index on IssueType
            issueCollection.EnsureIndex(x => x.IssueType);
        }
    }
}
