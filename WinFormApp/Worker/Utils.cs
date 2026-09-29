using System;
using System.IO;
using System.Linq;

namespace WinFormApp
{
    public static class Utils
    {
        public static string GetAssemblyPath()
        {
            // Gde je rapidtdd.exe - tu kopiras Dapper.dll
            return AppContext.BaseDirectory;
        }

        public static string GetDotNetPath()
        {
            return Path.GetDirectoryName(typeof(object).Assembly.Location) ?? "";
        }

        public static string GetNet9RefPath()
        {
            try
            {
                var packs = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "dotnet", "packs", "Microsoft.NETCore.App.Ref");
                if (!Directory.Exists(packs)) return "";
                var ver = Directory.GetDirectories(packs).OrderByDescending(x => x).FirstOrDefault();
                if (ver == null) return "";
                return Path.Combine(ver, "ref", "net9.0");
            }
            catch { return ""; }
        }
    }
}