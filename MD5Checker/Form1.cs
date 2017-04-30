using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace MD5Checker
{
    public partial class Form1 : Form // Many crutches, sorry
    {
        private string error = "Error";
        private string please_choose_file = "Please choose file";
        private string entry_verification_sum = "Please entry verification hashsum";
        private string please_wait = "Please wait, checher is busy!";
        private string successful = "Hashsum of the selected file matches the verification hashsum;Successful Comparison";
        private string not_successful = "Verification hashsum does not match the selected file hashsum!;Not Successful Comparison";
        //private string default_language = "english";
        private string please_drop = "Please drop only one file!";
        private string file_empty = "File is empty!";
        private string please_drop_file = "Please drop file, not directory!";
        private string file_does_not_exists = "File does not exists!";
        private string please_choose_algorithm = "Please choose algoritm!";
        private Algorithm alg;

        public Form1()
        {
            InitializeComponent();
        }

        private string getMD5hash(string path)
        {
            byte[] buffer;
            int bytesRead;
            long size;
            long totalBytesRead = 0;
            
            try
            {
                    if (alg == Algorithm.MD5)
                    {
                        using (Stream file = File.OpenRead(path))
                        {
                        size = file.Length;
                            using (HashAlgorithm hasher = MD5.Create())
                            {
                                do
                                {
                                    buffer = new byte[4096];

                                    bytesRead = file.Read(buffer, 0, buffer.Length);

                                    totalBytesRead += bytesRead;

                                    hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                                    MessageBox.Show(((int)((double)totalBytesRead / size * 100)).ToString());
                                    backgroundWorker1.ReportProgress((int)((double)totalBytesRead / size * 100));


                                }
                                while (bytesRead != 0);
                                hasher.TransformFinalBlock(buffer, 0, 0);
                                file.Close();
                                return MakeHashString(hasher.Hash);

                            }
                        }

                       
                    }
                    else if (alg == Algorithm.SHA256)
                    {
                        using (Stream file = File.OpenRead(path))
                        {
                            size = file.Length;
                            using (HashAlgorithm hasher = SHA256.Create())
                            {
                                do
                                {
                                    buffer = new byte[4096];

                                    bytesRead = file.Read(buffer, 0, buffer.Length);

                                    totalBytesRead += bytesRead;

                                    hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                                    MessageBox.Show(((int)((double)totalBytesRead / size * 100)).ToString());
                                    backgroundWorker1.ReportProgress((int)((double)totalBytesRead / size * 100));


                                }
                                while (bytesRead != 0);
                                hasher.TransformFinalBlock(buffer, 0, 0);
                                file.Close();
                                return MakeHashString(hasher.Hash);

                            }
                        }

                }
                else
                    {
                        return null;
                    }
                
            }catch(Exception ex){
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        private int compareMD5Hash(string hash, string hashcheack)
        {
       
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hash, hashcheack))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = false;

            DialogResult result = ofd.ShowDialog();
            
            if(result == DialogResult.OK){

                textBox1.Text = ofd.FileName;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show(please_choose_file, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!File.Exists(textBox1.Text)){
                MessageBox.Show(file_does_not_exists, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(backgroundWorker1.IsBusy){
                MessageBox.Show(please_wait, error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            long size = File.OpenRead(textBox1.Text).Length;
            if (size <= 0)
            {
                MessageBox.Show(file_empty, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!radioButton1.Checked && !radioButton2.Checked) { MessageBox.Show(please_choose_algorithm, error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            if (radioButton1.Checked) alg = Algorithm.MD5; else if (radioButton2.Checked) alg = Algorithm.SHA256;
            label5.Text = size.ToString() + " bytes";
            DateTime creationtime = File.GetCreationTime(textBox1.Text);
            DateTime lastchanged = File.GetLastWriteTime(textBox1.Text);
            label8.Text = creationtime.ToString();
            label9.Text = lastchanged.ToString();
            
            backgroundWorker1.RunWorkerAsync("compute|" + textBox1.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show(please_wait, error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show(file_does_not_exists, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show(entry_verification_sum, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            long size = File.OpenRead(textBox1.Text).Length;
            if(size <= 0){
                MessageBox.Show(file_empty, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!radioButton1.Checked && !radioButton2.Checked) { MessageBox.Show(please_choose_algorithm, error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            if (radioButton1.Checked) alg = Algorithm.MD5; else if (radioButton2.Checked) alg = Algorithm.SHA256;
            label5.Text = size.ToString() + " bytes";
            DateTime creationtime = File.GetCreationTime(textBox1.Text);
            DateTime lastchanged = File.GetLastWriteTime(textBox1.Text);
            label8.Text = creationtime.ToString();
            label9.Text = lastchanged.ToString();
            
            backgroundWorker1.RunWorkerAsync("compare|" + textBox1.Text);
        }
        private void russianToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            label1.Text = "Файл:";
            label2.Text = "Хеш-сумма";
            label3.Text = "Контрольная хеш-сумма";
            button1.Text = "Выбрать";
            button2.Text = "Рассчитать";
            button3.Text = "Сравнить";
            error = "Ошибка";
            please_choose_file = "Пожалуйста выберете файл";
            entry_verification_sum = "Пожалуйста введите контрольную хеш-сумму";
            successful = "Хеш-сумма выбранного файла совпадает с контрольной хеш-суммой;Сравнение прошло успешно";
            not_successful = "Хеш-сумма выбранного файла не совпадает с контрольной хеш-суммой;Сравнение прошло не успешно";
            fileToolStripMenuItem.Text = "Файл";
            file_does_not_exists = "Файла не существует!";
            languageToolStripMenuItem.Text = "Язык";
            englishToolStripMenuItem1.Text = "Английский";
            russianToolStripMenuItem1.Text = "Русский";
            infoToolStripMenuItem.Text = "Информация";
            please_wait = "Пожалуйста подождите, программа занята другой задачей!";
            please_drop = "Пожалуйста перетаскивайте только один файл!";
            please_choose_algorithm = "Пожалуйста выберете алгоритм!";
            please_drop_file = "Пожалуйста перетащите файл, а не папку!";
            file_empty = "Файл пустой!";
            label4.Text = "Дата создания:";
            label7.Text = "Дата последнего изменения:";
            label6.Text = "Размер:";
            MessageBox.Show("Язык изменён!");

        }

        private void englishToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            label1.Text = "File:";
            label2.Text = "Hashsum:";
            label3.Text = "Verification hashsum:";
            button1.Text = "Choose";
            button2.Text = "Calculate";
            button3.Text = "Compare";
            error = "Error";
            please_choose_file = "Please choose file";
            entry_verification_sum = "Please entry verification hashsum";
            successful = "Hashsum of the selected file matches the verification hashsum;Successful Comparison";
            not_successful = "Verification hashsum does not match the selected file hashsum!;Not Successful Comparison";
            please_wait = "Please wait, checher is busy!";
            please_drop = "Please drop only one file!";
            file_empty = "File is empty!";
            please_drop_file = "Please drop file, not directory!";
            file_does_not_exists = "File does not exists!";
            please_choose_algorithm = "Please choose algoritm!";

            fileToolStripMenuItem.Text = "File";
            languageToolStripMenuItem.Text = "Language";
            englishToolStripMenuItem1.Text = "English";
            russianToolStripMenuItem1.Text = "Russian";
            infoToolStripMenuItem.Text = "Info";
            label4.Text = "Creation time:";
            label7.Text = "Last changed:";
            label6.Text = "Size:";
            MessageBox.Show("Language changed!");
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomBox.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/thesujewolt");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] arg = e.Argument.ToString().Split('|');
            string mode = arg[0];
            string filepath = arg[1];
            string hash = getMD5hash(filepath);
            if (hash == null)
            {
                e.Result = null;
            }
            else
            {
                e.Result = mode + "|" + hash;  

            }
            


        }
        private static string MakeHashString(byte[] hashBytes)
        {
            StringBuilder hash = new StringBuilder(32);

            foreach(byte b in hashBytes){
                hash.Append(b.ToString("X2").ToLower());

            }
            return hash.ToString();
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Result == null){
                return;
            }
            string[] args = e.Result.ToString().Split('|');
            string mode = args[0];
            string result = args[1];
            if (mode == "compute")
            {
                textBox2.Text = result;

            }
            else if (mode == "compare")
            {
                int resulta = compareMD5Hash(result, textBox3.Text);

                if (resulta == 1)
                {
                    string[] successful = this.successful.Split(';');
                    MessageBox.Show(successful[0], successful[1], MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (resulta == 2)
                {
                    string[] not_successful = this.not_successful.Split(';');
                    MessageBox.Show(not_successful[0], not_successful[1], MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            toolStripProgressBar1.Value = 0;
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filedrop = (string[])e.Data.GetData(DataFormats.FileDrop);
            if(filedrop.Length>1){
                MessageBox.Show(please_drop, error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(!File.Exists(filedrop[0])){
                MessageBox.Show(please_drop_file, error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox1.Text = filedrop[0];
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false)){
                e.Effect = DragDropEffects.All;
            }
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void GetChangeLog()
        {
            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create("http://sumjest.ru/index/md5checker/0-7");
            proxy_request.Method = "GET";
            proxy_request.Timeout = 20000;
            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
            string html = "";
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                html = sr.ReadToEnd();
            string a = Regex.Match(html, @"<!--Dangerous--><p>Change log:([\s\S]*)\<!--Dangerous-->").ToString();
            a = a.Replace("<!--Dangerous-->", "");
            a = a.Replace("<p>", "");
            a = a.Replace("</p>", "");
            a = a.Replace("<br />", "");
            a = a.Replace("&nbsp;", "  ");
            MessageBox.Show(a);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                HttpWebResponse res = (HttpWebResponse)HttpWebRequest.Create("http://sumjest.ru/programsinfo/programs.txt").GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new StreamReader(res.GetResponseStream(), encoding))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] linea = line.Split(';');

                        if (line.Split(';')[0].Contains("MD5Checker"))
                        {
                            Version v;
                            if (Version.TryParse(line.Split(';')[1], out v)) { if (v.CompareTo(Version.Parse(Application.ProductVersion)) > 0) { menuStrip1.Items.Add("Вышла новая версия программы!", null, onNewClick); } }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        

    }

        private void onNewClick(object sender, EventArgs e)
        {
            GetChangeLog();
        }
    }
    public enum Algorithm
    {
        MD5,
        SHA256
    }
}
