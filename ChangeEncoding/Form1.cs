using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ChangeEncoding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool IsPathValid(string path)
        {
            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void ChangeEncoding(FileInfo file, Encoding encoding)
        {
            var content = File.ReadAllText(file.FullName);
            File.WriteAllText(file.FullName, content, encoding);
        }

        private Encoding GetEncoding(int idx)
        {
            switch (idx)
            {
                case 0:
                    {
                        return new UTF8Encoding(false);
                    }
                case 1:
                    {
                        return Encoding.UTF8;
                    }
            }

            return null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = tbPath.Text;
            if (!IsPathValid(path))
            {
                MessageBox.Show("非法的路径！");
            }

            var dir = new DirectoryInfo(path);
            var encoding = GetEncoding(cbEncoding.SelectedIndex);
            var files = dir.GetFiles(tbFilter.Text, SearchOption.AllDirectories);

            pbProgress.Value = 0;
            pbProgress.Maximum = files.Length;
            foreach (var f in files)
            {
                ChangeEncoding(f, encoding);
                pbProgress.Value++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath)) {
                    tbPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbEncoding.SelectedIndex = 0;
        }
    }
}
