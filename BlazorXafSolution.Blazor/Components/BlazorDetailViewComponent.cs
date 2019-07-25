using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorXafSolution.Blazor.Components
{
    public class BlazorDetailViewComponent : ComponentBase
    {
        [Parameter]
        protected DetailView View { get; set; }
        protected RenderFragment Layout { get; set; }
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, Layout);
            builder.CloseElement();
        }
        protected override void OnParametersSet()
        {
            View.CreateControls();
            Layout = (RenderFragment)View.Control;
            base.OnParametersSet();
        }
    }
}


