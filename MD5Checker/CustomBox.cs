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



    }
}
