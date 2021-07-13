using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace G3Ddatabase
{
    public partial class Form1 : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "GtNL9LezL43hkmHjMn2WcHrkc0RR3AduJDVVMg6Y",
            BasePath = "https://g3ddatabase-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if(client!=null)
            {
                MessageBox.Show("Connection is Established!");
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            FirebaseResponse resp = await client.GetTaskAsync("Counter/node");
            Counter_class get = resp.ResultAs<Counter_class>();
            MessageBox.Show(get.cnt);

            
            var data = new Data
            {
                Name = txtName.Text,
                PartNumber = (Convert.ToInt32(get.cnt)+1).ToString(),
                Description = txtDescription.Text,
                UnitPrice = txtUnitPrice.Text,
                Invoice = txtInvoice.Text,
                UnitStock = txtUnitStock.Text,
                DesignStatus = txtDesignStatus.Text,
                SpecsStatus = txtSpecsStatus.Text,
                Remarks = txtRemarks.Text

            };

            SetResponse response = await client.SetTaskAsync("Information/"+txtName.Text,data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted " + result.Name);

            var obj = new Counter_class
            {
                cnt = data.PartNumber
            };

            SetResponse response1 = await client.SetTaskAsync("Counter/node",obj);
        } 

        private async void btnRetrieve_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Information/" + txtName.Text);
            Data obj = response.ResultAs<Data>();

            txtName.Text = obj.Name;
            txtPartNumber.Text = obj.PartNumber;
            txtDescription.Text = obj.Description;
            txtUnitPrice.Text = obj.UnitPrice;
            txtInvoice.Text = obj.Invoice;
            txtUnitStock.Text = obj.UnitStock;
            txtDesignStatus.Text = obj.UnitStock;
            txtSpecsStatus.Text = obj.SpecsStatus;
            txtRemarks.Text = obj.Remarks;

            MessageBox.Show("Data Retrieved.");
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Name = txtName.Text,
                PartNumber = txtPartNumber.Text,
                Description = txtDescription.Text,
                UnitPrice = txtUnitPrice.Text,
                Invoice = txtInvoice.Text,
                UnitStock = txtUnitStock.Text,
                DesignStatus = txtDesignStatus.Text,
                SpecsStatus = txtSpecsStatus.Text,
                Remarks = txtRemarks.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Information/" + txtName.Text,data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Data updated at Name: " + result.Name); 
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Information/" + txtName.Text);
            MessageBox.Show("Deleted Record of Name: " + txtName.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            DataGrid dg = new DataGrid();
            dg.Show();
        }
    }
}
