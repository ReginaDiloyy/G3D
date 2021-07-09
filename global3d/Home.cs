using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Newtonsoft.Json;

namespace global3d
{
    public partial class Home : Form
    {
        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "7WOUhtWRszZ0mXK4idpVzWh9BxAkdxP80riF8eoX",
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public Home()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm logout = new LoginForm();
            logout.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct add = new AddProduct();
            add.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            FirebaseResponse resRef = client.Get("Information/");
            Dictionary<string, Information> data = JsonConvert.DeserializeObject<Dictionary<string, Information>>(resRef.Body.ToString());
            PopulateDataGrid(data);
        }

        void PopulateDataGrid(Dictionary<string, Information> record)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();


            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("PartNumber", "Part Number");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("UnitPrice", "Unit Price");
            dataGridView1.Columns.Add("Invoice", "Invoice");
            dataGridView1.Columns.Add("UnitStock", "Unit Stock");
            dataGridView1.Columns.Add("DesignStatus", "Design Status");
            dataGridView1.Columns.Add("SpecsStatus", "Specs Status");
            dataGridView1.Columns.Add("Remarks", "Remarks");


            foreach (var item in record)
            {
                dataGridView1.Rows.Add(item.Key, item.Value.Name, item.Value.PartNumber,
                    item.Value.Description, item.Value.UnitPrice, item.Value.UnitStock,
                    item.Value.DesignStatus, item.Value.SpecsStatus, item.Value.Remarks);
            }
        }

        private void Home_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("Connection fo Firebase Failed");
            }
        }

       private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            View viewProduct = new View();

            viewProduct.txtName.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            viewProduct.txtPartNumber.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            viewProduct.txtDescription.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            viewProduct.txtUnitPrice.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            viewProduct.txtInvoice.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            viewProduct.txtUnitStock.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            viewProduct.txtDesignStatus.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            viewProduct.txtSpecsStatus.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            viewProduct.txtRemarks.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();

            viewProduct.ShowDialog();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            /*
            AddProduct viewProduct = new AddProduct();
            viewProduct.txtName.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            viewProduct.txtPartNumber.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            
            viewProduct.ShowDialog(); */
        }
    }
}
