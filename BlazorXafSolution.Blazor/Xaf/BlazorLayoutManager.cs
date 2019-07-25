using BlazorXafSolution.Blazor.Xaf.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Xaf {
    public class BlazorLayoutManager : LayoutManager {
        public override object LayoutControls(IModelNode layoutModel, ViewItemsCollection detailViewItems) {
            if (layoutModel is IModelSplitLayout) {
                return base.LayoutControls(layoutModel, detailViewItems);
            } else {
                RenderFragment root = (builder) => {
                    foreach (var propertyEditor in detailViewItems) {
                        if (propertyEditor is BlazorPropertyEditor blazorPropertyEditor) {
                            blazorPropertyEditor.CreateControl();
                            RenderFragment renderFragment = blazorPropertyEditor.RenderFragment;
                            builder.AddContent(0, renderFragment);
                        }
                    }
                };
                return root;
            }
        }
        protected override object GetContainerCore() {
            return null;
        }
    }
}
