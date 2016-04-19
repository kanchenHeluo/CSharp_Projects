using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Global.SearchSyncService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        [ServiceProcessDescriptionAttribute("SyncService")]
        [TypeConverterAttribute("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        public string ServiceName { get; set; }

        [ServiceProcessDescriptionAttribute("ServiceInstallerStartType")]
        public ServiceStartMode StartType { get; set; }
        public ProjectInstaller()
        {
            InitializeComponent();
            this.ServiceName = "SyncService";
            this.StartType = System.ServiceProcess.ServiceStartMode.Manual;
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
