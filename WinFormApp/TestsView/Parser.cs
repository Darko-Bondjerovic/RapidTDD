using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffNamespace
{
    public class Parser
    {
        static public string TEST_KEYWORD = "[TEST]";
        static public string EXPC_KEYWORD = "[EXPC]";

        public static List<TestItem> Find(string response)
        {
            response = response.Replace("\r\n", "\n");

            var tests = new List<TestItem>();

            var splited = response.Split(new string[] { TEST_KEYWORD },
                        StringSplitOptions.None);

            foreach (var spl in splited)
            {
                if (string.IsNullOrEmpty(spl.Trim()))
                    continue;

                var indx = spl.IndexOf("\n");

                if (indx > -1)
                {
                    var test = new TestItem();
                    var title = spl.Substring(0, indx).Trim();
                    if (string.IsNullOrEmpty(title))
                        continue;

                    test.name = title;
                    test.act = spl.Substring(indx + 1, spl.Length - indx - 1);

                    var expc = test.act.IndexOf(EXPC_KEYWORD);
                    if (expc > -1)
                    {        
                        test.exp = test.act.Substring(expc + 6);
                        test.act = test.act.Substring(0, expc);
                    }

                    // if we have more than 1 test with same name:
                    test.count = tests.Count(n => n.name.Equals(test.name));
                    tests.Add(test);
                }
            }

            foreach (var t in tests)
                t.UpdateNameByCount();

            return tests;
        }
    }
}