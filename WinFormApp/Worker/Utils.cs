using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WinFormApp
{
    public static class Utils
    {
        public static string GetReferenceAssembliesPath()
        {
            // "C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.X";
            var programFiles =
                Environment.GetFolderPath(Environment.Is64BitOperatingSystem
                    ? Environment.SpecialFolder.ProgramFilesX86
                    : Environment.SpecialFolder.ProgramFiles);
            var path = Path.Combine(programFiles, @"Reference Assemblies\Microsoft\Framework\.NETFramework");
            var directories = Directory.EnumerateDirectories(path).OrderByDescending(Path.GetFileName);
            return directories.FirstOrDefault();
        }

        public static string GetDotNetPath()
        {
            // @"C:\Windows\Microsoft.NET\Framework\v4.0.30319"

            return Path.GetDirectoryName(typeof(System.Runtime.GCSettings)
                .GetTypeInfo().Assembly.Location);
        }

        public static string GetAssemblyPath()
	{
    		return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
	}
    }
}
