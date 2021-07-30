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
using Firebase;



namespace G3Dsys_database
{
    public partial class Upload : Form
    {
        public Upload()
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

        private async void Upload_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("No Internet Connection Problem");
            }

            // Get any Stream — it can be FileStream, MemoryStream or any other type of Stream
            var stream = File.Open(@"C:\file.dwg", FileMode.Open);

            // Construct FirebaseStorage with path to where you want to upload the file and put it there
            var task = new FirebaseStorage("g3dproj.appspot.com")
             .Child("DWG Files")
             .Child("file.dwg")
             .PutAsync(stream);

            // Track progress of the upload
            task.Progress.ProgressChanged += (s, a) => Console.WriteLine($"Progress: {a.Percentage} %");

            // Await the task to wait until upload is completed and get the download url
            //var downloadUrl = await task;

            var downloadUrl = await task;
            Console.WriteLine(downloadUrl);

            MessageBox.Show(downloadUrl);
            
        }

        
    }
}
