using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorXafSolution.Blazor.Xaf;
using DevExpress.Persistent.Base;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorXafSolution.Blazor.Services
{
    public interface IBlazorApplicationProvider
    {
        BlazorApplication Application { get; }
    }
    public interface IViewSiteControl
    {
        void Render();
    }

    public class BlazorApplicationProvider<T> : IBlazorApplicationProvider where T : BlazorApplication
    {
        private static object syncRoot = new object();
        private IServiceProvider serviceProvider;
        private BlazorApplication application;
        public BlazorApplication Application {
            get {
                if (application == null)
                {
                    lock (syncRoot)
                    {
                        if (application == null)
                        {
                            application = ActivatorUtilities.CreateInstance<T>(serviceProvider);
                            application.Setup();
                        }
                    }
                }
                return application;
            }
        }

        public BlazorApplicationProvider(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
    }
}
