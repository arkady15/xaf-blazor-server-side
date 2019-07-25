using BlazorXafSolution.Blazor.Xaf.SystemModule;

namespace BlazorXafSolution.Blazor {
    partial class BlazorXafSolutionBlazorApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private SystemBlazorModule module2;
        private BlazorXafSolution.Module.BlazorXafSolutionModule module3;
        //private BlazorXafSolution.Module.Spa.BlazorXafSolutionSpaModule module4;
        //private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule objectsModule;

        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new SystemBlazorModule();
            this.module3 = new BlazorXafSolution.Module.BlazorXafSolutionModule();
            //this.objectsModule = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();

            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();


            // 
            // BlazorXafSolutionSpaApplication
            // 
            this.ApplicationName = "BlazorXafSolution";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.module3);
            //this.Modules.Add(this.module4);
            //this.Modules.Add(this.objectsModule);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.BlazorXafSolutionSpaApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

    }
}