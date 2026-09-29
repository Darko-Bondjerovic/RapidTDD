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
                // Ovo više ne postoji u .NET 9, ostavi samo ono što ti treba za custom dll-ove
                files = new List<string>
                {
                    "System.Windows.Forms.dll",
                    "System.Drawing.dll",
                    "netstandard.dll",
                    // OVDE KUCAJ TVOJE: npr. Dapper.dll, Newtonsoft.Json.dll
                };
            }
        }

        private static void LoadPaths()
        {
            paths.Clear();
            paths.Add(Utils.GetAssemblyPath()); // AppContext.BaseDirectory - gde je exe
            paths.Add(Path.Combine(Utils.GetAssemblyPath(), "RefsDir"));
            paths.Add(Path.Combine(Utils.GetAssemblyPath(), "Refs"));
            paths.Add(Utils.GetDotNetPath()); // runtime folder
            paths.Add(Utils.GetNet9RefPath()); // C:\Program Files\dotnet\packs\...\ref\net9.0
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
                // 1. Ako je upisana apsolutna putanja - uzmi direktno
                if (Path.IsPathRooted(file) && File.Exists(file))
                {
                    result.Add(file);
                    continue;
                }

                var fileName = Path.GetFileName(file);

                // 2. Traži po svim path-evima
                foreach (var path in paths.Distinct())
                {
                    if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                        continue;

                    var full = Path.Combine(path, fileName);
                    if (File.Exists(full))
                    {
                        result.Add(full);
                        break;
                    }
                    // probaj i ako je u txt upisano "RefsDir\Dapper.dll"
                    var full2 = Path.Combine(path, file);
                    if (File.Exists(full2))
                    {
                        result.Add(full2);
                        break;
                    }
                }
            }
            return result.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }
    }
}
