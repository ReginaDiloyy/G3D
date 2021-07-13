using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace G3D
{

    public partial class AddProduct : Form
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login ss = new Login();
            ss.Show();
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {

        }
    }
}
