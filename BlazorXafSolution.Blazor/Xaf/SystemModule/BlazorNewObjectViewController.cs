using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Xaf.SystemModule
{
    public class BlazorNewObjectViewController : NewObjectViewController
    {
        protected override void UpdateActionState()
        {
            base.UpdateActionState();
            NewObjectAction.BeginUpdate();
            try
            {
                NewObjectAction.Items.Clear();
                NewObjectAction.Items.AddRange(CollectDescendantItems());
                if (NewObjectAction.Items.Count > 0)
                {
                    NewObjectAction.SelectedItem = NewObjectAction.Items.FirstActiveItem;
                }
            }
            finally
            {
                NewObjectAction.EndUpdate();
            }
        }
    }
}
