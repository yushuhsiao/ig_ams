using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IEToolbarEngine
{
    internal class MainMenu : LinkListItem
    {
        internal MainMenu(IEToolbarEngine engine, 
            string caption,
            string hint,
            KeyValuePair<string, string>[] links,
            System.Drawing.Image img)
            : base(engine, caption, hint, links, img)
        {
        }

        public override ToolbarItemType TypeID
        {
            get { return ToolbarItemType.MainMenu; }
        }

        public void Create (string logoTooltip,                            
                            KeyValuePair<string, string> [] links)
        {
            base.Create ("", logoTooltip, links);
        }
    }
}
