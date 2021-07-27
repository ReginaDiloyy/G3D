using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace G3Dsys_database
{
    public partial class mainFRM : Form
    {
        public mainFRM()
        {
            InitializeComponent();
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            //BORIS FIREBASE
            //AuthSecret = "9mDgcPnCeS1P6sRdg09lvm7dzRWN7Z3Uedgl3E9E",
            //BasePath = "https://g3dsys-database-default-rtdb.asia-southeast1.firebasedatabase.app/"

            //REG FIREBASE
            AuthSecret = "7WOUhtWRszZ0mXK4idpVzWh9BxAkdxP80riF8eoX",
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        private void mainFRM_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("No Internet Connection Problem");
            }

            DGrid();

           
        }

        DataTable dt = new DataTable();


        /// LOGOUT ******************************************************************************************************//
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void mainFRM_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to logout?", "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.ExitThread();
            }

            else if (dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        //***************************************************************************************************************//

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
           this.Cursor = Cursors.Hand;
        }
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Add_Item additional = new Add_Item();
            additional.ShowDialog();
        }
        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            /* OLD REFRESH **********************
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            DGrid();
            ********************************** */ 

            // REFRESH
            DataView dv = new DataView(dt);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = dv;

        }

        public void DGrid()
        {

            try
            {

                dt.Columns.Add("Part Number");
                dt.Columns.Add("Specs Status");
                dt.Columns.Add("Design Status");
                dt.Columns.Add("Description");
                dt.Columns.Add("Name of Item");
                dt.Columns.Add("Price");
                dt.Columns.Add("Stock");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("Invoice");

               
                
                dataGridView1.Rows.Clear();
                FirebaseResponse response = client.Get("ItemList/Items");
                Dictionary<string, AddItem> getAddItem = response.ResultAs<Dictionary<string, AddItem>>();

                foreach (var get in getAddItem)
                {
                    dt.Rows.Add(

                        get.Value.Part,
                        get.Value.Specs,
                        get.Value.Design,
                        get.Value.Description,
                        get.Value.ItemName,
                        get.Value.Price,
                        get.Value.Stock,
                        get.Value.Remarks,
                        get.Value.Invoice

                       
                        );
                }

                DataView dv = new DataView(dt);
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dv;
                dataGridView1.Columns[0].Width = 300; //Part Number
                dataGridView1.Columns[1].Width = 200; //Specs Status
                dataGridView1.Columns[2].Width = 100; //Design Status
                dataGridView1.Columns[3].Width = 548; //Description
                dataGridView1.Columns[4].Width = 200; //Name of Item
                dataGridView1.Columns[5].Width = 200; //Price
                dataGridView1.Columns[6].Width = 200; //Stock
                dataGridView1.Columns[7].Width = 200; //Remarks
                dataGridView1.Columns[8].Width = 200; //Invoice
                               

            }

            catch
            {
                MessageBox.Show("No Data");
            }

        }

        
        private void SearchTxt_Click(object sender, EventArgs e)
        {
            SearchTxt.Clear();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            ItemDetails details = new ItemDetails();
            details.Namelbl.Text = this.dataGridView1.CurrentRow.Cells[4].Value.ToString(); // Name
            details.Partlbl.Text = this.dataGridView1.CurrentRow.Cells[0].Value.ToString(); //Part no
            details.Pricelbl.Text = this.dataGridView1.CurrentRow.Cells[5].Value.ToString(); //price
            details.Stocklbl.Text = this.dataGridView1.CurrentRow.Cells[6].Value.ToString(); //stock
            details.Specslbl.Text = this.dataGridView1.CurrentRow.Cells[1].Value.ToString(); //specs
            details.Designlbl.Text = this.dataGridView1.CurrentRow.Cells[2].Value.ToString(); //design
            details.DescriptionTBox.Text = this.dataGridView1.CurrentRow.Cells[3].Value.ToString(); //description
            details.RemarksTBox.Text = this.dataGridView1.CurrentRow.Cells[7].Value.ToString(); //remarks
            details.InvoiceTBox.Text = this.dataGridView1.CurrentRow.Cells[8].Value.ToString(); //invoice
                        
            details.ShowDialog();

        }

        

        private void SearchTxt_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void SearchTxt_TextChanged(object sender, EventArgs e)
        {
            string searchValue = SearchTxt.Text;
            try
            {
                var re = from row in dt.AsEnumerable()
                         where row[0].ToString().Contains(searchValue) //Search by Part Number
                         select row;
                if (re.Count() == 0)
                {
                    MessageBox.Show("No match found");
                }
                else
                {
                    dataGridView1.DataSource = re.CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
