using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;

namespace LiteDBSample
{

    public partial class FrmMain : Form
    {
        string liteDBPath = ConfigurationManager.AppSettings["DbPath"].ToString();
        Guid selectedIssueItem = Guid.Empty;

        public FrmMain()
        {
            InitializeComponent();
            this.MaximizeBox = false;

            var issueHelper = new IssueRepository(liteDBPath);
            this.StartPosition = FormStartPosition.CenterScreen;
            cmbIssueType.DataSource = issueHelper.GetIssueTypes();
            
            var issueTypes = issueHelper.GetIssueTypes();
            issueTypes.Add("All");
            cmbFilterIssuesBy.DataSource = issueTypes;
            cmbFilterIssuesBy.SelectedIndex = 3;

            // Get all issues          
            RefreshGridView(issueHelper.GetAll());

            dataGridView1.RowEnter += dataGridView1_RowEnter;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // Disable Delete Button
            btnDelete.Enabled = false;
        }
        
        private void RefreshGridView(IList<Issue> issues)
        {
            var bindingList = new BindingList<Issue>(issues);        
            var source = new BindingSource(bindingList, null);
            dataGridView1.DataSource = source;
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }        

        private void btnSave_Click(object sender, EventArgs e)
        {
            var issueHelper = new IssueRepository(liteDBPath);
            var issue = new Issue
            {
                DateTime = DateTime.Now,
                ErrorText = txtErrorMessage.Text.Trim(),
                IssueType = cmbIssueType.SelectedValue.ToString()
            };

            // Add or Update an Issue Item
            if (selectedIssueItem != Guid.Empty)
            {
                issue.IssueId = selectedIssueItem;
                issueHelper.Update(issue);
            }
            else
            {
                issue.IssueId = Guid.NewGuid();
                issueHelper.Add(issue);
            }
            RefreshGridView(issueHelper.GetAll());
            Clear();
        }

        private void Clear()
        {
            txtErrorMessage.Text = "";
            cmbIssueType.SelectedIndex = 0;
            selectedIssueItem = Guid.Empty;
            btnDelete.Enabled = false;
        }
        
        private void btnFilterIssues_Click(object sender, EventArgs e)
        {
            var issueHelper = new IssueRepository(liteDBPath);
            var filteredIssues = issueHelper.Get(cmbFilterIssuesBy.SelectedValue.ToString(),
                                                 dateTimePicker1.Value);
            RefreshGridView(filteredIssues);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (((DataGridView)sender).SelectedRows.Count > 0)
            {
                var row = ((DataGridView)sender).SelectedRows[0];
                selectedIssueItem = Guid.Parse(row.Cells["IssueId"].Value.ToString());
                txtErrorMessage.Text = row.Cells["ErrorText"].Value.ToString();
                cmbIssueType.SelectedItem = row.Cells["IssueType"].Value.ToString();
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var issueHelper = new IssueRepository(liteDBPath);
            issueHelper.Delete(selectedIssueItem);
            RefreshGridView(issueHelper.GetAll());
            Clear();
        }
    }
}
