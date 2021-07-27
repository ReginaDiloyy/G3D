using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace G3Dsys_database
{
    public partial class loginFRM : Form
    {
       
         
        public loginFRM()
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
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/",
            
        };

        IFirebaseClient client;

        private void loginFRM_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(ifc);
            }

            catch
            {
                MessageBox.Show("No Internet Connection Problem");
            }
            this.AcceptButton = loginBTN;
        }

        private void loginBTN_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(usernameTXT.Text) &&
                string.IsNullOrWhiteSpace(passwordTXT.Text))
             {
                MessageBox.Show("Please fill all the fields");
                return;
             }

            FirebaseResponse res = client.Get(@"Users/" + usernameTXT.Text);
            MyUser LogUser = res.ResultAs<MyUser>(); //database result
            
            MyUser CurUser = new MyUser()       //current user, user given info
            {
                Username = usernameTXT.Text,
                Password = passwordTXT.Text
            };

            if(MyUser.IsEqual(LogUser,CurUser))
            {
                this.Hide();
                mainFRM main = new mainFRM();
                main.ShowDialog();
                this.Close();
            }
            else
            {
                MyUser.ShowError();
            }

            
        }

        private void RegisterBTN_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.ShowDialog();
        }
    }
}
