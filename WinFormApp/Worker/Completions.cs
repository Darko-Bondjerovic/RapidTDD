using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormApp
{
    public class Completions : IEnumerable<AutocompleteItem>
    {
        private AutocompleteMenu menu;
        private Worker work;
        private EditForm form;

        public Action RunUpdateDocs = () => { };

        // Kes po kontekstu - kljuc je "doc@pos:contextPart" npr "new 1@120:System."
        private static Dictionary<string, List<Tuple<string, string>>> _cache = new Dictionary<string, List<Tuple<string, string>>>();
        private static string _lastContextKey = "";
        private static List<Tuple<string, string>> _fullList = new List<Tuple<string, string>>();
        private static int _requestVersion = 0;
        private static Task<List<Tuple<string, string>>> _pendingTask = null;

        public Completions(AutocompleteMenu menu, Worker work, EditForm form)
        {
            this.menu = menu;
            this.work = work;
            this.form = form;
        }

        public IEnumerator<AutocompleteItem> GetEnumerator()
        {
            var fragment = menu.Fragment;
            if (fragment == null) yield break;

            string fullText = fragment.Text; // "System.Con" ili "System."
            string docname = form.TabName;
            int fragStartPos = form.fctb.PlaceToPosition(fragment.Start);

            int lastDot = fullText.LastIndexOf('.');
            string contextPart = lastDot >= 0 ? fullText.Substring(0, lastDot + 1) : "";
            string filterPart = lastDot >= 0 ? fullText.Substring(lastDot + 1) : fullText;
            string contextKey = docname + "@" + fragStartPos + ":" + contextPart;

            int roslynPos = fragStartPos + (lastDot >= 0 ? lastDot + 1 : 0);

            // ISTI KONTEKST - samo filtriraj lokalno, ne zovi Roslyn
            if (contextKey == _lastContextKey && _fullList.Count > 0)
            {
                if (_pendingTask != null && !_pendingTask.IsCompleted)
                    _pendingTask.Wait(15);

                var filtered = string.IsNullOrEmpty(filterPart)
                    ? _fullList
                    : _fullList.Where(x => x.Item1.StartsWith(filterPart, StringComparison.InvariantCultureIgnoreCase)).ToList();

                foreach (var data in filtered)
                    yield return new MethodAutocompleteItem(data.Item1) { ToolTipTitle = data.Item2, ToolTipText = data.Item2 };
                yield break;
            }

            // NOVI KONTEKST
            // 1. Probaj cache za ovaj contextKey
            if (_cache.TryGetValue(contextKey, out var cachedList) && cachedList.Count > 0)
            {
                _lastContextKey = contextKey;
                _fullList = cachedList;
                var filtered = string.IsNullOrEmpty(filterPart)
                    ? _fullList
                    : _fullList.Where(x => x.Item1.StartsWith(filterPart, StringComparison.InvariantCultureIgnoreCase)).ToList();

                foreach (var data in filtered)
                    yield return new MethodAutocompleteItem(data.Item1) { ToolTipTitle = data.Item2, ToolTipText = data.Item2 };
                yield break;
            }

            // 2. Nema cache - moramo Roslyn, ali NE prikazuj staru listu!
            // Sinhronizuj dokument PRE nego sto pokrenes task
            try { RunUpdateDocs(); } catch { }

            _lastContextKey = contextKey;
            int myVersion = ++_requestVersion;
            string cDoc = docname;
            int cPos = roslynPos;

            _pendingTask = Task.Run(async () =>
            {
                try
                {
                    var list = await work.ReadCompletionItems(cDoc, "", cPos).ConfigureAwait(false);
                    if (myVersion != _requestVersion) return new List<Tuple<string, string>>();
                    return list ?? new List<Tuple<string, string>>();
                }
                catch { return new List<Tuple<string, string>>(); }
            });

            // Sacekaj do 300ms da dobijemo tacnu listu - ne prikazuj gluposti
            List<Tuple<string, string>> resultList = null;
            if (_pendingTask.Wait(300))
                resultList = _pendingTask.Result;
            else
                resultList = _cache.TryGetValue(contextKey, out var cl) ? cl : null;

            if (resultList != null && resultList.Count > 0)
            {
                _fullList = resultList;
                _cache[contextKey] = resultList; // kesiraj

                var filtered = string.IsNullOrEmpty(filterPart)
                    ? _fullList
                    : _fullList.Where(x => x.Item1.StartsWith(filterPart, StringComparison.InvariantCultureIgnoreCase)).ToList();

                foreach (var data in filtered)
                    yield return new MethodAutocompleteItem(data.Item1) { ToolTipTitle = data.Item2, ToolTipText = data.Item2 };
            }
            else
            {
                // Jos uvek se racuna - ako imamo BILO STA u kesu za slican kontekst (npr "System."), ne prikazuj nista
                // da ne bi prikazali WriteLine umesto Console. Bolje prazno 200ms nego pogresno.
                // Ali da ne blinka prazno, prikazi loading samo ako nema nista
                if (_pendingTask != null)
                {
                    var ver = _requestVersion;
                    _pendingTask.ContinueWith(t =>
                    {
                        if (ver != _requestVersion) return;
                        if (t.Result == null || t.Result.Count == 0) return;
                        try
                        {
                            var ctrl = form as Control;
                            if (ctrl != null && ctrl.IsHandleCreated && !ctrl.IsDisposed)
                            {
                                ctrl.BeginInvoke((Action)(() =>
                                {
                                    try
                                    {
                                        _cache[contextKey] = t.Result;
                                        if (_lastContextKey == contextKey)
                                            _fullList = t.Result;

                                        if (menu.Visible && _lastContextKey == contextKey)
                                        {
                                            var fp = filterPart; // closure
                                            var fl = string.IsNullOrEmpty(fp)
                                                ? t.Result
                                                : t.Result.Where(x => x.Item1.StartsWith(fp, StringComparison.InvariantCultureIgnoreCase)).ToList();

                                            menu.Items.SetAutocompleteItems(
                                                fl.Select(d => new MethodAutocompleteItem(d.Item1) { ToolTipTitle = d.Item2, ToolTipText = d.Item2 })
                                            );
                                        }
                                    }
                                    catch { }
                                }));
                            }
                        }
                        catch { }
                    }, TaskScheduler.Default);
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
