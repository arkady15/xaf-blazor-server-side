using DevExpress.ExpressApp.Templates.ActionContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorXafSolution.Blazor.Xaf.Templates
{
    public class BlazorActionContainer : SimpleActionContainer
    {
        public string DefaultActionID { get; set; }
        public bool AutoChangeDefaultAction { get; set; }
        public bool IsDropDown { get; set; }
    }
}
