using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.ComponentModel;
using LiteDB;

namespace LiteDbMySample1
{
    public partial class Form1 : Form
    {
        string LiteDBPath = ConfigurationManager.AppSettings["Dbpath"].ToString();
        Guid SelectHost = Guid.Empty;


        public Form1()
        {
            InitializeComponent();
            
            var libDbCon = new MyLiteDb(LiteDBPath);

            cmbPingStatus.DataSource = libDbCon.GetTypesPing();
            cmbPingStatus.SelectedIndex = 17;

            var StatusPing = libDbCon.GetTypesPing();   //0-23
            StatusPing.Add("All");                      //24
            StatusPing.Add("Failed All");               //25

            cmbFiltrStatus.DataSource = StatusPing;
            cmbFiltrStatus.SelectedIndex = 24;          //all

            RefreshGridView(libDbCon.GetAll());

            dtGridView.RowEnter += dtGridView_RowEnter;
            dtGridView.Columns[0].Visible = false;
            dtGridView.AllowUserToAddRows = false;
            dtGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtGridView.MultiSelect = false;

        }

        private void dtGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void RefreshGridView(IList<StructDB> hostResult)
        {
            var bindList = new BindingList<StructDB>(hostResult);
            var source = new BindingSource(bindList, null);

            dtGridView.DataSource = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var PingAdd = new MyLiteDb(LiteDBPath);

            var Host = new StructDB
            {
                dtHostPing = DateTime.Now,
                MessageText = "IP",
                TypePing = cmbPingStatus.SelectedValue.ToString()
            };

            if (SelectHost != Guid.Empty)
            {
                Host.ID = SelectHost;

            }
            else
            {
                Host.ID = Guid.NewGuid();
                PingAdd.Add(Host);

            }

            RefreshGridView(PingAdd.GetAll());
            CLear();

        }

        private void CLear()
        {
            cmbPingStatus.SelectedIndex = 0;
            SelectHost = Guid.Empty;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var FilterList = new MyLiteDb(LiteDBPath);

            var filterTypeStatus = FilterList.Get(cmbFiltrStatus.SelectedValue.ToString(), dateTimePicker1.Value);

            RefreshGridView(filterTypeStatus);

        }
    }
}
