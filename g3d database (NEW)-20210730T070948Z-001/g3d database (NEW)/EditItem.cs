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
using System.IO;
using Firebase.Storage;



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
            //BORIS FIREBASE
            //AuthSecret = "9mDgcPnCeS1P6sRdg09lvm7dzRWN7Z3Uedgl3E9E",
            //BasePath = "https://g3dsys-database-default-rtdb.asia-southeast1.firebasedatabase.app/"

            //REG FIREBASE
            AuthSecret = "7WOUhtWRszZ0mXK4idpVzWh9BxAkdxP80riF8eoX",
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/"
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
                Invoice = EditInvoiceTBox.Text //dont change
            };
           // SaveFile(AdduploadTxt.Text);

            FirebaseResponse updateResponse = client.Update("ItemList/Items/" + EditInvoiceTBox.Text, items);

            MessageBox.Show("Update Successfully!");

            this.Hide();
            mainFRM main = new mainFRM();
            main.ShowDialog();
            this.Close();
        }

        void ClearText()
        {
            EditNameTxt.Clear();
            EditPartTxt.Clear();
            EditDescTxt.Clear();
            EditPriceTxt.Clear();
            EditStockTxt.Clear();
            EditDesignTxt.Clear();
            EditSpecsTxt.Clear();
            EditRemarksTxt.Clear();
        }

        public async void SaveFile(string filepath)
        {
            AddItem add1 = new AddItem();

            // *************** NEW UPLOAD *****
            var stream = File.OpenRead(filepath);

            var task = new FirebaseStorage("g3dproj.appspot.com")
             .Child("DWG Files")
             .Child(filepath)
             .PutAsync(stream);

            task.Progress.ProgressChanged += (s, a) => Console.WriteLine($"Progress: {a.Percentage} %");

            var downloadUrl = await task;
            Console.WriteLine(downloadUrl);

            MessageBox.Show(downloadUrl);
            ClearText();

            this.Hide();
            mainFRM home = new mainFRM();
            home.ShowDialog();
            this.Close();

        }

        private void CloseLbl_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainFRM main = new mainFRM();
            main.ShowDialog();
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
