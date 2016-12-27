using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IEToolbarEngine
{
    //internal class RssTickerEx : RssTicker
    //{
    //    public RssTickerEx (IEToolbarEngine engine, string name)
    //        : base (engine, name)
    //    {
    //    }

    //    internal string FeedName
    //    {
    //        get { return Path.GetFileNameWithoutExtension (this.CacheFileName); }
    //    }

    //    protected override void LoadSettings ()
    //    {     
    //        string val = engine.LoadString (FeedName);
    //        if (!string.IsNullOrEmpty (val)) this.Url = val;
    //    }

    //    protected override void CreateAdditionalLinks ()
    //    {
    //        System.Windows.Forms.ToolStripItem configureItem = this.linkListButton.DropDownItems.Add (configureTitle);
    //        configureItem.Click += new EventHandler (configureItem_Click);
    //    }

    //    override internal string Url
    //    {
    //        get { return base.Url; }
    //        set
    //        {   
    //            base.Url = value;
    //            engine.SaveString (FeedName, value);
    //            Refresh ();
    //        }
    //    }

    //    void configureItem_Click (object sender, EventArgs e)
    //    {
    //        ConfigureRSS dlg = new ConfigureRSS (this);
    //        dlg.ShowDialog ();
    //    }

    //    public override bool RestoreGuts (System.Xml.XmlElement element)
    //    {
    //        Reset ();
    //        try
    //        {
    //            try
    //            {
    //                configureTitle = ReadStringByTag (element, GetMenuItem (element, "Configure").Attributes ["caption"].Value);
    //            }
    //            catch (Exception) { }
                 
    //            return base.RestoreGuts(element);
    //        }
    //        catch (Exception)
    //        {
    //        }

    //        return false;
    //    }


    //    protected string configureTitle = "Configure...";
    //}
}
