using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motr.Web.Controls
{
    public delegate String CellEventHandler(Object cellText,Object dataItem);
    public delegate void DeleteEventHandler(Object sender,GridCommandEventArgs args);
}
