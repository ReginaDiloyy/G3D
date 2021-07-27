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
using System.Diagnostics;

namespace G3Dsys_database
{
    public partial class ItemDetails : Form
    {
        public ItemDetails()
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


        private void ItemDetails_Load(object sender, EventArgs e)
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

        private void label11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label11_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void label11_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }


        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to remove this item?", "Delete", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                FirebaseResponse deleteresponse = client.Delete("ItemList/Items/" + InvoiceTBox.Text);
                MessageBox.Show("Delete Successfully");
            }

            else if (dialog == DialogResult.No)
            {
                return;
            }
            this.Close();

        }



        private void EditBtn_Click(object sender, EventArgs e)
        {
            
            FirebaseResponse editresponse = client.Get("ItemList/Items/" + InvoiceTBox.Text);
            AddItem add = editresponse.ResultAs<AddItem>();
           
            EditItem editform = new EditItem();

            editform.EditNameTxt.Text = add.ItemName;
            editform.EditPartTxt.Text = add.Part;
            editform.EditDescTxt.Text = add.Description;
            editform.EditPriceTxt.Text = add.Price;
            editform.EditStockTxt.Text = add.Stock;
            editform.EditDesignTxt.Text = add.Design;
            editform.EditSpecsTxt.Text = add.Specs;
            editform.EditRemarksTxt.Text = add.Remarks;
           // editform.EditInvoiceTBox.Text = add.Invoice;

            editform.ShowDialog();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
                FirebaseResponse dwgDL = client.Get("Files/" + InvoiceTBox.Text);
                Files dl = dwgDL.ResultAs<Files>();
                Process.Start(dl.dwgFiles);
                
           
        }

       




    }
}
