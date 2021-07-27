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
    public partial class Registration : Form
    {
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

        private void Registration_Load(object sender, EventArgs e)
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
        public Registration()
        {
            InitializeComponent();
        }

        private void regBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(regUserTxt.Text) &&
                string.IsNullOrWhiteSpace(regPassTxt.Text) &&
                string.IsNullOrWhiteSpace(regNameTxt.Text) &&
                string.IsNullOrWhiteSpace(regIDTxt.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            MyUser user = new MyUser()
            {
                Username = regUserTxt.Text,
                Password = regPassTxt.Text,
                Name = regNameTxt.Text,
                numberID = regIDTxt.Text
            };

            SetResponse set = client.Set(@"Users/" +regUserTxt.Text, user);

            MessageBox.Show("Successfully register!");
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
