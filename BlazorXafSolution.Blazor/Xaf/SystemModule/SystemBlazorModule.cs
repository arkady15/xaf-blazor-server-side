using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BlazorXafSolution.Blazor.Services;
using BlazorXafSolution.Blazor.Xaf.Editors;
using BlazorXafSolution.Module.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace BlazorXafSolution.Blazor.Xaf.SystemModule {
    public class TestViewController : ViewController<ListView> {
        public TestViewController() {
            SingleChoiceAction singleChoiceAction = new SingleChoiceAction(this, "TestSingleChoiceAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            singleChoiceAction.Items.Add(new ChoiceActionItem() { ImageName = "Action_New", Caption = "Choice 1", });
            singleChoiceAction.Items.Add(new ChoiceActionItem() { ImageName = "Action_Delete", Caption = "Choice 2" });
            ChoiceActionItem item3 = new ChoiceActionItem() { Caption = "Choice 3" };
            item3.Items.Add(new ChoiceActionItem() { ImageName = "Action_Delete", Caption = "Choice 31" });
            item3.Items.Add(new ChoiceActionItem() { ImageName = "Action_Delete", Caption = "Choice 32" });
            singleChoiceAction.Items.Add(item3);
        }
        protected override void OnActivated() {
            base.OnActivated();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
        }
        protected override void OnViewChanged() {
            base.OnViewChanged();
        }
        protected override void OnFrameAssigned() {
            base.OnFrameAssigned();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            BlazorGridListEditor editor = View.Editor as BlazorGridListEditor;
            if (View.ObjectTypeInfo.Type == typeof(Contact)) {
                editor.Component.SettingsModel.PageSize = 10;
            } else if (View.ObjectTypeInfo.Type == typeof(DemoTask)) {
                editor.Component.SettingsModel.PageSize = 5;
            }
        }
        protected override void OnViewControlsDestroying() {
            base.OnViewControlsDestroying();
        }
        protected override void OnViewChanging(View view) {
            base.OnViewChanging(view);
        }
        protected override void OnViewControllersActivated() {
            base.OnViewControllersActivated();
        }
    }
    [ToolboxItemFilter("Xaf.Platform.Blazor")]
    public sealed class SystemBlazorModule : ModuleBase {
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory) {
            base.RegisterEditorDescriptors(editorDescriptorsFactory);
            editorDescriptorsFactory.RegisterListEditor(EditorAliases.GridListEditor, typeof(Object), typeof(BlazorGridListEditor), true);
            editorDescriptorsFactory.RegisterPropertyEditor(typeof(object), typeof(BlazorPropertyEditor), true);
        }
        protected override IEnumerable<Type> GetDeclaredControllerTypes() {
            return new Type[] {
                typeof(TestViewController),
                typeof(BlazorNewObjectViewController),
                typeof(BlazorModificationsController),
                typeof(BlazorListViewController)
            };
        }
    }
}
