using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Linq;

namespace DiffNamespace
{    
    public class GroupItem : TestItem
    {
        public List<TestItem> Tests = new List<TestItem>();
        
        public GroupItem() : base() {}
        
        public GroupItem (XElement e) : base(e) { }

        internal override void CheckIfPass() { }

        protected override bool IsPass()
        {
            return Tests.All(item => item.pass);
        }
    }
    
    public class TestItem
    {
        public bool Ignore = false;

        private bool _pass = false;
        public bool pass
        {
            get { return IsPass(); }
            set { _pass = value; }
        }

        protected virtual bool IsPass()
        {
            return _pass;
        }

        private string _exp = "";
        private string _act = "";
        
        public string name = "";
        public int count { get; set; } = 0;
        
        public void UpdateNameByCount()
        {
            if (count > 0)
                this.name = $"{++count}.{name}";
        }
        
        public TestItem Clone() 
        {
            return new TestItem 
            {
                name = this.name, 
                exp = this.exp,
                act = this.act,
                pass = this.pass,
                count = this.count
            };
        }
        
        public TestItem() { } 
        
        public TestItem(string name, string exp)
        {
            this.name = name;
            this.exp = exp;
        }
        
        public TestItem (XElement e)
        {
            if (e == null)
                throw new ArgumentException("XElement is null!");
            
            this.name = e.Element("name").Value;
            //pass = bool.Parse(e.Element("pass").Value),
            _act = e.Element("act").Value;
            _exp = e.Element("exp").Value;
            
            CheckIfPass();
        }

        public override string ToString()
        {
            return $"{name}\n{act}\n{exp}";
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
            if (!Ignore)
            {
                this._exp = this.act;
                this.pass = true;
            }
        }

        internal Color GetDefaultColor()
        {
            return pass ? Color.YellowGreen : Color.Salmon;
        }

        internal Color GetSelectColor()
        {
            return pass ? Color.OliveDrab : Color.IndianRed;            
        }

        internal virtual void CheckIfPass()
        {
            this.pass = act.Equals(exp);
        }
        
        internal XElement ToXml()
        {
            return new XElement(this.GetType().Name,
                new XElement("name", name),
                new XElement("pass", pass),
                new XElement("act", act),
                new XElement("exp", exp));
        }

        internal void PaintBoxUI(ColoredTextBox box)
        {
            if (this.pass)
                box.ClearColors(this);
            else
                box.PaintDiffs(this);
        }

        internal bool NoName()
        {
            return string.IsNullOrEmpty(this.name);
        }
    }
}
