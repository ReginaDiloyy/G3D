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
    public partial class Add_Item : Form
    {
        public Add_Item()
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

        private void Add_Item_Load(object sender, EventArgs e)
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

        private void CloseLbl_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void CloseLbl_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void CloseLbl_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void AddSaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddNameTxt.Text) &&
                string.IsNullOrWhiteSpace(AddPartTxt.Text) &&
                string.IsNullOrWhiteSpace(AddDescTxt.Text) &&
                string.IsNullOrWhiteSpace(AddPriceTxt.Text) &&
                string.IsNullOrWhiteSpace(AddStockTxt.Text) &&
                string.IsNullOrWhiteSpace(AddDesignTxt.Text) &&
                string.IsNullOrWhiteSpace(AddSpecsTxt.Text) &&
                string.IsNullOrWhiteSpace(AddRemarksTxt.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }


// AUTO INVOICE *******************************************************
            FirebaseResponse Addresponse = client.Get("ItemList/Invoice/id");
            InvoiceID get = Addresponse.ResultAs<InvoiceID>(); //0
            
////////////////// ****************************************************

            AddItem add = new AddItem()
            {
                Invoice = (Convert.ToInt32(get.cnt)+1).ToString("D5"),
                ItemName = AddNameTxt.Text,
                Part = AddPartTxt.Text,
                Description = AddDescTxt.Text,
                Price = AddPriceTxt.Text,
                Stock = AddStockTxt.Text,
                Design = AddDesignTxt.Text,
                Specs = AddSpecsTxt.Text,
                Remarks = AddRemarksTxt.Text
                
            };

            var setter = client.Set("ItemList/Items/" + add.Invoice, add);
            

            //DOWNLOAD

            var downloadUrl = await SaveFile(AdduploadTxt.Text);

            Files dl = new Files()
            {
                dwgFiles = downloadUrl
            };

            var setter2 = client.Set("Files/" + add.Invoice, dl);
                       
            // INVOICE ID
            var obj = new InvoiceID
            {
                cnt = add.Invoice
            };
            var setter1 = client.Set("ItemList/Invoice/id", obj);



            //////////////////////////////////////// upload FILE      ////////////////////////////////////////////////////////////
            //SaveFile(AdduploadTxt.Text);
            AddItem add1 = new AddItem();
            Files files1 = new Files();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            MessageBox.Show("     Item inserted succsessfully!" +Environment.NewLine + "                " +
                "Invoice: " + add.Invoice);
            ClearText();    
        }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<String> SaveFile(string filepath)
        {
            //AddItem add1 = new AddItem();
            
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

            return downloadUrl;
            
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void ClearText()
        {
            AddNameTxt.Clear();
            AddPartTxt.Clear();
            AddDescTxt.Clear();
            AddPriceTxt.Clear();
            AddStockTxt.Clear();
            AddDesignTxt.Clear();
            AddSpecsTxt.Clear();
            AddRemarksTxt.Clear();
        }

// ************************ NUMBERS ONLY ACCEPTED *************************************
        private void AddPriceTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch!=8 ) //8=backspace key, 
            {
                e.Handled = true;
                MessageBox.Show("Invalid Input!");
            }
            
        }
        private void AddStockTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //8=backspace key, 
            {
                e.Handled = true;
                MessageBox.Show("Invalid Input!");
            }
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
                        
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            AdduploadTxt.Text = dlg.FileName;
                                   
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            //SaveFile(AdduploadTxt.Text);
            
        }
    }
}
