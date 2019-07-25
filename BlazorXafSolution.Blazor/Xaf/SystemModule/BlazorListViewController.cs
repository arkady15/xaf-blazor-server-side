using BlazorXafSolution.Blazor.Services;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace BlazorXafSolution.Blazor.Xaf.SystemModule {
    public class BlazorListViewController : ViewController<ListView> {
        public BlazorListViewController() {
            SimpleAction editAction = new SimpleAction(this, "Edit", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            editAction.Execute += EditAction_Execute;
            editAction.ImageName = "Action_Edit";
            editAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
        }
        private void EditAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ViewUrlHelper urlHelper = (ViewUrlHelper)((BlazorApplication)Application).ServiceProvider.GetService(typeof(ViewUrlHelper));
            string viewId = Application.GetDetailViewId(View.ObjectTypeInfo.Type);
            string objectKey = ObjectSpace.GetKeyValueAsString(e.CurrentObject);
            urlHelper.NavigateToView(viewId, objectKey);
        }
    }
}
