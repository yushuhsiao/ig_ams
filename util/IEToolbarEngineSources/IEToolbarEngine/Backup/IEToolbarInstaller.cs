using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using SHDocVw;
using Microsoft.Win32;

namespace IEToolbarEngine
{
    [RunInstaller (true)]
    public partial class IEToolbarInstaller : Installer
    {
        public IEToolbarInstaller ()
        {
            InitializeComponent ();
        }

        /// <summary>
        /// Installation
        /// </summary>
        public override void Install (System.Collections.IDictionary stateSaver)
        {
            base.Install (stateSaver);
            RegistrationServices regsrv = new RegistrationServices ();
            if (!regsrv.RegisterAssembly (this.GetType ().Assembly,
            AssemblyRegistrationFlags.SetCodeBase))
            {
                throw new InstallException ("Failed To Register for COM");
            }

            IEToolbarEngine.InstallationDate = DateTime.Now;
        }

        /// <summary>
        /// Deinstallation
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall (System.Collections.IDictionary savedState)
        {
            Assembly asm = Assembly.GetExecutingAssembly ();
            string fullName = asm.GetModules () [0].FullyQualifiedName;
            string dataFolder = IEToolbarEngine.DataFolder;
            try
            {
                Directory.Delete (dataFolder, true);
            }
            catch (Exception)
            {
                //System.Windows.Forms.MessageBox.Show (ex.Message);
            }

            try
            {
                Registry.LocalMachine.DeleteSubKeyTree (IEToolbarEngine.AppKey);
                Registry.CurrentUser.DeleteSubKeyTree(IEToolbarEngine.AppKey);
            }
            catch (Exception)
            {
            }

            base.Uninstall (savedState);
            RegistrationServices regsrv = new RegistrationServices ();
            if (!regsrv.UnregisterAssembly (this.GetType ().Assembly))
            {
                throw new InstallException ("Failed To Unregister for COM");
            }
        }
    }
}