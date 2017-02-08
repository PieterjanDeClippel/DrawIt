using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrawIt
{
    public class Editor : Control
    {
        private Subpictogram icoon;
        public Subpictogram Icoon
        {
            get { return icoon; }
            set
            {
                icoon = value;
                this.Invalidate();
            }
        }
    }
}
