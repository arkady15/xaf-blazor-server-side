using BlazorXafSolution.Blazor.Services;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Components.Actions {
    public abstract class ActionBaseComponent : ComponentBase, IDisposable {
        [Inject]
        private IViewSiteControlProvider ViewSiteControlProvider { get; set; }
        [Inject]
        private IJSRuntime JsRuntime { get; set; }
        public ActionBase Action { get; set; }
        protected string DisabledCssClass => Action.Enabled ? "" : "disabled";

        protected string GetActionImageUrl() => ImageToStringHelper.GetImageBase64(Action.ImageName);
        protected override void OnInit() {
            base.OnInit();
            Action.Changed += Action_Changed;
        }

        private void Action_Changed(object sender, ActionChangedEventArgs e) {
            if (e.ChangedPropertyType == ActionChangedType.Enabled || e.ChangedPropertyType == ActionChangedType.Active) {
                StateHasChanged();
            }
        }
        public virtual void Dispose() {
            Action.Changed -= Action_Changed;
        }
        private async Task<bool> Confirmation() {
            string confirmationMessage = Action.ConfirmationMessage;
            if (!string.IsNullOrEmpty(confirmationMessage)) {
                return await JsRuntime.InvokeAsync<bool>("confirm", confirmationMessage);
            }
            return true;
        }
        public async void Execute() {
            bool confirmationResult = await Confirmation();
            if (confirmationResult) {
                ExecuteCore();
                ViewSiteControlProvider.ViewSiteControl.Render();
            }
        }
        protected abstract void ExecuteCore();
    }
}
