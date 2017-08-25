using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Web;
using System.Reflection;
using System.Windows.Forms;
using BandObjectLib;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Xml;

namespace IEToolbarEngine
{
    using Link = KeyValuePair<string, string>;

    [Guid("691CA8EC-7205-4aa9-BDD6-15493D16F835")]
    [ComVisible (true), ClassInterface (ClassInterfaceType.None)]
    public partial class IEToolbarEngine : BandObject
    {
        internal const string AppKey = "Software\\IEToolbar";
        internal const string SettingsKey = AppKey + "\\Settings";
        internal const string LastRunValue = "LastRun";
        internal const string InstalledValue = "Installed";
        const string RegHistoryValue = "History";        
        internal string toolbarFolder;

        internal string dataFolder;
        internal string imagesFolder;
        internal string settingsFolder;
        internal string rssFolder;
        internal string cacheFolder;

        MainMenu menu;
        SearchBoxItem searchBox;
        LinkListItem advantagesItem;
        RssTicker    rss;
        
                
        List<BaseToolbarItem> items = new List<BaseToolbarItem> ();
        
        internal const string cmdPrefix = "jeaks://";
        internal const string cmdClearHistory = "ClearHistory";

        #region "Static Code"

        internal static DateTime InstallationDate
        {
            get
            {
                try
                {
                    using (RegistryKey rk = Registry.LocalMachine.OpenSubKey (IEToolbarEngine.SettingsKey, false))
                    {
                        string val = rk.GetValue (IEToolbarEngine.InstalledValue).ToString();
                        DateTime result = new DateTime (Convert.ToInt64(val));
                        return result;
                    }
                }
                catch (Exception)
                {
                    return DateTime.MaxValue;
                }
            }
            set
            {
                using (RegistryKey rk = Registry.LocalMachine.CreateSubKey (IEToolbarEngine.SettingsKey))
                {
                    rk.SetValue (IEToolbarEngine.InstalledValue, value.Ticks.ToString());
                }
            }
        }

        /// <summary>
        /// Forms URL for internal command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static string WrapInternalCommand (string command)
        {
            return cmdPrefix + command;
        }

        /// <summary>
        /// Gets internal command from URL.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string UnwrapInternalCommand (string url)
        {
            if (!url.StartsWith (cmdPrefix, StringComparison.CurrentCultureIgnoreCase)) return null;
            return url.Substring (cmdPrefix.Length);
        }

        /// <summary>
        /// Loads image from HTTP protocol.
        /// </summary>
        internal static Image GetImageFromHTTP (string sURL)
        {
            Stream str = null;
            HttpWebRequest wReq = (HttpWebRequest) WebRequest.Create (sURL);
            HttpWebResponse wRes = (HttpWebResponse) (wReq).GetResponse ();
            str = wRes.GetResponseStream ();

            return Image.FromStream (str);
        }
                
        /// <summary>
        /// Copys the foder with subfolders.
        /// </summary>
        /// <param name="srcFolder"></param>
        /// <param name="dstFolder"></param>
        internal static void CopyFolder (string srcFolder, string dstFolder, bool overwrite)
        {
            try
            {
                Directory.CreateDirectory (dstFolder);
            }
            catch (Exception) { }

            DirectoryInfo di = new DirectoryInfo (srcFolder);
            foreach (FileInfo fi in di.GetFiles ())
            {
                fi.CopyTo (Path.Combine (dstFolder, fi.Name), overwrite);
            }

            foreach (DirectoryInfo sdi in di.GetDirectories ())
            {
                if (sdi.Name == "." || sdi.Name == "..") continue;
                CopyFolder (sdi.FullName, Path.Combine (dstFolder, sdi.Name), overwrite);
            }

        }

        /// <summary>
        /// Retrieves DateTime from XML encoded string.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static DateTime StringToDateTime (string val)
        {
            return XmlConvert.ToDateTime (val, XmlDateTimeSerializationMode.Utc);
        }

        /// <summary>
        /// Encode DateTime for storing in XML
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        internal static string DateTimeToString (DateTime val)
        {
            return XmlConvert.ToString (val, XmlDateTimeSerializationMode.Utc);
        }

        /// <summary>
        /// Saves the XML document in local file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        internal static void SaveXml (string url, string fileName)
        {
            using (XmlReader rdr = XmlReader.Create (url))
            {
                XmlDocument doc = new XmlDocument ();
                doc.Load (rdr);
                doc.Save (fileName);
            }
        }

        /// <summary>
        /// Folder with application data and files.
        /// </summary>
        public static string DataFolder
        {
            get
            {
                string result = Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData);
                return Path.Combine (result, "IEToolbar");
            }
        }
        #endregion
        
        /// <summary>
        /// Constructor
        /// </summary>
        public IEToolbarEngine ()
        {
            Assembly asm = Assembly.GetExecutingAssembly ();
            string fullName = asm.GetModules () [0].FullyQualifiedName;
            toolbarFolder = Path.GetDirectoryName (fullName);
            dataFolder = DataFolder;
            try
            {
                cacheFolder = Path.Combine (dataFolder, "Cache");
                Directory.CreateDirectory (cacheFolder);                
            }
            catch (Exception){}
                        

            try
            {
                rssFolder = Path.Combine (cacheFolder, "RSS");
                Directory.CreateDirectory (rssFolder);
            }
            catch (Exception){}

            //try
            //{
            //    settingsFolder = Path.Combine(dataFolder, "Settings");
            //    //if (!Directory.Exists (settingsFolder))
            //    {
            //        //MessageBox.Show (Path.Combine (toolbarFolder, "Settings"), settingsFolder);
            //        CopyFolder(Path.Combine(toolbarFolder, "Settings"), settingsFolder, false);

            //    }

            //}
            //catch (Exception) { }

            //try
            //{
            //    imagesFolder = Path.Combine(cacheFolder, "Images");
            //    //if (!Directory.Exists (imagesFolder))
            //    {
            //        CopyFolder(Path.Combine(toolbarFolder, "Images"), imagesFolder, false);
            //    }
            //}
            //catch (Exception) { }

            
            InitializeComponent ();            
        }
               

