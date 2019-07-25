using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorXafSolution.Blazor.Components;
using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace BlazorXafSolution.Blazor.Xaf {
    public class BlazorGridListEditor : ListEditor {
        private List<object> selectedObjects = new List<object>();
        public BlazorGridListEditorComponent Component { get; private set; }
        public override SelectionType SelectionType => SelectionType.Full;

        public override IList GetSelectedObjects() {
            return selectedObjects;
        }
        public override void Refresh() { }
        protected override void AssignDataSourceToControl(object dataSource) { }
        protected override object CreateControlsCore() {
            Component.SettingsModel.SelectedDataRowChanged += SettingsModel_SelectedDataRowChanged;
            return Component;
        }

        private void SettingsModel_SelectedDataRowChanged(object sender, SelectedDataRowChangedEventArgs e) {
            selectedObjects.Clear();
            selectedObjects.Add(e.SelectedItem);
            OnSelectionChanged();
        }

        protected override void OnDataSourceChanged() {
            base.OnDataSourceChanged();
        }
        public void SetComponent(BlazorGridListEditorComponent component) {
            Component = component;
        }
        public BlazorGridListEditor(IModelListView model) : base(model) { }
    }
}
