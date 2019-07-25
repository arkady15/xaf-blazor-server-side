using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

namespace BlazorXafSolution.Blazor.Xaf {
    public class BlazorShowViewStrategy : ShowViewStrategyBase {
        public BlazorShowViewStrategy(XafApplication application) : base(application) {
        }
        protected override void ShowViewCore(ShowViewParameters parameters, ShowViewSource showViewSource) {
            ((BlazorApplication)Application).MainWindow.SetView(parameters.CreatedView);
        }


        protected override void ShowMessageCore(MessageOptions options) {
            throw new NotImplementedException();
        }

        protected override void ShowViewFromCommonView(ShowViewParameters parameters, ShowViewSource showViewSource) {
            throw new NotImplementedException();
        }

        protected override void ShowViewFromLookupView(ShowViewParameters parameters, ShowViewSource showViewSource) {
            throw new NotImplementedException();
        }

        protected override void ShowViewFromNestedView(ShowViewParameters parameters, ShowViewSource showViewSource) {
            throw new NotImplementedException();
        }

        protected override void ShowViewInCurrentWindow(ShowViewParameters parameters, ShowViewSource showViewSource) {
            throw new NotImplementedException();
        }

        protected override void ShowViewInModalWindow(ShowViewParameters parameters, ShowViewSource sourceFrame) {
            throw new NotImplementedException();
        }

        protected override Window ShowViewInNewWindow(ShowViewParameters parameters, ShowViewSource showViewSource) {
            throw new NotImplementedException();
        }

        protected override void ShowViewInPopupWindowCore(View view, Action okDelegate, Action cancelDelegate, string okButtonCaption, string cancelButtonCaption) {
            throw new NotImplementedException();
        }
    }
}
