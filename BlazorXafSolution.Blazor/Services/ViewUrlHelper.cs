using Microsoft.AspNetCore.Components;

namespace BlazorXafSolution.Blazor.Services
{
    public class ViewUrlHelper
    {
        IUriHelper uriHelper;
        public ViewUrlHelper(IUriHelper uriHelper)
        {
            this.uriHelper = uriHelper;
        }
        public void NavigateToView(string viewId, string objectKey = null)
        {

            if (!string.IsNullOrEmpty(objectKey))
            {
                NavigateTo($"{viewId}/{objectKey}");
            }
            else
            {
                NavigateTo($"/{viewId}");
            }
        }
        private void NavigateTo(string url)
        {
            uriHelper.NavigateTo(url);
        }
    }
}
