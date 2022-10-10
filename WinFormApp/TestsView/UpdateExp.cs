using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DiffNamespace
{
    public class UpdateItem
    {
        public bool IsSame = false;
        public string oldName = "";
        public string newName = "";
        public string expected = "";

        public UpdateItem() { }

        public UpdateItem(TestItem newt)
        {
            this.newName = newt.name;
            this.expected = newt.exp;
        }

        static public UpdateItem MakeFromOld(TestItem old)
        {
            return new UpdateItem()
            {
                oldName = old.name,
                newName = "",
                expected = old.exp
            };
        }

        public override string ToString()
        {
            return $"{IsSame,-6}|{oldName,-12}|{newName,-12}|{expected}";
        }

        public void UpdateFromOld(TestItem old)
        {
            this.oldName = old.name;
            if (expected == "")
                expected = old.exp;
        }
    }

    public class UpdateExp
    {
        // this class copy exp. value from old tests to new tests, if new tests 
        // does not exists in list of old tests (and exp.value is empty)

        public enum Operation { DeleteDiffs, RenameDiffs };

        Operation operation = Operation.RenameDiffs;

        List<TestItem> oldList = null;
        List<TestItem> newList = null;

        List<TestItem> oldDiffs = null;

        public List<UpdateItem> result = new List<UpdateItem>();

        public UpdateExp(List<TestItem> oldList, List<TestItem> newList)
        {
            this.oldList = oldList;
            this.newList = newList;

            oldDiffs = oldList.Except(newList).ToList();
        }

        public void Find(bool rename)
        {
            var op = rename ? Operation.RenameDiffs 
                : Operation.DeleteDiffs;

            Find(op);
        }

        public void Find(Operation operation)
        {
            if (oldList.Count == 0)
                return;

            this.operation = operation;

            result.Clear();
            FindForSame();
            FindForDiffs();
        }

        public void FindForSame()
        {
            for (int i = 0; i < newList.Count; i++)
            {
                var n = newList[i];

                var item = new UpdateItem(n);

                var o = oldList
                    .Where(x => x.name.Equals(n.name))
                    .FirstOrDefault();

                if (o != null)
                {
                    item.IsSame = true;
                    item.UpdateFromOld(o);
                }

                result.Add(item);
            }
        }

        internal int GetNewCount()
        {
            return newList.Count;
        }

        public bool HaveDiffs()
        {
            if (oldDiffs.Count == 0)
                return false; // nothing to update / rename

            return true;
        }

        public void FindForDiffs()
        {
            if (!HaveDiffs())
                return;

            var indx = 0;
            
            if (operation == Operation.RenameDiffs)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    var res = result[i];

                    if (indx < oldDiffs.Count)
                    {
                        var o = oldDiffs[indx];

                        if (res.oldName == "" && res.expected == "")
                        {
                            
                            res.UpdateFromOld(o);
                            indx++;
                        }                    
                    }
                }
            }

            // add rest of previous different tests 
            for (int i = indx; i < oldDiffs.Count; i++)
                result.Add(UpdateItem.MakeFromOld(oldDiffs[i]));
        }
        
        public void PrevExpToNew()
        {
            for(int i=0; i<newList.Count; i++)
                if (i < result.Count)
                    newList[i].exp = result[i].expected;
        }
    }
}