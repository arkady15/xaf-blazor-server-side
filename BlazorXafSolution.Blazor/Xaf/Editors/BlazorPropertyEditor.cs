using DevExpress.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Xaf.Editors
{
    public class BlazorPropertyEditor : PropertyEditor
    {
        public BlazorPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        public RenderFragment RenderFragment { get; private set; }
        public DxTextBox TextBox { get; private set; }
        protected override object CreateControlCore()
        {
            RenderFragment = CreateBuilder();
            return RenderFragment;
        }
        public virtual RenderFragment CreateBuilder()
        {
            return (builder) =>
            {
                builder.OpenElement(0, "div");
                builder.OpenElement(1, "span");
                builder.AddContent(2, Caption);
                builder.CloseElement();
                builder.OpenComponent<DxTextBox>(3);
                builder.AddAttribute(4, "Text", PropertyValue == null ? "" : PropertyValue.ToString());
                builder.AddAttribute(5, "TextChanged", new Action<string>(value =>
                {
                    PropertyValue = value;
                    OnControlValueChanged();
                }));
                builder.AddAttribute(6, "TextExpression", RuntimeHelpers.TypeCheck<Expression<Func<string>>>(() =>
                    PropertyValue == null ? "" : PropertyValue.ToString()
                ));
                builder.AddComponentReferenceCapture(7, (object obj) =>
                {
                    TextBox = (DxTextBox)obj;
                });
                builder.CloseComponent();
                builder.CloseElement();
            };
        }
        protected override object GetControlValueCore()
        {
            return null;
        }

        protected override void ReadValueCore()
        {

        }
    }
}
