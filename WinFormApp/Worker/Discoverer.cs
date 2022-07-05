using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WinFormApp
{
    public class MainClassInfo
    {
        public TypeInfo MainClass;
        public MethodInfo MainMethod;
    }

    public class Discoverer
    {
        public static MainClassInfo FindStaticEntryMethod(Assembly assembly)
        {
            var candidates = new List<MainClassInfo>();

            foreach (var type in assembly
                .DefinedTypes
                .Where(t => t.IsClass)
                .Where(t => t.GetCustomAttribute<CompilerGeneratedAttribute>() == null))
            {
                FindMainMethodCandidates(type, candidates);
            }

            string MainMethodFullName()
            {
                return "Main";
            }

            if (candidates.Count > 1)
            {
                throw new AmbiguousMatchException(
                    $"Ambiguous entry point. Found multiple static functions named '{MainMethodFullName()}'. " +
                    $"Could not identify which method is the main entry point for this function.");
            }

            if (candidates.Count == 0)
            {
                throw new InvalidProgramException(
                    $"Could not find a static entry point '{MainMethodFullName()}' that accepts option parameters.");
            }

            return candidates[0];
        }

        private static void FindMainMethodCandidates(TypeInfo type, List<MainClassInfo> candidates)
        {
            foreach (var method in type
                .GetMethods(BindingFlags.Static |
                            BindingFlags.Public |
                            BindingFlags.NonPublic)
                .Where(m =>
                    string.Equals("Main", m.Name, StringComparison.OrdinalIgnoreCase)))
            {
                if (method.ReturnType == typeof(void)
                    || method.ReturnType == typeof(int)
                    || method.ReturnType == typeof(Task)
                    || method.ReturnType == typeof(Task<int>))
                {
                    candidates.Add(new MainClassInfo()
                    { MainClass = type, MainMethod = method });
                }
            }
        }
    }
}

