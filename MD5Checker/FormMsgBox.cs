using System;
using System.Windows.Forms;

namespace MD5Checker
{
    public partial class FormMsgBox : Form
    {
        public FormMsgBox()
        {
            InitializeComponent();
            label3.Text = "Version: " + Application.ProductVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/sumjest");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormMsgBox_Load(object sender, EventArgs e)
        {

        }
    }
}
