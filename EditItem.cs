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
using FireSharp.Response;
using FireSharp.Interfaces;


namespace G3Dsys_database
{
    public partial class EditItem : Form
    {
        public EditItem()
        {
            InitializeComponent();
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "9mDgcPnCeS1P6sRdg09lvm7dzRWN7Z3Uedgl3E9E",
            BasePath = "https://g3dsys-database-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };

        IFirebaseClient client;



        private void EditItem_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("No Internet Connection Problem");
            }

        }

        private void AddSaveBtn_Click(object sender, EventArgs e)
        {

            var items = new AddItem
            {
                ItemName = EditNameTxt.Text,
                Part = EditPartTxt.Text,
                Description = EditDescTxt.Text,
                Price = EditPriceTxt.Text,
                Stock = EditStockTxt.Text,
                Design = EditDesignTxt.Text,
                Specs = EditSpecsTxt.Text,
                Remarks = EditRemarksTxt.Text,
                Invoice = EditInvoiceTBox.Text
            };
    
            FirebaseResponse updateResponse = client.Update("ItemList/Items/" + EditInvoiceTBox.Text, items);

            MessageBox.Show("Update Successfully!");

            this.Close();
        }

        private void CloseLbl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseLbl_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void CloseLbl_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg2 = new OpenFileDialog();
            dlg2.ShowDialog();
            AdduploadTxt.Text = dlg2.FileName;
        }
    }
}
