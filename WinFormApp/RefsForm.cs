using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class RefsForm : Form
    {
        static string refsFile = @"..\..\RefsDir\files.txt";

        static List<string> paths = new List<string>();

        static List<string> files = new List<string>();

        public RefsForm()
        {
            InitializeComponent();
            
            LoadPaths();
            LoadFiles();

            txtFiles.Text = string.Join(Environment.NewLine, files);
            txtPaths.Text = string.Join(Environment.NewLine, paths);

            this.Text = Path.GetFullPath(refsFile);

            this.Load += RefsForm_Load; 
        }

        private void RefsForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnSave;
            btnSave.Focus();
        }

        private static void LoadFiles()
        {
            if (File.Exists(refsFile))            
                files = File.ReadAllLines(refsFile).ToList();
        }

        private static void LoadPaths()
        {
            paths.Clear();
            paths.Add(Utils.GetAssemblyPath());
            paths.Add(Utils.GetDotNetPath());            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var full = Path.GetFullPath(refsFile);
            var path = Path.GetDirectoryName(full);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(full, txtFiles.Text);
            Close();
        }

        internal static List<string> GetRefsFilesList()
        {
            LoadPaths();
            LoadFiles();

            var result = new List<string>();

            foreach (var file in files)
            {
                foreach (var path in paths)
                {
                    var full = Path.Combine(path, file);
                    if (File.Exists(full))
                    {
                        result.Add(full);
                        continue;
                    }
                }
            }

            return result;
        }
    }
}
