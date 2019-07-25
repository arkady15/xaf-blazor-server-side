namespace BlazorXafSolution.Blazor.Services
{

    public interface IViewSiteControlProvider
    {
        IViewSiteControl ViewSiteControl { get; }
        void RegisterViewSiteControl(IViewSiteControl viewSiteControl);
    }
    public class ViewSiteControlProvider : IViewSiteControlProvider
    {
        public IViewSiteControl ViewSiteControl { get; private set; }

        public void RegisterViewSiteControl(IViewSiteControl viewSiteControl)
        {
            this.ViewSiteControl = viewSiteControl;
        }
    }
}
