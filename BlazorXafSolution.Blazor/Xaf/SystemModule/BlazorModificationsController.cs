using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Xaf.SystemModule {
    public class BlazorModificationsController : ModificationsController {
        protected override void Save(SimpleActionExecuteEventArgs args) {
            View.ObjectSpace.CommitChanges();
        }
        protected override void UpdateActionState() {
            base.UpdateActionState();
            SaveAction.Active["DetailViewOnly"] = View is DetailView;
            SaveAndCloseAction.Active["DetailViewOnly"] = View is DetailView;
        }
    }
}
