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
using Firebase.Storage;
using System.IO;



namespace global3d
{ 
    public partial class AddProduct : Form
    {
        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "7WOUhtWRszZ0mXK4idpVzWh9BxAkdxP80riF8eoX",
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        
        public AddProduct()
        {
            InitializeComponent();
        }

        private void AddProduct_Load(object sender, EventArgs e)
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var data = new Information
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

            SetResponse response = await client.SetAsync("Information/" + txtName.Text, data);
            Information result = response.ResultAs<Information>();

            MessageBox.Show(result.Name + " Added Successfully.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home cancel = new Home();
            cancel.ShowDialog();
        }

        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                //openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                   filePath = openFileDialog.FileName;

                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    txtDWGfiles.Text = openFileDialog.FileName;
                }
            }

           // MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Upload(txtDWGfiles.Text);
        }

        public void Upload(string filepath)
        {
            AddProduct add = new AddProduct();
            using (Stream stream = File.OpenRead(filepath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                string extn = new FileInfo(filepath).Extension;
                var upload = client.Set("Files/" + txtDWGfiles.Text, add);
            }
        }
    }
}
