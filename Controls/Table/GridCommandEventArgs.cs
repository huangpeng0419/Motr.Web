using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Web.Controls
{
    public class GridCommandEventArgs : EventArgs
    {
        public String CommandType { get; set; }
        public String DateKeyValue { get; set; }
        public Int32 PageIndex { get; set; }
    }
}
