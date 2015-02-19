namespace TravelRequestApproval
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TravelAgencyProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.TravelAgencyInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // TravelAgencyProcessInstaller
            // 
            this.TravelAgencyProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.TravelAgencyProcessInstaller.Password = null;
            this.TravelAgencyProcessInstaller.Username = null;
            this.TravelAgencyProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.TravelAgencyProcessInstaller_AfterInstall);
            // 
            // TravelAgencyInstaller
            // 
            this.TravelAgencyInstaller.Description = "Runs approval and emailing components for the TravelRequest application";
            this.TravelAgencyInstaller.DisplayName = "TravelRequestApprovalService";
            this.TravelAgencyInstaller.ServiceName = "TRAservice";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TravelAgencyProcessInstaller,
            this.TravelAgencyInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller TravelAgencyInstaller;
        private System.ServiceProcess.ServiceProcessInstaller TravelAgencyProcessInstaller;
    }
}