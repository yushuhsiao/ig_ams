using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace TopGame
{
    public class ConnectionStringUITypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
            //return base.GetEditStyle(context);
        }

        static Microsoft.Data.ConnectionUI.DataConnectionDialog dialog;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            lock (typeof(ConnectionStringUITypeEditor))
            {
                if (dialog == null)
                {
                    dialog = new Microsoft.Data.ConnectionUI.DataConnectionDialog();
                    dialog.StartPosition = FormStartPosition.CenterScreen;
                    dialog.DataSources.Add(Microsoft.Data.ConnectionUI.DataSource.SqlDataSource);
                    dialog.SelectedDataSource = Microsoft.Data.ConnectionUI.DataSource.SqlDataSource;
                    dialog.SelectedDataProvider = Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider;
                }
            }
            object obj = base.EditValue(context, provider, value);
            dialog.ConnectionString = (string)obj;
            if (Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dialog) == DialogResult.OK)
                obj = dialog.ConnectionString;
            return obj;
        }
    }
}
