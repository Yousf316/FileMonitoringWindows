using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace FileMonitoringWindows
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller;
        public ProjectInstaller()
        {
            InitializeComponent();
            this.serviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();

            // Service Process Installer
            this.serviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;

            // Service Installer
            this.serviceInstaller.ServiceName = "FileMonitoringService";
            this.serviceInstaller.DisplayName = "File Monitoring Service";
            this.serviceInstaller.Description = "A service that monitors a folder for new files and processes them.";
            this.serviceInstaller.StartType = ServiceStartMode.Manual;

            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
                this.serviceProcessInstaller,
                this.serviceInstaller
            });
        }

        


       
    }
}
