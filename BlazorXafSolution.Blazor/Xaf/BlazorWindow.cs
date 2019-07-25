using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;

namespace BlazorXafSolution.Blazor.Xaf {
    public class BlazorWindow : Window, IDisposable {
        public BlazorWindow(XafApplication application, TemplateContext context, ICollection<Controller> controllers, bool isMain, bool activateControllersImmediately)
: base(application, context, controllers, isMain, activateControllersImmediately) {
        }


    }
}
