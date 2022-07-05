using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
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

        public async Task<List<string>> GetItems(string docname, string word)
        {
            return await work.ReadCompletionItems(docname, word);
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            //get current fragment of the text
            var word = menu.Fragment.Text;
            var docname = form.TabName;

            RunUpdateDocs();            

            List<string> methods = null;
            try
            {
                methods = GetItems(docname, word).Result;
                if (methods == null)
                    yield break;
            }
            catch { }

            foreach (var name in methods)
                yield return
                    new MethodAutocompleteItem(name);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
