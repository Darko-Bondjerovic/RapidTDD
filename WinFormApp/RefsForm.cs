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
            StartPosition = FormStartPosition.CenterScreen;

            LoadPaths();
            LoadFiles();

            txtFiles.Text = string.Join(Environment.NewLine, files);
            txtPaths.Text = string.Join(Environment.NewLine, paths);

            Text = Path.GetFullPath(refsFile);

            Load += RefsForm_Load;
        }

        private void RefsForm_Load(object sender, EventArgs e)
        {
            ActiveControl = btnSave;
            btnSave.Focus();
        }

        private static void LoadFiles()
        {
            if (File.Exists(refsFile))
            {
                files = File.ReadAllLines(refsFile)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => l.Trim())
                    .ToList();
            }
            else
            {
                files = new List<string>();
            }
        }

        private static void LoadPaths()
        {
            paths.Clear();

            string baseDir = Utils.GetAssemblyPath();

            paths.Add(baseDir);
            paths.Add(Path.Combine(baseDir, "RefsDir"));
            //paths.Add(Path.Combine(baseDir, "Refs"));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var full = Path.GetFullPath(refsFile);
            var dir = Path.GetDirectoryName(full);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

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
                if (string.IsNullOrWhiteSpace(file))
                    continue;

                if (Path.IsPathRooted(file))
                {
                    if (File.Exists(file))
                        result.Add(Path.GetFullPath(file));

                    continue;
                }

                foreach (var path in paths.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    if (!Directory.Exists(path))
                        continue;

                    string full = Path.Combine(path, file);

                    if (File.Exists(full))
                    {
                        result.Add(Path.GetFullPath(full));
                        break;
                    }

                    string fileName = Path.GetFileName(file);

                    if (!string.Equals(fileName, file, StringComparison.OrdinalIgnoreCase))
                    {
                        full = Path.Combine(path, fileName);

                        if (File.Exists(full))
                        {
                            result.Add(Path.GetFullPath(full));
                            break;
                        }
                    }
                }
            }

            return result
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}