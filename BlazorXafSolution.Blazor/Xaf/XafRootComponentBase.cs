using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorXafSolution.Blazor.Services;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionContainers;
using Microsoft.AspNetCore.Components;

namespace BlazorXafSolution.Blazor.Xaf {
    //public interface IViewShortcutHolder {
    //    ViewShortcut GetViewShortcut();
    //}

    public class XafRootComponentBase : ComponentBase, IWindowTemplate, IDynamicContainersTemplate { //, IViewShortcutHolder {
        [Inject]
        private IBlazorApplicationProvider ApplicationProvider { get; set; }
        [Inject]
        private ViewUrlHelper ViewUrlHelper { get; set; }
        [Parameter]
        private string ObjectKey { get; set; }
        [Parameter]
        private string ViewID { get; set; }

        private ActionContainerCollection actionContainers = new ActionContainerCollection();

        private View view = null;

        public View View => view;

        public BlazorApplication Application => ApplicationProvider.Application;
        protected override void OnInit() {
            base.OnInit();
            BlazorApplication application = ApplicationProvider.Application;
            //application.SetViewShortcutHolder(this);
            application.CreateMainWindow().SetTemplate(this);
        }
        private ViewShortcut GetStartupViewShortcut() {
            ShowNavigationItemController controller = Application.MainWindow.GetController<ShowNavigationItemController>();
            SingleChoiceAction showNavigationItemAction = controller != null ? controller.ShowNavigationItemAction : null;
            if (showNavigationItemAction != null && showNavigationItemAction.Active && showNavigationItemAction.Enabled) {
                ChoiceActionItem startupNavigationItem = controller.GetStartupNavigationItem();
                if (startupNavigationItem != null) {
                    return startupNavigationItem.Data as ViewShortcut;
                }
            }
            return null;
        }
        protected override void OnParametersSet() {
            ViewShortcut viewShortcut = GetViewShortcut();
            if (!string.IsNullOrEmpty(viewShortcut.ViewId)) {
                View view = Application.ProcessShortcut(viewShortcut);
                Application.MainWindow.SetView(view);
            }
            base.OnParametersSet();
        }
        protected override void OnAfterRender() {
            base.OnAfterRender();
            ViewShortcut viewShortcut = GetViewShortcut();
            if (string.IsNullOrEmpty(viewShortcut.ViewId) && string.IsNullOrEmpty(viewShortcut.ObjectKey)) {
                ViewShortcut startupViewShortcut = GetStartupViewShortcut();
                if (startupViewShortcut == null) {
                    throw new Exception("Startup view is not found");
                }
                ViewUrlHelper.NavigateToView(startupViewShortcut.ViewId);
            }
            ViewID = null;
        }
        public void Render() {
            StateHasChanged();
        }
        //#region IViewShortcutHolder
        private ViewShortcut GetViewShortcut() {
            return new ViewShortcut(ViewID, ObjectKey);
        }
        //#endregion
        #region IWindowTemplate
        public IActionContainer DefaultContainer => null;
        public bool IsSizeable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public void SetStatus(ICollection<string> statusMessages) { }
        public void SetCaption(string caption) { }
        public ICollection<IActionContainer> GetContainers() {
            return new ActionContainerCollection();
        }
        public void SetView(View view) {
            this.view = view;
        }

        public void RegisterActionContainers(IEnumerable<IActionContainer> actionContainers) {
            IList<IActionContainer> addedContainers = this.actionContainers.TryAdd(actionContainers);
            if (addedContainers.Count > 0) {
                ActionContainersChanged?.Invoke(this, new ActionContainersChangedEventArgs(addedContainers, ActionContainersChangedType.Added));
            }
        }

        public void UnregisterActionContainers(IEnumerable<IActionContainer> actionContainers) {
            IList<IActionContainer> removedContainers = new List<IActionContainer>();
            foreach (IActionContainer actionContainer in actionContainers) {
                if (this.actionContainers.Contains(actionContainer)) {
                    this.actionContainers.Remove(actionContainer);
                    removedContainers.Add(actionContainer);
                }
            }
            if (removedContainers.Count > 0) {
                ActionContainersChanged?.Invoke(this, new ActionContainersChangedEventArgs(removedContainers, ActionContainersChangedType.Removed));
            }
        }
        public event EventHandler<ActionContainersChangedEventArgs> ActionContainersChanged;
        #endregion

    }
}