        internal void Clear ()
        {
            foreach (BaseToolbarItem item in items)
            {
                item.Reset ();
            }
            items.Clear ();
        }

        internal void CreateToolbarItems ()
        {
            try
            {
                // Prevent redrowing of this toolbar control.
                this.SuspendLayout ();

                // Clear previos state.
                this.Clear ();

                Assembly currentAssembly = Assembly.GetAssembly( this.GetType() );

                //
                // Main Menu
                //

                Link link1 = new Link("Home page", "http://kbsoft-group.com/");
                Link link2 = new Link("Clear Search History", IEToolbarEngine.WrapInternalCommand(IEToolbarEngine.cmdClearHistory));
                Image img = Image.FromStream( currentAssembly.GetManifestResourceStream("IEToolbarEngine.main.png") );

                menu = new MainMenu(this, "Company", "Main", new Link[] { link1, link2 }, img);
                
                items.Add (menu);

                //
                // Search box
                //

                img = Image.FromStream( currentAssembly.GetManifestResourceStream("IEToolbarEngine.view.png") );

                searchBox = new SearchBoxItem (this, "<clear>", "terms to serach!",
                    "http://www.google.ru/search?q={0}",
                    "Search here", "Search", "Click to search",new Size (160, 16), 
                    FlatStyle.System, img);

                items.Add (searchBox);

                link1 = new Link("Quality", "http://kbsoft-group.com/");
                link2 = new Link("Imagination", "http://kbsoft-group.com/");
                Link link3 = new Link("Expertise", "http://kbsoft-group.com/");
                Link link4 = new Link("Reliability", "http://kbsoft-group.com/");

                //
                //List of links
                //

                img = Image.FromStream(currentAssembly.GetManifestResourceStream("IEToolbarEngine.magic-wand.png"));

                advantagesItem = new LinkListItem(this, "Advantages", "Advantages", 
                    new Link[] { link1, link2, link3, link4 }, img);

                items.Add(advantagesItem);

                //
                // RSS links
                //

                img = Image.FromStream(currentAssembly.GetManifestResourceStream("IEToolbarEngine.gear.png"));
                rss = new RssTicker(this, "RSS", "RSS Channel", "http://www.euro2008.uefa.com/rss/index.xml", 1440, img, "RSSChannel");

                items.Add(rss);

            }
            finally
            {                   
                this.ResumeLayout ();                
            }            
        }


        public System.Windows.Forms.ToolStrip Toolbar
        {
            get { return this.toolbar; }
        }

        //IEToolbarEngine toolbarEngine;

        internal void Navigate2 (string url)
        {
            try
            {
                if (string.IsNullOrEmpty (url)) return;
                object val = null;
                object objURL = url;
                Explorer.Navigate2 (ref objURL, ref val, ref val, ref val, ref val);
            }
            catch (Exception)
            {
            }
        }

        internal void SmartNavigate (string url)
        {
            string cmd = UnwrapInternalCommand (url);
            if (null != cmd)
            {
                if(string.Compare (cmd, cmdClearHistory, true) == 0)
                {
                    this.searchBox.ClearHistory ();
                    return;
                }
            }

            Navigate2 (string.Format (url, this.searchBox.SearchText));
        }

        

        internal void SaveSearchHistory (SearchBoxItem item)
        {
            byte [] data = item.EncryptedHistory;
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(SettingsKey))
            {
                rk.SetValue (RegHistoryValue, data, RegistryValueKind.Binary);
            }
        }

        internal void LoadSearchHistory (SearchBoxItem item)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(SettingsKey))
            {
                try
                {
                    byte [] data = rk.GetValue (RegHistoryValue) as byte [];
                    item.EncryptedHistory = data;
                }
                catch (Exception)
                {
                }
            }
            
        }

        internal void SaveString (string name, string value)
        {               
            using ( RegistryKey rk = Registry.CurrentUser.CreateSubKey (SettingsKey) )
            {
                rk.SetValue (name, value, RegistryValueKind.String);
            }
        }

        internal string LoadString (string name)
        {
            string result = null;
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey (SettingsKey))
            {
                try
                {
                    result = rk.GetValue (name) as string;                    
                }
                catch (Exception)
                {
                }
            }
            return result;

        }

        private void IEToolbarEngine_Load (object sender, EventArgs e)
        {
            CreateToolbarItems ();            
        }

        //private void tracker_DocumentCompleted (object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    try
        //    {
        //        this.tracker.Document.Click += new HtmlElementEventHandler (Document_Click);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //void Document_Click (object sender, HtmlElementEventArgs e)
        //{
        //    Navigate2 (this.trackerNavigateUrl);
        //    e.BubbleEvent = false;
        //    e.ReturnValue = false;
        //}

    }
}
