using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class RefsForm : Form
    {
        #if DEBUG
            static string refsFile = @"..\..\RefsDir\references.txt";
        #else
            static string refsFile = @"RefsDir\references.txt";
        #endif

        static List<string> paths = new List<string>();

        static List<string> files = new List<string>();

        public RefsForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            //this.ShowInTaskbar = false;

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
            else
            {
                files.Add("System.Windows.Forms.dll");                
                files.Add("System.Drawing.dll");
                files.Add("System.Web.dll");
                files.Add("System.Xml.Linq.dll");                
                files.Add("System.Data.dll");
                files.Add("System.Data.SqlClient.dll");
                files.Add("netstandard.dll");
            }
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
