using BlazorXafSolution.Blazor.Xaf.Editors;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorXafSolution.Blazor.Xaf {
    public class BlazorApplication : XafApplication {
        private Window mainWindow;
        //private IViewShortcutHolder viewShortcutHolder;
        public override Window MainWindow {
            get {
                return mainWindow;
            }
        }
        public IServiceProvider ServiceProvider { get; private set; }
        protected override LayoutManager CreateLayoutManagerCore(bool simple) {
            return new BlazorLayoutManager();
        }
        protected override Window CreateWindowCore(TemplateContext context, ICollection<Controller> controllers, bool isMain, bool activateControllersImmediately) {
            return new BlazorWindow(this, context, controllers, isMain, activateControllersImmediately);
        }
        protected override ShowViewStrategyBase CreateShowViewStrategy() {
            return new BlazorShowViewStrategy(this);
        }
        //public void SetViewShortcutHolder(IViewShortcutHolder viewShortcutHolder) {
        //    this.viewShortcutHolder = viewShortcutHolder;
        //}
        public Window CreateMainWindow() {
            IList<Controller> list = CreateControllers(typeof(Controller), true, null, null);
            mainWindow = CreateWindowCore(TemplateContext.ApplicationWindow, list, true, true);
            return mainWindow;
        }
        public BlazorApplication(IServiceProvider serviceProvider) {
            ServiceProvider = serviceProvider;
        }
    }
}
