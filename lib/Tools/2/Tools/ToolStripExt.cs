#if NET452
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    [ToolboxBitmap(typeof(NumericUpDown))]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        public ToolStripNumericUpDown() : base(new NumericUpDown()) { }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NumericUpDown NumericUpDown { get { return Control as NumericUpDown; } }

        public event EventHandler ValueChanged { add { NumericUpDown.ValueChanged += value; } remove { NumericUpDown.ValueChanged -= value; } }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
    [ToolboxBitmap(typeof(Panel))]
    public class ToolStripPanel : ToolStripControlHost
    {
        public ToolStripPanel()
            : base(new Panel())
        {
            Panel.Dock = DockStyle.Fill;
            this.Width = 100;
            this.AutoSize = false;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Panel { get { return Control as Panel; } }
    }

    //[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    //[ToolboxBitmap(typeof(System.Windows.Forms.ToolStripButton))]
    //public class ToolStripButton : System.Windows.Forms.ToolStripButton
    //{
    //}

    //[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    //[ToolboxBitmap(typeof(System.Windows.Forms.ToolStripLabel))]
    //public class ToolStripLabel : System.Windows.Forms.ToolStripLabel
    //{
    //}


    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    [ToolboxBitmap(typeof(Panel))]
    public class ToolStripTextBox_Label : ToolStripTextBox
    {
        internal ToolStripLabel_TextBox label;
        public ToolStripLabel_TextBox Label
        {
            get { return label; }
            set { label = value; this.init(); }
        }

        internal void init()
        {
            if (label == null)
                return;
            label.textBox = this;
            OnLeave(EventArgs.Empty);
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (label != null)
            {
                this.Visible = false;
                label.Visible = true;
            }
        }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    [ToolboxBitmap(typeof(Panel))]
    public class ToolStripLabel_TextBox : ToolStripLabel
    {
        internal ToolStripTextBox_Label textBox;

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (textBox != null)
            {
                this.Visible = false;
                textBox.Visible = true;
                textBox.Focus();
            }
        }
    }

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class ToolStripLabel2 : ToolStripStatusLabel
    {
    }
}
#endif