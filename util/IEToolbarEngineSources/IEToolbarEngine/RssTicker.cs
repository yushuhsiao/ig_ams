using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace IEToolbarEngine
{
    using Link = KeyValuePair<string, string>;
    using LinkList = List<KeyValuePair<string, string>>;

    internal class RssTicker: BaseToolbarItem
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        public RssTicker( IEToolbarEngine engine, 
            string caption,
            string hint,
            string rssURL,
            int refreshInterval,
            System.Drawing.Image img,
            string name)
            : base(engine, img)
        {
            this.name = name;
            this.Create(caption, hint, rssURL, refreshInterval);
        }

        public override ToolbarItemType TypeID
        {
            get { return ToolbarItemType.RssTicker; }
        }


        public void PopulateLinksFromCache ()
        {
            CreateStaticLinks ();
            LinkList links = ReadRssFeed (this.CacheFileName, null);
            AddLinksToMenu (links);
        }



        internal void Refresh ()
        {
            Thread thread = new Thread (new ThreadStart (this.SyncRefresh));
            thread.Start ();            
        }

        public override void Reset ()
        {
            if (this.timer != null)
            {
                this.timer.Dispose ();
                this.timer = null;
            }
            base.Reset ();
        }

        private void SyncRefresh ()
        {
            try
            {
                ReadRssFeed (this.Url, this.CacheFileName);                
                this.engine.Invoke (new MethodInvoker (this.PopulateLinksFromCache));
            }
            catch (Exception)
            {
                //MessageBox.Show (ex.Message);
            }
        }

        protected virtual void CreateAdditionalLinks ()
        {
        }

        protected virtual void CreateStaticLinks ()
        {
            this.linkListButton.DropDownItems.Clear ();
            System.Windows.Forms.ToolStripItem refreshItem = this.linkListButton.DropDownItems.Add (refreshTitle);
            refreshItem.Click += new EventHandler (refreshItem_Click);
            CreateAdditionalLinks ();

            this.linkListButton.DropDownItems.Add (new System.Windows.Forms.ToolStripSeparator ());
        }

        internal void AddLinksToMenu (LinkList links)
        {
            foreach (Link link in links)
            {
                System.Windows.Forms.ToolStripItem menuItem = this.linkListButton.DropDownItems.Add (link.Key);
                menuItem.Tag = link.Value;
                menuItem.Click += new EventHandler (menuItem_Click);
            }            
        }

        internal static LinkList ReadRssFeed (string rssUrl, string backupFile)
        {
            LinkList result = new LinkList ();
            try
            {   
                using (XmlReader rdr = XmlReader.Create (rssUrl))
                {
                    XmlDocument xmlDoc = new XmlDocument ();                    
                    xmlDoc.Load (rdr);
                    try
                    {
                        if (!string.IsNullOrEmpty (backupFile)) xmlDoc.Save (backupFile);
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show (ex.Message, "Cache exception");
                    }

                    XmlNodeList items = xmlDoc.SelectNodes ("//item");
                    foreach (XmlNode node in items)
                    {
                        XmlElement elem = node as XmlElement;
                        if (elem != null)
                        {
                            try
                            {
                                XmlElement title = elem.GetElementsByTagName ("title") [0] as XmlElement;
                                XmlElement link = elem.GetElementsByTagName ("link") [0] as XmlElement;
                                Link newLink = new Link (title.InnerText, link.InnerText);
                                result.Add (newLink);                                
                            }
                            catch (Exception)
                            {
                                //MessageBox.Show (ex.Message, "XML exception");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show (ex.Message, "RSS exception");
            }

            return result;
        }

        void refreshItem_Click (object sender, EventArgs e)
        {
            Refresh ();
        }


        protected virtual void LoadSettings ()
        {               
        }

        public void Create (string buttonText,            
            string buttonTooltip,            
            string rssURL,
            int refreshInterval)
        {               
            this.Url = rssURL;
            LoadSettings ();          

            this.linkListButton = new System.Windows.Forms.ToolStripSplitButton ();
            this.linkListButton.Image = this.Image;            
            this.linkListButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkListButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.linkListButton.Text = buttonText;
            this.linkListButton.ToolTipText = buttonTooltip;

            this.linkListButton.ButtonClick += new EventHandler (linkListButton_Click);
            

            items.Add (this.linkListButton);
            engine.Toolbar.Items.AddRange (this.items.ToArray ());

            PopulateLinksFromCache ();
            Refresh ();

            timer = new System.Windows.Forms.Timer ();
            timer.Interval = refreshInterval * 1000;
            timer.Enabled = refreshInterval > 0;
            timer.Start ();
            timer.Tick += new EventHandler (timer_Tick);           

        }

        void timer_Tick (object sender, EventArgs e)
        {
            Refresh ();
        }

        void linkListButton_Click (object sender, EventArgs e)
        {
            this.linkListButton.DropDown.Visible = true;
        }

        void menuItem_Click (object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem menuItem = sender as System.Windows.Forms.ToolStripMenuItem;
            this.engine.Navigate2 (menuItem.Tag.ToString ());
        }

        virtual internal string Url
        {
            get 
            {
                if (!string.IsNullOrEmpty (rssFeedUrl))
                {
                    if (!rssFeedUrl.StartsWith ("http://", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return "http://" + rssFeedUrl;
                    }
                }
                return rssFeedUrl; 
            }
            set
            {
                rssFeedUrl = value;
            }
        }

        protected System.Windows.Forms.ToolStripSplitButton linkListButton;
        protected System.Windows.Forms.Timer timer;
        protected string rssFeedUrl;
        protected string CacheFileName
        {
            get 
            {
                return Path.ChangeExtension (Path.Combine (this.engine.rssFolder, Name), "rss");
            }
        }

        protected string refreshTitle = "Refresh";        

    }
}
