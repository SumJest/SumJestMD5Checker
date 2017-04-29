using System;
using System.Windows.Forms;

namespace MD5Checker
{
    public partial class FormNV : Form
    {
        string download_page;
        public FormNV(string url, Version v)
        {
            InitializeComponent();
            download_page = url;
            label3.Text = v.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(download_page);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
