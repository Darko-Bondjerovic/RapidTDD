using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffNamespace
{
    public class Parser
    {
        static public string GROUP_KEYWORD = "[GROUP]";
        static public string TEST_KEYWORD = "[TEST]";
        static public string EXPC_KEYWORD = "[EXPC]";
        static public string DEBUG_KEYWORD = "[DEBUG]";
        
        private static string group_default_name = "GROUP";

        private static string DeleteDebug(string input)
        {
            var lines = input.Split(new string[] { "\n" },
                StringSplitOptions.None).ToList();
 
             for (int i = lines.Count - 1; i >= 0; i--)
                 if (lines[i].Contains(DEBUG_KEYWORD)) 
                     lines.RemoveAt(i); 
 
             return string.Join("\n", lines);
        }
        
        public static List<TestItem> Find(string response)
        {
            response = response.Replace("\r\n", "\n");
            
            var all = new List<TestItem>();
            
            if (!response.Contains(GROUP_KEYWORD))
                return FindTests(response);
            
            var grps = response.Split(new string[] { GROUP_KEYWORD },
                    StringSplitOptions.None);

            foreach(var gr in grps)
            {
                var indx = gr.IndexOf("\n");
                if (indx > -1)
                {                    
                    var title = gr.Substring(0, indx).Trim();
                    
                    if (string.IsNullOrEmpty(title))
                        title = group_default_name;
                    
                    var groupItem = new GroupItem();
                    groupItem.name = title;
                    
                    // if we have more than 1 group with same name:
                    groupItem.count = all.Count(n => n.name.Equals(title));
                    all.Add(groupItem);                   
                 
                    var tests = gr.Substring(indx + 1, gr.Length - indx - 1);
                    if (tests.Length > 0)
                        groupItem.Tests = FindTests(tests);
                }
            }
            
            foreach (var gr in all)
                gr.UpdateNameByCount();
            
            return all;
        }
          
        public static List<TestItem> FindTests(string response)            
        {
            var tests = new List<TestItem>();
            
            if (!response.Contains(TEST_KEYWORD))
                return tests;

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
                    
                    var act_text = spl.Substring(indx + 1, spl.Length - indx - 1);
                    
                    test.act = DeleteDebug(act_text);                   
      
                    var expc = test.act.IndexOf(EXPC_KEYWORD);
                    if (expc > -1)
                    {        
                        test.exp = test.act.Substring(expc + 6);
                        test.act = test.act.Substring(0, expc);
                    }

                    // if we have more than 1 test with same name:
                    test.count = tests.Count(n => n.name.Equals(title));
                    tests.Add(test);
                }
            }

            foreach (var t in tests)
                t.UpdateNameByCount();

            return tests;
        }
    }
}