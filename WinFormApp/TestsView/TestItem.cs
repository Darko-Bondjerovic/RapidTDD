using System.Drawing;

namespace DiffNamespace
{
    public class TestItem
    {
        private string _exp = "";
        private string _act = "";
        public string name = "";
        public bool pass = false;

        public int count { get; set; } = 0;

        public override string ToString()
        {
            return $"{name}\n{act}\n{exp}";
        }

        public void UpdateNameByCount()
        {
            if (count > 0)
                this.name = $"{++count}.{name}";
        }

        public string act
        {
            get { return _act; }
            set
            {
                _act = value;
                CheckIfPass();
            }
        }
        public string exp
        {
            get { return _exp; }
            set
            {
                _exp = value;
                CheckIfPass();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            TestItem c = obj as TestItem;
            if (c == null)
                return false;
            if (name.Equals(c.name))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        internal void CopyActToExp()
        {
            this._exp = this.act;
            this.pass = true;
        }

        internal Color GetDefaultColor()
        {
            return pass ? Color.YellowGreen : Color.Salmon;
        }

        internal Color GetSelectColor()
        {
            return pass ? Color.OliveDrab : Color.IndianRed;
        }

        internal void CheckIfPass()
        {            
            this.pass = act.Equals(exp);
        }

        internal void PaintBoxUI(ColoredTextBox box)
        {
            if (this.pass)
                box.ClearColors(this);
            else
                box.PaintDiffs(this);
        }
    }
}
