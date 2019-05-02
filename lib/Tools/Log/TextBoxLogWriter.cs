#if netfx
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    [DesignTimeVisible(false), DesignerCategory("")]
	[_DebuggerStepThrough]
	[Docking(DockingBehavior.AutoDock)]
	//[ProvideProperty("Groups", typeof(Control))]
	//[ProvideProperty("All", typeof(Control))]
	//[ProvideProperty("Interval", typeof(Control))]
	//[ProvideProperty("MaxLength", typeof(Control))]
	public sealed class TextBoxLogWriter : TextBox, ILogWriter
	{
		//public bool CanExtend(object extendee)
		//{
		//	Control control = extendee as Control;
		//	return ((control != null) && (this.Parent == control));
		//}

		//List<int> groups = new List<int>();
		//[Category("MessagePage"), DisplayName("Group")]
		//public int[] GetGroups(Control myControl)
		//{
		//	return Groups;
		//}
		//public void SetGroups(Control myControl, int[] value)
		//{
		//	Groups = value;
		//}
		//public int[] Groups
		//{
		//	get { return groups.ToArray(); }
		//	set { groups.Clear(); groups.AddRange(value); }
		//}

		//[Category("MessagePage"), DisplayName("ShowAll")]
		//public bool GetAll(Control myControl)
		//{
		//	return All;
		//}
		//public void SetAll(Control myControl, bool value)
		//{
		//	All = value;
		//}
		//public bool All { get; set; }

		//public int GetInterval(Control myControl)
		//{
		//	return Interval;
		//}
		//public void SetInterval(Control myControl, int value)
		//{
		//	Interval = value;
		//}
		[Category("MessagePage"), DisplayName("Interval")]
        public int Interval
		{
			get { return updateTimer.Interval; }
			set { if (value > 0) updateTimer.Interval = value; }
		}

		public int GetMaxLength(Control myControl)
		{
			return MaxLength;
		}
		public void SetMaxLength(Control myControl, int value)
		{
			MaxLength = value;
		}
		public int m_MaxLength = 1024 * 1024;
		[Category("MessagePage"), DisplayName("MaxLength")]
		[DefaultValue(1024 * 1024)]
		public new int MaxLength
		{
			get { return m_MaxLength; }
			set { m_MaxLength = value; }
		}

		string m_TimeFormat = null;
		[DefaultValue(log.DefaultTimeFormat)]
		public string TimeFormat
		{
			get { return m_TimeFormat ?? log.DefaultTimeFormat; }
			set { m_TimeFormat = ((value == log.DefaultTimeFormat) || (value == string.Empty)) ? null : value; }
		}

		private System.Windows.Forms.Timer updateTimer;
		private IContainer components;

		public TextBoxLogWriter()
		{
			log.AddWriter(this);
			this.components = new System.ComponentModel.Container();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// updateTimer
			// 
			this.updateTimer.Enabled = true;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// MessagePage
			// 
			this.Multiline = true;
			this.ReadOnly = true;
			this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.WordWrap = false;
			this.ResumeLayout(false);

            this.Categorys_Include = new string[0];
            this.Categorys_Exclude = new string[0];
		}

		StringBuilder buf2 = new StringBuilder();
		private void updateTimer_Tick(object sender, EventArgs e)
		{
			if (updateTimer.Tag == null)
			{
				try
				{
					updateTimer.Tag = updateTimer;
					if (!Monitor.TryEnter(buf2))
						return;
					string s;
					try
					{
						if (buf2.Length == 0)
							return;
						s = buf2.ToString();
						buf2.Length = 0;
					}
					finally { Monitor.Exit(buf2); }
					this.AppendText(s);
					if (this.TextLength > this.m_MaxLength)
					{
						this.Select(0, this.m_MaxLength / 2);
						this.SelectedText = null;
					}
					this.ClearUndo();
				}
				catch { }
				finally
				{
					updateTimer.Tag = null;
				}
			}
		}

        //public string[] GetCategorys(Control myControl)
        //{
        //    return Categorys;
        //}
        //public void SetCategorys(Control myControl, string[] value)
        //{
        //    Categorys = value;
        //}
        [Category("MessagePage"), DisplayName("Includes")]
        public string[] Categorys_Include { get; set; }

        [Category("MessagePage"), DisplayName("Excludes")]
        public string[] Categorys_Exclude { get; set; }


        static string[] _null = new string[0];

        void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
		{
            string[] n1 = this.Categorys_Include ?? _null;
            string[] n2 = this.Categorys_Exclude ?? _null;
            string c = category ?? "";

            if (n1.Length > 0)
            {
                int n = n1.Length;
                bool m = false;
                for (int i = 0; (m == false) && (i < n); i++)
                    m = string.Compare(n1[i], c, true) == 0;
                if (m == false) return;
            }
            else if (n2.Length > 0)
            {
                int n = n2.Length;
                for (int i = 0; i < n; i++)
                {
                    if (string.Compare(n2[i], c, true) == 0)
                        return;
                }
            }

            lock (buf2)
			{
				buf2.Append(time.ToString(this.TimeFormat));
				buf2.Append('\t');
				buf2.Append(category);
				buf2.Append('\t');
				buf2.Append(message);
				buf2.AppendLine();
			}
		}
	}
}
#endif