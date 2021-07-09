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


namespace global3d
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        IFirebaseConfig ifc = new FirebaseConfig()
        {
            AuthSecret = "7WOUhtWRszZ0mXK4idpVzWh9BxAkdxP80riF8eoX",
            BasePath = "https://g3dproj-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        private void LoginForm_Load(object sender, EventArgs e)
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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register reg = new Register();
            reg.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            #region Condition
            if (string.IsNullOrWhiteSpace(txtUsername.Text) && string.IsNullOrWhiteSpace(txtPassword.Text))

            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            #endregion

            FirebaseResponse res = client.Get("Users/" + txtUsername.Text);
            Users ResUser = res.ResultAs<Users>();

            Users CurUser = new Users()
            {
                Username = txtUsername.Text,
                Password = txtPassword.Text
            };

            if(Users.IsEqual(ResUser,CurUser))
            {
                this.Hide();
                Home homepage = new Home();
                homepage.ShowDialog();
            }

            else
            {
                Users.ShowError();
            }
        }
    }
}
