using BlazorXafSolution.Blazor.Xaf.Templates;
using Microsoft.AspNetCore.Components;

namespace BlazorXafSolution.Blazor.Components.Actions
{
    public class BlazorActionContainerComponent : ComponentBase
    {
        [CascadingParameter] protected ActionContainerHolder ActionContainerHolder { get; set; }
        [Parameter] string ContainerId { get; set; }
        [Parameter] string DefaultActionID { get; set; }
        [Parameter] bool IsDropDown { get; set; }
        [Parameter] bool AutoChangeDefaultAction { get; set; }
        protected override void OnInit()
        {
            base.OnInit();
            if (ActionContainerHolder != null)
            {
                BlazorActionContainer actionContainer = new BlazorActionContainer()
                {
                    ContainerId = ContainerId,
                    DefaultActionID = DefaultActionID,
                    IsDropDown = IsDropDown,
                    AutoChangeDefaultAction = AutoChangeDefaultAction,
                };
                ActionContainerHolder.AddActionContainer(actionContainer);
            }
        }
    }
}
