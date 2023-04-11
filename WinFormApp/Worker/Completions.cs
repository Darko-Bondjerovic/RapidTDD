using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WinFormApp
{
    public class Completions : IEnumerable<AutocompleteItem>
    {
        private AutocompleteMenu menu;
        private Worker work;
        private EditForm form;

        public Action RunUpdateDocs = () => { };
        
        public Completions( AutocompleteMenu menu, Worker work, EditForm form)
        {
            this.menu = menu;
            this.work = work;
            this.form = form;
        }

        public async Task<List<Tuple<string,string>>> GetItems(string docname, string word)
        {
            return await work.ReadCompletionItems(docname, word);
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            //get current fragment of the text
            var word = menu.Fragment.Text;
            var docname = form.TabName;

            RunUpdateDocs();            

            List<Tuple<string,string>> methods = null;
            try
            {
                methods = GetItems(docname, word).Result;
                if (methods == null)
                    yield break;
            }
            catch { }

            foreach (var data in methods.Distinct())
            {                
                yield return new MethodAutocompleteItem(data.Item1)
                {                 
                    ToolTipTitle = data.Item2,
                    ToolTipText = data.Item2
                };
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
