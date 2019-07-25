using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlazorXafSolution.Blazor.Services;
using BlazorXafSolution.Blazor.Xaf;
using DevExpress.Blazor;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorXafSolution.Blazor.Components {
    public class SelectedDataRowChangedEventArgs : EventArgs {
        public object SelectedItem { get; private set; }
        public SelectedDataRowChangedEventArgs(object selectedItem) {
            SelectedItem = selectedItem;
        }
    }
    public class BlazorGridSettingsModel {
        public object Data { get; set; }
        public int PageSize { get; set; } = 20;
        public bool ShowFilterRow { get; set; } = false;
        public bool AllowDataRowSelection { get; set; } = true;
        public event EventHandler<SelectedDataRowChangedEventArgs> SelectedDataRowChanged;
        internal Action<object> SelectedDataRowChangedAction { get => (obj) => OnSelectionChanged(obj); }
        private void OnSelectionChanged(object selectedItem) {
            SelectedDataRowChanged?.Invoke(this, new SelectedDataRowChangedEventArgs(selectedItem));
        }
    }
    public class BlazorGridListEditorComponent : ComponentBase {
        [Parameter]
        protected ListView View { get; set; }

        //public DxDataGrid<Contact> DataGrid { get; set; }

        public BlazorGridSettingsModel SettingsModel { get; private set; }

        protected BlazorGridListEditor Editor;

        protected override void OnParametersSet() {

            Editor = View.Editor as BlazorGridListEditor;
            Editor.SetComponent(this);
            SettingsModel = new BlazorGridSettingsModel();
            object viewData = GetCollection(View.CollectionSource);
            SettingsModel.Data = ApplyLoadOptions();
            View.BreakLinksToControls();
            View.CreateControls();
            base.OnParametersSet();
        }
        private object ApplyLoadOptions() {
            object viewData = GetCollection(View.CollectionSource);
            return ApplyLoadOptions(viewData);
        }
        public void Render() {
            SettingsModel.Data = ApplyLoadOptions();
            StateHasChanged();
        }
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            MethodInfo loadGenericMethod = typeof(BlazorGridListEditorComponent).GetMethod(nameof(CreateComponent), BindingFlags.Static | BindingFlags.NonPublic);
            Type resultType = SettingsModel.Data.GetType();
            if (resultType.IsGenericType) {
                resultType = resultType.GetGenericArguments()[0];
            }
            MethodInfo loadMethod = loadGenericMethod.MakeGenericMethod(resultType);
            loadMethod.Invoke(null, new object[] { builder, this });
        }
        protected override bool ShouldRender() {
            bool test = base.ShouldRender();
            return test;
        }
        private static void CreateComponent<T>(RenderTreeBuilder builder, BlazorGridListEditorComponent gridListEditorComponent) {
            builder.OpenComponent<DxDataGrid<T>>(0);
            builder.AddAttribute(1, "Data", gridListEditorComponent.SettingsModel.Data);
            builder.AddAttribute(2, "PageSize", gridListEditorComponent.SettingsModel.PageSize);
            builder.AddAttribute(3, "AllowDataRowSelection", gridListEditorComponent.SettingsModel.AllowDataRowSelection);
            builder.AddAttribute(4, "ShowFilterRow", gridListEditorComponent.SettingsModel.ShowFilterRow);
            builder.AddAttribute(5, "SelectedDataRowChanged", gridListEditorComponent.SettingsModel.SelectedDataRowChangedAction);
            builder.AddAttribute(6, "ChildContent", (RenderFragment)((builder2) => {
                foreach (IModelColumn column in gridListEditorComponent.Editor.Model.Columns.Where(c => c.Index > -1)) {
                    builder2.OpenComponent<DxDataGridColumn>(7);
                    builder2.AddAttribute(8, "Field", column.PropertyName);
                    builder2.AddAttribute(9, "Caption", column.Caption);
                    builder2.CloseComponent();
                }
            }));
            builder.AddComponentReferenceCapture(9, (object obj) => {
                //gridListEditorComponent.DataGrid = (DxDataGrid<Contact>)obj;
            });
            builder.CloseComponent();
        }
        protected internal object ApplyLoadOptions(object targetObject) {
            MethodInfo loadGenericMethod = typeof(BlazorGridListEditorComponent).GetMethod(nameof(ApplyLoadOptionsGeneric), BindingFlags.Instance | BindingFlags.NonPublic);

            Type resultType = targetObject.GetType();
            if (resultType.IsGenericType) {
                resultType = resultType.GetGenericArguments()[0];
            }
            MethodInfo loadMethod = loadGenericMethod.MakeGenericMethod(resultType);
            return loadMethod.Invoke(this, new object[] { targetObject });
        }
        private IQueryable<ObjectType> ApplyLoadOptionsGeneric<ObjectType>(object target) {
            return ToQueryable<ObjectType>(target);
        }
        private IQueryable<ObjectType> ToQueryable<ObjectType>(object target) {
            IQueryable<ObjectType> queryable = null;
            if (target is IQueryable<ObjectType>) {
                queryable = (IQueryable<ObjectType>)target;
            } else if (target is IEnumerable<ObjectType>) {
                queryable = ((IEnumerable<ObjectType>)target).AsQueryable();
            } else if (target is ObjectType) {
                queryable = (new ObjectType[] { (ObjectType)target }).AsQueryable();
            }
            return queryable;
        }
        private object GetCollection(CollectionSourceBase collectionSource) {
            MethodInfo getCollectionGenericMethod = typeof(BlazorGridListEditorComponent).GetMethod(nameof(GetCollectionGeneric), BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo getCollectionMethod = getCollectionGenericMethod.MakeGenericMethod(collectionSource.ObjectTypeInfo.Type);
            object[] parameters = new object[] { collectionSource };
            object result = getCollectionMethod.Invoke(this, parameters);
            return result;
        }
        protected internal IEnumerable<ObjectType> GetCollectionGeneric<ObjectType>(CollectionSourceBase collectionSource) {
            object originalCollection = GetOriginalCollection(collectionSource);
            if (originalCollection != null) {
                IQueryable<ObjectType> queryableCollection = originalCollection as IQueryable<ObjectType>;
                if (queryableCollection != null) {
                    return queryableCollection;
                } else {
                    IObjectSpace objectSpace = collectionSource.ObjectSpace;
                    if (objectSpace.IsCollectionLoaded(originalCollection)) {
                        if (originalCollection is IEnumerable<ObjectType>) {
                            return (IEnumerable<ObjectType>)originalCollection;
                        } else {
                            return ((IEnumerable)originalCollection).Cast<ObjectType>();
                        }
                    } else {
                        CriteriaOperator collectionSourceCriteria = collectionSource.GetTotalCriteria();
                        if (CriteriaOperator.Equals(collectionSourceCriteria, null)) {
                            // LookupEditPropertyCollectionSource.Collection for DataSourcePropertyIsNullMode.SelectNothing mode has criteria
                            collectionSourceCriteria = objectSpace.GetCriteria(originalCollection);
                        }
                        IQueryable<ObjectType> query = objectSpace.GetObjectsQuery<ObjectType>();
                        CriteriaToExpressionConverter converter = new CriteriaToExpressionConverter();
                        return query.AppendWhere(converter, collectionSourceCriteria) as IQueryable<ObjectType>;
                    }
                }
            } else {
                // LookupEditPropertyCollectionSource.Collection is null for DataSourcePropertyIsNullMode.SelectNothing mode
                return new List<ObjectType>();
            }
        }
        private object GetOriginalCollection(CollectionSourceBase collectionSource) {
            object originalCollection = collectionSource.Collection;
            ProxyCollection proxyCollection = originalCollection as ProxyCollection;
            if (proxyCollection != null) {
                originalCollection = proxyCollection.OriginalCollection;
            }

            return originalCollection;
        }
    }
}
