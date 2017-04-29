using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MD5Checker
{
    class CustomBox
    {
        private CustomBox() { }

        public static void Show(){
            Form form = new FormMsgBox();
            form.ShowDialog();
        }
        public static void ShowU(string url, Version v)
        {
            Form f = new FormNV(url, v);
            f.ShowDialog();
        }


    }
}
