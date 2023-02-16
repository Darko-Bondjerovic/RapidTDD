using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace DiffNamespace
{
    public class UpdateItem
    {
        public TestItem oldti = new TestItem();
        public TestItem newti = new TestItem();
        
        public bool ForDelete = false;

        public string expected
        {
            get
            {
                return newti.exp == "" ? oldti.exp : newti.exp;
            }
        }

        public UpdateItem() { }

        public UpdateItem(TestItem newti)
        {
            this.newti = newti;
        }

        static public UpdateItem MakeFromOld(TestItem old)
        {
            var result = new UpdateItem();
            result.oldti = old;
            result.ForDelete = true;
            return result;
        }

        public override string ToString()
        {
            return $"{ForDelete,-6}|{oldti.name,-12}|{newti.name,-12}|{expected}";
        }
        
        public string[] ToListViewStr()
        {
            return new[] { oldti.name, newti.name, expected };
        }

        public void SetOldItem(TestItem old)
        {
            this.oldti = old;
        }

        internal bool IsSameNames()
        {
            return oldti.name.Equals(newti.name);
        }
    }

    public class UpdateExp
    {
        // This class copy exp. value from old tests to new tests with same name
        // Find tests with different names and try to mach them (rename or delete)

        protected bool rename = true;
        
        protected List<TestItem> oldList = null;
        protected List<TestItem> newList = null;
        protected List<TestItem> oldDiffs = null;
        
        public List<UpdateItem> Result = new List<UpdateItem>();

        public bool HaveDiffs { get; set; } = false;

        protected bool keepOld = false;        

        public UpdateExp(List<TestItem> oldList, List<TestItem> newList, bool keep = false)
        {
            this.oldList = oldList;
            this.newList = newList;

            oldDiffs = oldList.Except(newList).ToList();
            HaveDiffs = oldDiffs.Count != 0;

            this.keepOld = keep;
            if (keep)
            {
                foreach (var o in oldDiffs)
                {
                    o.Ignore = true;
                    if (o is GroupItem)
                    {
                        var gr = o as GroupItem;
                        foreach (var t in gr.Tests)
                            t.Ignore = true;
                    }
                }

                newList.AddRange(oldDiffs);
            }
        }
        
        public void Find(bool rename)
        {
            this.rename = rename;
            Result.Clear();
            DoFind();
        }

        protected virtual void DoFind()
        {
            FindSame();
            FindDiffs();
        }

        void FindSame()
        {
            foreach(var n in newList)
            {
                var item = new UpdateItem(n);

                var old = oldList
                    .Where(o => o.Equals(n))
                    .FirstOrDefault();

                if (old != null)
                    item.SetOldItem(old);

                Result.Add(item);
            }
        }

        void FindDiffs()
        {
            var indx = 0;

            if (rename)
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    var res = Result[i];
                    if (indx < oldDiffs.Count && res.oldti.name == "")
                    {
                        res.SetOldItem(oldDiffs[indx]);
                        indx++;
                    }
                }
            }

            // add rest of previous different tests 
            for (int i = indx; i < oldDiffs.Count; i++)
            {
                var updItem = UpdateItem.MakeFromOld(oldDiffs[i]);
                updItem.ForDelete = true;
                Result.Add(updItem);
            }
        }

        public void PrevExpToNew()
        {
            foreach (var item in Result)
            {
                var o = item.oldti;
                var n = item.newti;

                if (!item.ForDelete)
                    n.exp = item.expected;
            }
        }
        
        public static bool IsOverlap(int OrigListCount, int dropIndex, List<int> selIndx)
        {
            if (dropIndex == -1)
                dropIndex = OrigListCount;

            var target = Enumerable.Range(dropIndex, selIndx.Count).ToList();
        
            if (target.Intersect(selIndx).Any())
                return true;
            
            return false;
        }

        
        public bool MoveItems(List<int> indexes, int dropIndex)
        {
            if (IsOverlap(this.Result.Count, dropIndex, indexes))
            {
                MessageBox.Show("Can't move selected items to the same items.");
                return false;
            }
            
            var selected = new List<UpdateItem>();

            foreach (var l in indexes)            
                selected.Add(Result[l]);            
            
            for(int i=0; i < selected.Count; i++)
            {
                var indx = dropIndex + i;                
                var old = selected[i].oldti;

                if (dropIndex == -1 || Result.Count <= indx)
                    Result.Add( UpdateItem.MakeFromOld(old));
                else
                    Result[indx].SetOldItem(old);
            }

            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                var indx = indexes[i];
                var res = Result[indx];

                if (res.newti.NoName())
                    Result.RemoveAt(indx);
                else
                    res.SetOldItem(new TestItem());
            }                
            
            return true;
        }

        internal bool NewListIsEmpty()
        {
            return newList.Count == 0;
        }
    }

    public class UpdateExpGroups : UpdateExp
    {
        public UpdateExpGroups (List<TestItem> oldList, List<TestItem> newList, bool keep = false) 
            : base( oldList, newList, keep) { }
        
        protected override void DoFind()
        {
            var newti = GetTestItems(newList);
            var upti = new UpdateExp(GetTestItems(oldList), newti, keepOld);
            upti.Find(rename);
            
            Result.AddRange(upti.Result.ToList());
            this.HaveDiffs = upti.HaveDiffs;
            
            var oldgr = GetGroupItems(oldList);
            var newgr = GetGroupItems(newList);
            if (oldgr.Count == 0 && newgr.Count == 0)
                return;
            
            var upgr = new UpdateExp(oldgr, newgr, keepOld);
            upgr.Find(rename);
            this.HaveDiffs = HaveDiffs || upgr.HaveDiffs;

            foreach (var r in upgr.Result)
            {
                var up = new UpdateExp(GetTests(r.oldti), GetTests(r.newti), keepOld);
                up.Find(rename);

                this.HaveDiffs = HaveDiffs || up.HaveDiffs;

                Result.Add(r);
                Result.AddRange(up.Result);
            }
        }
        
        List<TestItem> GetTests(TestItem item)
        {
            var result = new List<TestItem>();
            
            if (item is GroupItem)
                result = (item as GroupItem).Tests;
            
            return result;
        }
        
        List<TestItem> GetGroupItems(List<TestItem> list)
        {
            return list.Where(x => x is GroupItem).ToList();
        }

        List<TestItem> GetTestItems(List<TestItem> list)
        {
            return list.Where(x => !(x is GroupItem)).ToList();
        }
        
    }

}



